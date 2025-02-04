using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class UIManager : MonoBehaviour
{
    public GameObject fadePanelPrefab;
    GameObject fadePanelInst = null;


    private static UIManager _instance;
    public static UIManager Instance => _instance;

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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
    public void ShowFadePanel()
    {
        Debug.Log("ShowFadePanel called");
        // 패널 프리팹 객체화하여 캔버스 하위에 배치
        if (fadePanelPrefab != null)
        {
            GameObject fadeCanvas = GameObject.FindWithTag("FadeCanvas");
            if (fadeCanvas == null) { Debug.Log("캔버스 못 찾음"); }
            
            fadePanelInst = Instantiate(fadePanelPrefab, fadeCanvas.transform);
            if (fadePanelInst == null) { Debug.Log("fade panel 인스턴스화 실패"); }

            FadeManager.Instance.panel = fadePanelInst;
            fadePanelInst.SetActive(true);
        }
        else
            Debug.Log("Panel Prefab is null");
    }

    public void HideFadePanel()
    {
        if (fadePanelPrefab != null)
        {
            fadePanelInst.SetActive(false);
        }
    }
    public void UpdateCombatUI(GameState state)
    {
        CombatCanvasController combatUI = FindObjectOfType<CombatCanvasController>();
        if (combatUI)
            combatUI.UpdateUI(state);
    }

    public void EnableDieMenu(bool _b)
    {
        CombatCanvasController combatUI = FindObjectOfType<CombatCanvasController>();
        if (combatUI)
            combatUI.EnableDieMenu(_b);
    }
}
