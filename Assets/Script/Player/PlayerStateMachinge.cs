using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachinge
{
    public PlayerState currentState {  get; private set; }
    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }
    public void ChangeSate(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
