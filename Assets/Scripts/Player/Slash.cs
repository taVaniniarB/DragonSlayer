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
        Debug.Log($"Slash�� {other.name}�� ����");

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
            // �巡���� collider�� �巡���� �ڽ� ������Ʈ�̹Ƿ� EnemyHealth�� �����ϱ� ���� ��Ʈ ������Ʈ�� ���Ѵ�
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
