using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class AdditiveSceneLoader : MonoBehaviour
    {
        public static AdditiveSceneLoader Instance;

        private AsyncOperation _pendingLoad;
        private int _pendingSceneIndex = -1;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        
        public AsyncOperation StartLoadAdditiveAsync(int sceneIndex)
        {
            if (_pendingLoad != null)
            {
                Debug.LogWarning("A scene is already loading. Discarding previous load first.");
                DiscardLoadedScene();
            }

            _pendingSceneIndex = sceneIndex;
            _pendingLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            if (_pendingLoad != null)
                _pendingLoad.allowSceneActivation = false;

            return _pendingLoad;
        }

        
        public void ActivateLoadedScene()
        {
            if (_pendingLoad == null)
            {
                Debug.LogWarning("No pending loaded scene to activate.");
                return;
            }

            _pendingLoad.allowSceneActivation = true;
            _pendingLoad = null;
            _pendingSceneIndex = -1;
        }

        
        public void DiscardLoadedScene()
        {
            if (_pendingLoad == null && _pendingSceneIndex == -1)
            {
                Debug.LogWarning("No pending loaded scene to discard.");
                return;
            }

            if (_pendingSceneIndex >= 0)
            {
                var scene = SceneManager.GetSceneByBuildIndex(_pendingSceneIndex);
                if (scene.IsValid() && scene.isLoaded)
                {
                    SceneManager.UnloadSceneAsync(_pendingSceneIndex);
                }
            }

            _pendingLoad = null;
            _pendingSceneIndex = -1;
        }
    }
}