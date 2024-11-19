using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyState
{
    public EnemyIdle(Enemy _enemy, EnemyStateChinge _stateMachinge, string _animBoolName) : base(_enemy, _stateMachinge, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (agent.velocity.magnitude > 0) stateMachinge.ChangeSate(enemy.enemyMoveState);
    }
    public override void Exit()
    {
        base.Exit();

    }

}
