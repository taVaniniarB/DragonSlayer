using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatCanvasController : MonoBehaviour
{
    public GameObject defaultCombatUI;
    public GameObject questUI;
    public TMP_Text QuestText;
    public GameObject bossBattleUI;
    public GameObject victoryUI;
    public Canvas dieMenu;
    public void UpdateUI(GameState state)
    {
        defaultCombatUI.SetActive(false);
        bossBattleUI.SetActive(false);
        questUI.SetActive(true);

        switch (state)
        {
            case GameState.Exploration:
                defaultCombatUI.SetActive(true);
                bossBattleUI.SetActive(false);
                QuestText.text = "���� ���� ���͸� ��� ��ġ�϶�";
                break;
            case GameState.PreparingBoss:
                defaultCombatUI.SetActive(true);
                bossBattleUI.SetActive(false);
                QuestText.text = "�巡���� ����϶�";
                break;
            case GameState.BossBattle:
                defaultCombatUI.SetActive(true);
                bossBattleUI.SetActive(true);
                break;
            case GameState.Victory:
                defaultCombatUI.SetActive(true);
                bossBattleUI.SetActive(true);
                questUI.SetActive(false);
                victoryUI.SetActive(true);
                break;
        }
    }

    public void EnableDieMenu(bool _b)
    {
        dieMenu.enabled = _b;
    }
}
