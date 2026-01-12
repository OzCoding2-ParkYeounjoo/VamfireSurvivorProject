using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemySO/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    [Header("기본 정보")]
    public string enemyName;

    [Header("전투 스탯")]
    public float maxHP;
    public float moveSpeed;
    public float damage;
    public float detectRange;
    public float attackRange;

    [Header("보상")]
    public int expReward;
    public int goldReward;
}
