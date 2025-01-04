using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    public GameState currentState = GameState.Exploration;
    public bool allMonstersDefeated = false;
    public bool bossAreaEntered = false;
    public List<Undead> aliveMonsters;

    public GameObject areaDivider;
    public GameObject bossAreaEnterChecker;
    public GameObject bossMonster;
    public Player playerInst { get; private set; }

    public float bossDefeatToMenuTime = 8f; // ���� óġ �� ���θ޴��� ���ư�������� ��� �ð�


    private static GameManager _instance;
    public static GameManager Instance => _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            playerInst = FindObjectOfType<Player>();
            if (playerInst != null)
            {
                Debug.Log("Player Register Complete");
            }
        }
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
    public void Clear()
    {
        _instance = null;
    }

    public void StartGame()
    {
        ChangeState(GameState.Exploration);
        UIManager.Instance.EnableDieMenu(false);

        Debug.Log("Game Start");
    }

    void Update()
    {
        if (bossAreaEntered && allMonstersDefeated)
        {
            StartBossBattle();
        }
    }


    void StartBossBattle()
    {
        bossMonster.SetActive(true);
        bossAreaEnterChecker.SetActive(false);
        areaDivider.SetActive(true);
        ChangeState(GameState.BossBattle);
    }

    public void RemoveMonsterFromList(Undead undead)
    {
        aliveMonsters.Remove(undead);

        if (aliveMonsters.Count == 0)
        {
            BossBattleReady();
        }
    }

    private void BossBattleReady()
    {
        Debug.Log("������ �غ�");
        allMonstersDefeated = true;
        bossAreaEnterChecker.SetActive(true);
        areaDivider.SetActive(false);
        ChangeState(GameState.PreparingBoss);
    }

    void ChangeState(GameState state)
    {
        currentState = state;
        UIManager.Instance.UpdateCombatUI(currentState);
    }

    public void RegisterPlayer(Player player)
    {
        playerInst = player;
        if (playerInst == null)
            Debug.Log("�÷��̾� �ν��Ͻ��� null");
    }

    public void BossDefeated()
    {
        UIManager.Instance.UpdateCombatUI(GameState.Victory);
        StartCoroutine(BackToMenu(bossDefeatToMenuTime));
    }

    public IEnumerator BackToMenu(float time)
    {
        yield return new WaitForSeconds(time);
        SceneLoader.Instance.LoadMainMenu();
    }

    
}
