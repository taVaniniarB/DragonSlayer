using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPSlider : MonoBehaviour
{
    public Slider mpBar;
    public GameObject target;
    private PlayerStat _stat;
    // Start is called before the first frame update
    void Start()
    {
        _stat = target.GetComponent<PlayerStat>();
        mpBar.value = (float)_stat.curMP / (float)_stat.maxMP;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHPBar();
    }

    private void HandleHPBar()
    {
        mpBar.value = (float)_stat.curMP / (float)_stat.maxMP;
    }
}
