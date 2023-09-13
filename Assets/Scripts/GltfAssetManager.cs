using GLTFast;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GltfAssetManager : MonoBehaviour {
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

                // Get the first material
                //var material = gltf.GetMaterial();
                //Debug.LogFormat("The first material is called {0}", material.name);

                // Instantiate the glTF's main scene
                await gltf.InstantiateMainSceneAsync(new GameObject("Instance 1").transform);
                // Instantiate the glTF's main scene
                await gltf.InstantiateMainSceneAsync(new GameObject("Instance 2").transform);

                // Instantiate each of the glTF's scenes
                for (int sceneId = 0; sceneId < gltf.SceneCount; sceneId++) {
                    await gltf.InstantiateSceneAsync(transform, sceneId);
                }
            } else {
                Debug.LogError("Loading glTF failed!");
            }
        } else {
            Debug.LogWarning("Failed to getting the glTF file from API");
        }
    }
}
