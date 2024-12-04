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
        // ���̵�ƿ� ȿ��
        FadeManager.Instance.FadeOut(1f);
        yield return new WaitForSeconds(1f);

        // ���� �� ��ε�
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        yield return unloadOp;

        // ���θ޴��� �̵��ϴ� ���, ���ӸŴ����� ���� ���� �����ϴ� �̱��� �Ŵ����̹Ƿ� �����ش�
        if(sceneName == "MainMenu" && GameManager.Instance)
        {
            Destroy(GameManager.Instance.gameObject);
            GameManager.Instance.Clear(); // �̱��� ���� ����
        }

        // �� �� �ε�
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        loadOp.allowSceneActivation = true;
        while (!loadOp.isDone)
        {
            yield return null; // �ε� ���� ��
        }
        Debug.Log("�ε� �Ϸ�");
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

