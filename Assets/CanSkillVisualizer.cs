using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanSkillVisualizer : MonoBehaviour
{
    PlayerSkill skill;
    public Image skillIcon;

    void Start()
    {
        skill = GameManager.Instance.playerInst.GetComponent<PlayerSkill>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleSkillIconUI();
    }

    private void HandleSkillIconUI()
    {
        skillIcon.enabled = skill.canSkill;
    }
}
