using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public PlayerStateMachinge stateMachinge {  get; private set; }
    #region States
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerMoveState playerMoveState { get; private set; }
    public PlayerNhayState playerNhayState { get; private set; }

    public MainJoystick joystick { get; private set; }
    public Transform followPoint;
    public Transform lookAtPoint;
    public CharacterController controller;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachinge = new PlayerStateMachinge();
        playerIdleState = new PlayerIdleState(this, stateMachinge, "Idle");
        playerMoveState = new PlayerMoveState(this, stateMachinge, "Move");
        playerNhayState = new PlayerNhayState(this, stateMachinge, "Nhay");
    }

    protected override void Start()
    {
        base.Start();

        joystick = FindObjectOfType<MainJoystick>();

        stateMachinge.Initialize(playerIdleState);
    }

    protected override void Update()
    {
        
        base.Update();
        stateMachinge.currentState.Update();
        velocity = joystick.velocity;
        CheckStairs();
    }
    public void Final()
    {
        stateMachinge.ChangeSate(playerNhayState);
        gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        controller.enabled = false;
        rb.useGravity = true;
        ClearBrick();
        joystick.isJovstick = false;
        joystick.movementDirection = Vector3.zero;
        /*joystick.velocity = Vector3.zero;
        controller.SimpleMove(Vector3.zero);*/
        joystick.inputCanvas.gameObject.SetActive(false);
    }

    public void CheckStairs()
    {
        if (Physics.Raycast(checkStair.transform.position, Vector3.down, out RaycastHit hit, checkStairDistance, lmStair))
        {
            if (hit.transform.TryGetComponent(out StairsCheck stairBrick))
            {
                if (listBrickHiden.Count == 0 && velocity.y > 0 && stairBrick.color != color)
                {
                    joystick.movementSpeed = 0; 
                    return;
                }
                joystick.movementSpeed = 10;

                if (stairBrick == null || listBrickHiden.Count < 1) return;

                Brick brick = listBrickHiden[listBrickHiden.Count - 1];
                if (stairBrick.color == color) return;
                AudioManager.instance.PlaySFX("EatStars");
                stairBrick.AddBrick(brick);
                stairBrick.ChangeStairColor(color);
                RemoveBrick(brick);
            }
        }
    }
}
