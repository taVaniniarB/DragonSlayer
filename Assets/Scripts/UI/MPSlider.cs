using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPSlider : MonoBehaviour
{
    public Slider mpBar;
    public GameObject target;
    private PlayerState _stat;
    // Start is called before the first frame update
    void Start()
    {
        _stat = target.GetComponent<PlayerState>();
        UpdateHPBar();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        mpBar.value = (float)_stat.curMP / (float)_stat.maxMP;
    }
}
