using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    public Slider hpBar;
    public GameObject target;
    private PlayerStat stat;
    void Start()
    {
        stat = target.GetComponent<PlayerStat>();
        hpBar.value = (float)stat.curHP / (float)stat.maxHP;
    }

    void Update()
    {
        HandleHPBar();
    }

    private void HandleHPBar()
    {
        hpBar.value = (float)stat.curHP / (float)stat.maxHP;
    }
}
