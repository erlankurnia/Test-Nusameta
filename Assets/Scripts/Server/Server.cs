using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Server {
    public static readonly string authorizationHeader = "Bearer " +
        "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1ZX" +
        "IiLCJVc2VybmFtZSI6IkphdmFJblVzZSIsImV4cCI6MTY5MzM5MTA2NiwiaWF0I" +
        "joxNjkzMzkxMDY2fQ.cZp8l8dlrce_hJbFSO4LnYGe1wfVNSJwpVC2xB3z26Q";
    public static readonly string baseUrl = "https://dev-uploader.nusameta.com/api/";

    public static Coroutine GetGLTFFile(UnityAction<bool, byte[]> callback) { 
        string url = baseUrl + "storage/test/35e7d6d9-c75b-4768-ad0b-1fd4369cc729";
        Debug.Log(url);
        WWWHandler.Instance.SetTimeout(60);
        return WWWHandler.Instance.Request(url, WWWHandler.HttpMethod.GET, callback);
    }
}
