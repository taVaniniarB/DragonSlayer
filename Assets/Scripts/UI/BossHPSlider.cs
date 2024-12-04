using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPSlider : MonoBehaviour
{
    public Slider hpBar;
    public GameObject target;
    private EnemyHealth enemyHealth;
    void Start()
    {
        enemyHealth = target.GetComponent<EnemyHealth>();
        hpBar.value = (float)enemyHealth.curHP / (float)enemyHealth.maxHP;
    }
    
    void Update()
    {
        HandleHPBar();
    }

    private void HandleHPBar()
    {
        hpBar.value = (float)enemyHealth.curHP / (float)enemyHealth.maxHP;
    }
}
