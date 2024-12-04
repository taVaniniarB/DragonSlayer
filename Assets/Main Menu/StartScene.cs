using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class StartScene : MonoBehaviour
{
    [Header("Button")]
    public GameObject[] MainMenuBtn;
    public Color SelectedUIColor;
    public Color DefaultUIColor;
 
    int btnCnt;
    int SelectedBtnIdx = 0;
    
    int input = 0;

    [Header("Player")]
    public GameObject player;
    public GameObject dragon;
    Animator playerAnimator;

    void Start()
    {
        SelectedUIColor = new Color(0f, 0f, 0f, 0.9f);
        DefaultUIColor = new Color(0f, 0f, 0f, 0.55f);
        btnCnt = MainMenuBtn.Length;

        playerAnimator = player.GetComponent<Animator>();
    }

    void Update()
    {
        SetSelectedUI();
        SetUIColor();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteFunc();
        }
    }

    void ExecuteFunc()
    {
        switch (SelectedBtnIdx)
        {
            case 0:
                StartGame();
                break;
            case 1:
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
                Image image = MainMenuBtn[i].GetComponentInChildren<Image>();
                image.color = SelectedUIColor;
            }
            else
            {
                Image image = MainMenuBtn[i].GetComponentInChildren<Image>();
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

        if (input != 0)
        {
            SelectedBtnIdx = Mathf.Clamp((SelectedBtnIdx - input), 0, btnCnt - 1);
        }
    }

    void StartGame()
    {
        playerAnimator.SetTrigger("Start");
        //dragonCtrl.SetFireIntensity(1f);
        // µÂ∑°∞Ô¿Ã ∫“ª’¿Ω
        //SceneLoader.Instance.ChangeScene("The_Viking_Village");
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
