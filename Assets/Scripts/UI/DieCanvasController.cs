using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieCanvasController : MonoBehaviour
{
    [Header("Button")]
    public GameObject[] dieMenuBtn;
    public Color SelectedUIColor;
    public Color DefaultUIColor;

    int btnCnt;
    int SelectedBtnIdx = 0;

    int input = 0;
    void Start()
    {
        SelectedUIColor = new Color(0f, 0f, 0f, 0.9f);
        DefaultUIColor = new Color(0f, 0f, 0f, 0.55f);
        btnCnt = dieMenuBtn.Length;
    }

    void Update()
    {
        SetSelectedUI();
        SetUIColor();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteFunc();
            UIManager.Instance.EnableDieMenu(false);
        }
    }

    void ExecuteFunc()
    {
        switch (SelectedBtnIdx)
        {
            case 0:
                RestartGame();
                break;
            case 1:
                GotoMainMenu();
                break;
            case 2:
                QuitGame();
                break;
            default:
                break;
        }
    }

    private void SetUIColor()
    {
        for (int i = 0; i < btnCnt; i++)
        {
            if (i == SelectedBtnIdx)
            {
                Image image = dieMenuBtn[i].GetComponentInChildren<Image>();
                image.color = SelectedUIColor;
            }
            else
            {
                Image image = dieMenuBtn[i].GetComponentInChildren<Image>();
                image.color = DefaultUIColor;
            }
        }
    }

    private void SetSelectedUI()
    {
        if (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))
            input = 1;
        else if (Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.DownArrow))
            input = -1;
        else
            return;

        if (input != 0)
        {
            SelectedBtnIdx = Mathf.Clamp((SelectedBtnIdx - input), 0, btnCnt - 1);
        }
    }
    void GotoMainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }
    void RestartGame()
    {
        SceneLoader.Instance.LoadGameScene();
    }
    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
