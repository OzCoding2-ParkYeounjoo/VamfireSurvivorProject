using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DefaultPlayerData", menuName = "SO/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    [Header("기본 스텟")]
    public float maxHP = 100f;
    public float moveSpeed = 5.0f;

    [Header("스킬 세팅")]
    public float dashDuration = 0.2f;
    public float dashCooldown = 2.0f;
    public float attackCooldown = 0.5f;

    public float dashSpeed => moveSpeed * 3.0f;


}
