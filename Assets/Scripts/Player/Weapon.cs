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
        Debug.Log($"검으로 {other.name}를 공격했다");

        if (other.gameObject.CompareTag("Monster"))
        {
            enemyHealth = other.GetComponent<EnemyHealth>();
        }
        else
        {
            // 드래곤의 collider는 드래곤의 자식 오브젝트이므로 EnemyHealth에 접근하기 위해 루트 오브젝트를 구한다
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
