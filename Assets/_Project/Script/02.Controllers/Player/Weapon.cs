using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float baseDamage = 5f;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"충돌 감지! 닿은 상대 :{other.name} / 태그 {other.tag}");
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if(enemy != null)
            {
                float realDamage = baseDamage;
                if(PlayerController.Instance != null)
                {
                    realDamage = PlayerController.Instance.currentDamage;
                }
                enemy.TakeDamage(realDamage);
                Debug.Log($"적 {other.name} 타격 성공");
            }
        }
    }
}
