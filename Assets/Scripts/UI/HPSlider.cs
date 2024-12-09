using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    public Slider hpBar;
    public GameObject target;
    private PlayerState stat;
    void Start()
    {
        stat = target.GetComponent<PlayerState>();
        UpdateHPBar();
    }

    void Update()
    {
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        hpBar.value = (float)stat.curHP / (float)stat.maxHP;
    }
}
