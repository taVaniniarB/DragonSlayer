using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Player player;
    EnemyHealth enemyHealth;

    CameraShacker camShake;
    CameraShacker.CamShakeInfo camShakeInfo = new(0.2f, 0.5f, 100f);
/*    float camShakeTime = 0.2f;
    float camShakeAmplitude = 0.5f;
    float camShakeFrequency = 100f;*/

    void Start()
    {
        player = GameManager.Instance.playerInst;
        camShake = FindObjectOfType<CameraShacker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"������ {other.name}�� �����ߴ�");

        if (other.gameObject.CompareTag("Monster"))
        {
            enemyHealth = other.GetComponent<EnemyHealth>();
        }
        else
        {
            // �巡���� collider�� �巡���� �ڽ� ������Ʈ�̹Ƿ� EnemyHealth�� �����ϱ� ���� ��Ʈ ������Ʈ�� ���Ѵ�
            Transform rootTransform = other.transform.root;
            enemyHealth = rootTransform.GetComponent<EnemyHealth>();
        }

        if (enemyHealth != null)
        {
            enemyHealth.DecreaseHP(player.ATK);
            camShake.CameraShake(camShakeInfo);
        }
    }
}
