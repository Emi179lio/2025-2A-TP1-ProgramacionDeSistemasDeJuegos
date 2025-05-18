using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Excercise1
{
    public class GameManager : MonoBehaviour
    {
        //this is used to make sure the most critical scenes are loaded early,
        //regardless of load order
        [SerializeField] private List<SceneRef> prioritaryScenes = new(); 
        [SerializeField] private List<SceneRef> scenes = new();

        private async void Start()
        {
            foreach (var prioritaryScene in prioritaryScenes)
            {
                var loadSceneAsync = SceneManager.LoadSceneAsync(prioritaryScene.Index, LoadSceneMode.Additive);
                if (loadSceneAsync == null)
                    continue;
                await loadSceneAsync;
            }
            
            foreach (var scene in scenes)
            {
                var loadSceneAsync = SceneManager.LoadSceneAsync(scene.Index, LoadSceneMode.Additive);
                if (loadSceneAsync == null)
                    continue;
                await loadSceneAsync;
            }
        }
    }
}
