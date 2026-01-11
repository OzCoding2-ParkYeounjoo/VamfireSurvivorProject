using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float _attackTimer;
    private float _attackCoolDown = 1.5f;
    private float _damage = 10f;

    public EnemyAttackState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        enemy._rb.velocity = Vector3.zero;
        _attackTimer = 0f;
        Debug.Log("공격 상태 진입 성공");
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemy._target == null) return;
        float distance = Vector3.Distance(enemy.transform.position, enemy._target.position);
        if(distance > enemy.attackRange + 0.5f)
        {
            stateMachine.ChangeState(enemy.ChaseState);
            return;
        }
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= _attackCoolDown)
        {
            Attack();
            _attackTimer = 0f;
        }
    }
    private void Attack()
    {
        Debug.Log("적 공격 시도");
        PlayerController player = enemy._target.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(_damage);
            Debug.Log("플레이어에게 데미지 전달 성공");
        }
        else
        {
            Debug.Log("해당 플레이어의 스크립트를 찾지 못했습니다.");
        }
    }
    
}
