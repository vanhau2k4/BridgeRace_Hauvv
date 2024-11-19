using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{

    protected PlayerStateMachinge stateMachinge;
    protected Player player;
    public MainJoystick joystick;
    private string animBoolName;

    public PlayerState(Player _player, PlayerStateMachinge _stateMachinge, string _animBoolName)
    {
        this.player = _player;
        this.stateMachinge = _stateMachinge;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        joystick = player.joystick;
    }
    public virtual void Update() 
    {
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
}
