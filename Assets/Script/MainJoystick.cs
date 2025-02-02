using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainJoystick : MonoBehaviour
{
    public VariableJoystick joystick;
    public CharacterController controller;
    public float movementSpeed;
    public float rotationSpeed;
    public Vector3 velocity;
    public Canvas inputCanvas;
    public bool isJovstick;
    public Vector3 movementDirection;
    void Start()
    {
        EnableJoystickInput();
    }

    // Update is called once per frame
    void Update()
    {
        if (isJovstick && controller.enabled)
        {
            movementDirection = new Vector3(joystick.Direction.x, 0.0f, joystick.Direction.y);
            controller.SimpleMove(movementDirection * movementSpeed);
            velocity = new Vector3(movementDirection.x, joystick.Direction.y, movementDirection.z);
            if (movementDirection.sqrMagnitude <= 0)
            {
                return;
            }
            var targetDirection = Vector3.RotateTowards(controller.transform.forward, movementDirection,
                rotationSpeed * Time.deltaTime, 0.0f);
            controller.transform.rotation=Quaternion.LookRotation(targetDirection);
        }
    }

    public void EnableJoystickInput()
    {
        isJovstick = true;
        //inputCanvas.gameObject.SetActive(true);
    }
}
