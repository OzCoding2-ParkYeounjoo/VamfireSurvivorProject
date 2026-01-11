using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    #region 상태머신 및 선언
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    #endregion
    [Header("스탯")]
    public float maxHP = 10f;
    public float moveSpeed = 3.0f;
    private float _currentHP;
    public float detectRange = 5f;
    public float attackRange = 1.5f;

    public Rigidbody _rb { get; private set; }
    public Transform _target { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        StateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }
    private void OnEnable()
    {
        
        _currentHP = maxHP;
        if(_rb != null)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        if(PlayerController.Instance != null)
        {
            _target = PlayerController.Instance.transform;
        }
        StateMachine.Initialize(IdleState);
    }
    
    private void Update()
    {
        StateMachine.currentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        StateMachine.currentState.PhysicsUpdate();
    }
    public void TakeDamage(float damage)
    {
        _currentHP -= damage;
        Debug.Log($"으악! 남은체력 : {_currentHP}");
        StartCoroutine(FlashRed());
        if (_currentHP <= 0) OnDead();
        
    }
    System.Collections.IEnumerator FlashRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr != null)
        {
            Color originColor = sr.color;
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = originColor;
        }
    }
    public void OnDead()
    {
        if (GameManager.Instance != null) GameManager.Instance.AddkillCount();
        if (DataManager.instance != null) DataManager.instance.AddGold(10);
        if (GameManager.Instance != null && GameManager.Instance.expGmePrefab != null)
        {
            Instantiate(GameManager.Instance.expGmePrefab, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }
}
