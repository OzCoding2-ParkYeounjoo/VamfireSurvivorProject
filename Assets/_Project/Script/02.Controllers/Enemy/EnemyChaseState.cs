using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyController enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(enemy._target == null) 
        {
            stateMachine.ChangeState(enemy.IdleState);
            return;
        }
        float distance = Vector3.Distance(enemy.transform.position, enemy._target.position);

        if(enemy._animator != null) enemy._animator.SetBool("isMoving", true);
        if (distance > enemy.detectRange *2f)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
        if(distance <= enemy.attackRange)
        {
            stateMachine.ChangeState(enemy.AttackState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (enemy._target == null) return;
        Vector3 dir = (enemy._target.position - enemy.transform.position).normalized;
        enemy._rb.velocity = dir * enemy.moveSpeed;
        if(dir.x > 0)
        {
            enemy.transform.localScale = new Vector3(1, 1, 1);
        }
        else if(dir.x < 0)
        {
            enemy.transform.localScale = new Vector3(-1, 1, 1);
        }
        
    }
}
