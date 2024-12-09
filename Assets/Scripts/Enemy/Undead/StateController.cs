using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    Animator animator;
    public float idleTime {  get; private set; }
    Player player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameManager.Instance.playerInst;
    }

    void Update()
    {
        animator.SetFloat("DistToPlayer", Vector3.Distance(player.transform.position, transform.position));
    }

    public void TriggerStateWithDelay(string curStateName, string StateNameToChange, float delay)
    {
        StartCoroutine(ChangeStateAfterDelay(curStateName, StateNameToChange, delay));
    }

    private IEnumerator ChangeStateAfterDelay(string curStateName, string StateNameToChange, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool(curStateName, true);
        animator.SetBool(StateNameToChange, false);
    }

    // attack �ִϸ��̼� ���� ������ ȣ��
    public void AttackCounter()
    {
        int attackCount = animator.GetInteger("AttackCount");
        animator.SetInteger("AttackCount", ++attackCount);
    }
}
