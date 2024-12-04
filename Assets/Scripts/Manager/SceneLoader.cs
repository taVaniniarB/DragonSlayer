using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _instance;
    public static SceneLoader Instance => _instance;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
        StopAllCoroutines();
    }
    private IEnumerator LoadScene(string sceneName)
    {
        // 페이드아웃 효과
        FadeManager.Instance.FadeOut(1f);
        yield return new WaitForSeconds(1f);

        // 현재 씬 언로드
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        yield return unloadOp;

        // 메인메뉴로 이동하는 경우, 게임매니저는 전투 씬만 관리하는 싱글턴 매니저이므로 없애준다
        if(sceneName == "MainMenu" && GameManager.Instance)
        {
            Destroy(GameManager.Instance.gameObject);
            GameManager.Instance.Clear(); // 싱글턴 참조 제거
        }

        // 새 씬 로드
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        loadOp.allowSceneActivation = true;
        while (!loadOp.isDone)
        {
            yield return null; // 로딩 진행 중
        }
        Debug.Log("로딩 완료");
        FadeManager.Instance.FadeIn(1f);
    }

    public void LoadMainMenu()
    {
        ChangeScene("MainMenu");
    }
    public void LoadGameScene()
    {
        ChangeScene("The_Viking_Village");
    }

    private void ChangeScene(string SceneName)
    { StartCoroutine(LoadScene(SceneName)); }
}

