using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Core
{
    public class AppController : MonoBehaviour
    {
        private Coroutine loadSceneCoroutine;
        private static AppController instance;
        public static AppController Instance => instance;
        public AudioClip audioButtonClick;
        private Scene loadedScene;

        private void Awake()
        {
            instance = this;
        }

        private IEnumerator Start()
        {
            Cooking.Database.UserData.Initialize();
            yield return null;
            if (SceneManager.sceneCount <= 1)
            {
                LoadScene(Global.SceneMenu);
            }
            else
            {
                PanelFade.Instance.Hide();
            }
            //LoadScene(Global.SceneMenu);
        }

        public void LoadGameScene(bool isHidePanelFade = true, UnityAction onComplete = null)
        {
            LoadScene(Global.SceneGame, isHidePanelFade, onComplete);
        }

        public void LoadScene(string sceneName, bool hidePanelFade = true, UnityAction onComplete = null)
        {
            if (loadSceneCoroutine != null)
                StopCoroutine(loadSceneCoroutine);

            TouchController.Instance.DisableInput();
            PanelFade.Instance.Show(() =>
            {
                loadSceneCoroutine = StartCoroutine(LoadSceneAsync(sceneName, hidePanelFade, onComplete));
            });

        }

        private IEnumerator LoadSceneAsync(string sceneName, bool hidePanelFade, UnityAction onComplete)
        {
            AsyncOperation asyncOperation;
            if (loadedScene.IsValid())
            {
                asyncOperation = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(loadedScene.name));
                while (!asyncOperation.isDone) yield return null;
            }

            asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!asyncOperation.isDone)
                yield return null;

            loadedScene = SceneManager.GetSceneByName(sceneName);
            if (hidePanelFade)
                PanelFade.Instance.Hide();
            TouchController.Instance.EnableInput();

            onComplete?.Invoke();
        }

        private IEnumerator UnloadSceneAsync(string sceneName, UnityAction onComplete)
        {
            var asyncOperation = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
            while (!asyncOperation.isDone) yield return null;
            onComplete?.Invoke();
        }

        private bool IsSceneLoaded(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            return scene.isLoaded;
        }

        public void OnButtonClick(ButtonEx buttonEx)
        {
            AudioManager.Instance.PlaySfx(audioButtonClick);
            buttonEx.transform.DOKill();
            buttonEx.transform.localScale = Vector3.one;
            buttonEx.transform.DOPunchScale(Vector3.one * -0.15f, 0.2f, 1, 1);
        }
    }
}