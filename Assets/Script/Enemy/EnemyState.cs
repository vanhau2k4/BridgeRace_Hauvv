using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState
{
    protected EnemyStateChinge stateMachinge;
    protected Enemy enemy;
    private string animBoolName;
    public NavMeshAgent agent;
    public EnemyState(Enemy _enemy, EnemyStateChinge _stateMachinge, string _animBoolName)
    {
        this.enemy = _enemy;
        this.stateMachinge = _stateMachinge;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        enemy.anim.SetBool(animBoolName, true);
    }
    public virtual void Update()
    {
        agent = enemy.agent;
    }
    public virtual void Exit()
    {
        enemy.anim.SetBool(animBoolName, false);
    }
}
