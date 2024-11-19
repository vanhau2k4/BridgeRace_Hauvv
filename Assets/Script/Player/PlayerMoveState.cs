using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerMoveState : PlayerState
{

    public PlayerMoveState(Player _player, PlayerStateMachinge _stateMachinge, string _animBoolName) : base(_player, _stateMachinge, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (joystick.movementDirection.sqrMagnitude == 0) stateMachinge.ChangeSate(player.playerIdleState);
    }
}
