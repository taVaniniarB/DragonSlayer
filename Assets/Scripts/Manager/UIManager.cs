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
        // �г� ������ ��üȭ�Ͽ� ĵ���� ������ ��ġ
        if (fadePanelPrefab != null)
        {
            // fadeCanvas �±׸� ���� ĵ������ �ϳ� �߰�������
            Canvas canvas = FindObjectOfType<Canvas>();
            //Canvas fadeCanvas = FindObjectOfType<FadeCanvas>();
            if (canvas == null) { Debug.Log("ĵ���� �� ã��"); }
            
            //Instantiate(Object original, Transform parent);
            fadePanelInst = Instantiate(fadePanelPrefab, canvas.transform);
            if (fadePanelInst == null) { Debug.Log("fade panel �ν��Ͻ�ȭ ����"); }

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
