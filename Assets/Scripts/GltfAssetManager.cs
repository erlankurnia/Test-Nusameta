using GLTFast;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GltfAssetManager : MonoBehaviour {
    [SerializeField]
    private Transform cameraObj;

    [SerializeField, Tooltip("X = columns, Y = rows")]
    private Vector2Int objectsGrid = new(1, 1);

    [SerializeField]
    private float gridSpace = 0;

    private void Start() {
        Load();
    }

    public void Load() {
        Server.GetGLTFFile(GetGLTFFileCallback);
    }

    private async void GetGLTFFileCallback(bool success, byte[] results) {
        if (success) {
            // First step: load glTF
            var gltf = new GLTFast.GltfImport();
            success = await gltf.Load(results);

            if (success) {
                // Here you can customize the post-loading behavior
                Debug.Log("Success! gltf file is loaded");

                // Instantiate each of the glTF's scenes
                for (int sceneId = 0; sceneId < gltf.SceneCount; sceneId++) {
                    var clips = gltf.GetAnimationClips();
                    List<string> clipsName = new();

                    foreach (var clip in clips) {
                        Debug.Log(clip.name);
                        clipsName.Add(clip.name);
                    }

                    for (int y = 0; y < objectsGrid.y; y++) {
                        for (int x = 0; x < objectsGrid.x; x++) {
                            var rootObject = new GameObject($"{gltf.GetSceneName(sceneId)}_{y}_{x}").transform;
                            rootObject.transform.rotation = Quaternion.Euler(0, Random.Range(0, 36) * 10, 0);
                            rootObject.transform.position = new(x * gridSpace, 0, y * gridSpace);

                            await gltf.InstantiateMainSceneAsync(rootObject);
                            
                            Character ch = rootObject.gameObject.AddComponent<Character>();
                            ch.Init(objectsGrid.x * y + x, clipsName.ToArray());
                        }
                    }
                }

                cameraObj.position = new((gridSpace * objectsGrid.x - gridSpace) / 2, cameraObj.position.y, cameraObj.position.z);
            } else {
                Debug.LogError("Loading glTF failed!");
            }
        } else {
            Debug.LogWarning("Failed to getting the glTF file from API");
        }
    }
}
