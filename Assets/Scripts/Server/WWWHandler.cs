using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WWWHandler : MonoBehaviour {
    public enum HttpMethod {
        GET = 0, POST, PUT, DELETE
    }

    private static WWWHandler instance;
    public static WWWHandler Instance {
        get {
            instance = FindObjectOfType<WWWHandler>();
            instance = instance != null ? instance : new GameObject("WWWHandler").AddComponent<WWWHandler>();
            return instance;
        }
    }

    private float defaultTimeout = 16f;
    private float timeout = 0;

    private void Awake() {
        if (instance != null) {
            DontDestroyOnLoad(instance.gameObject);
        }
        timeout = defaultTimeout;
    }

    public void SetTimeout(float second) {
        timeout = second;
    }

    public Coroutine Request(string url, HttpMethod method, UnityAction<bool, string> callback, Dictionary<string, string> customHeader = null) {
        return StartCoroutine(Requesting(url, method, null, callback, null, customHeader));
    }

    public Coroutine Request(string url, HttpMethod method, WWWForm form, UnityAction<bool, string> callback, Dictionary<string, string> customHeader = null) {
        return StartCoroutine(Requesting(url, method, form, callback, null, customHeader));
    }

    public Coroutine Request(string url, HttpMethod method, UnityAction<bool, byte[]> callback, Dictionary<string, string> customHeader = null) {
        return StartCoroutine(Requesting(url, method, null, null, callback, customHeader));
    }

    public Coroutine Request(string url, HttpMethod method, WWWForm form, UnityAction<bool, byte[]> callback, Dictionary<string, string> customHeader = null) {
        return StartCoroutine(Requesting(url, method, form, null, callback, customHeader));
    }

    private IEnumerator Requesting(string url, HttpMethod method, WWWForm form, UnityAction<bool, string> Callback, UnityAction<bool, byte[]> Callback2, Dictionary<string, string> customHeader) {
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        bool success = false;
        string results = "";

        switch (method) {
            case HttpMethod.POST:
                if (form != null) uwr = UnityWebRequest.Post(url, form);
                break;
            case HttpMethod.PUT:
                if (form != null) uwr = UnityWebRequest.Put(url, form.data);
                break;
            case HttpMethod.DELETE:
                uwr = UnityWebRequest.Delete(url);
                break;
        }

        using (uwr) {
            if (customHeader == null || customHeader.Count == 0) {
                uwr.SetRequestHeader("Authorization", Server.authorizationHeader);
            } else {
                foreach (KeyValuePair<string, string> kvp in customHeader) {
                    uwr.SetRequestHeader(kvp.Key, kvp.Value);
                    yield return null;
                }
            }

            Debug.Log("Send WWW Request");
            uwr.SendWebRequest();
            float currentTIme = 0;

            while (!uwr.isDone) {
                if (currentTIme >= timeout) {
                    StopAllCoroutines();
                    
                    Callback?.Invoke(success, "Internet connection too slow");
                    uwr.Abort();
                    Debug.LogWarning("Internet connection too slow");
                    yield break;
                } else {
                    currentTIme += Time.unscaledDeltaTime;
                    //Debug.Log("CurrentTime: " + currentTIme);
                }
                yield return null;
            }

            if (uwr.result != UnityWebRequest.Result.Success) {
                Debug.LogWarning("uwr.result: " + uwr.result);
                Debug.LogWarning("uwr.timeout: " + uwr.timeout);
                Debug.LogWarning("uwr.downloadHandler.error: " + uwr.downloadHandler.error);
                results = uwr.error;
                success = false;
            } else {
                results = uwr.downloadHandler.text;
                success = true;
            }

            Callback?.Invoke(success, results);
            Callback2?.Invoke(success, uwr.downloadHandler.data);
            yield return null;
        }

        timeout = defaultTimeout;
        yield return null;
    }
}
