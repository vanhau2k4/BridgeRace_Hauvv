using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNhayState : PlayerState
{
    public PlayerNhayState(Player _player, PlayerStateMachinge _stateMachinge, string _animBoolName) : base(_player, _stateMachinge, _animBoolName)
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
        if (joystick.movementDirection.sqrMagnitude > 0) stateMachinge.ChangeSate(player.playerMoveState);
    }
}
