using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    Player player;
    public ParticleSystem ps;

    CameraShacker camShake;
    CameraShacker.CamShakeInfo camShakeInfo = new(0.3f, 0.7f, 200f);

    private void Start()
    {
        player = GameManager.Instance.playerInst;
        camShake = FindObjectOfType<CameraShacker>();
        ps.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Slash가 {other.name}를 공격");

        if (other.gameObject.CompareTag("Monster"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.DecreaseHP(player.SkillDamage);
                camShake.CameraShake(camShakeInfo);
            }
        }
        else
        {
            // 드래곤의 collider는 드래곤의 자식 오브젝트이므로 EnemyHealth에 접근하기 위해 루트 오브젝트를 구한다
            Transform rootTransform = other.transform.root;
            EnemyHealth enemyHealth = rootTransform.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.DecreaseHP(player.SkillDamage);
                camShake.CameraShake(camShakeInfo);
            }
        }
        Destroy(gameObject);
    }
}
