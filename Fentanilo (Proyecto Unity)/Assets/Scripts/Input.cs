using UnityEngine;
using UnityEngine.InputSystem;


public class Input : MonoBehaviour
{
    [SerializeField] private SombrasController controller;
    [SerializeField] private PlayerMovement playerMovement;

    private PlayerInput input;

    private Vector2 lastMoveInput;
    private bool lastShoot;
    private bool lastJump;

    private double startTime;

    void Start()
    {
        startTime = Time.fixedTime;
        input = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        double timeStamp = Time.fixedTime - startTime;

        // leer inputs
        Vector2 moveInput = input.actions["Move"].ReadValue<Vector2>();
        bool shoot = input.actions["Shoot"].IsPressed();
        bool jump = input.actions["Jump"].IsPressed();
        bool nextIteration = input.actions["NextIteration"].IsPressed();

        // mandar inputs a movement y eso

        playerMovement.MovementUpdate(moveInput, jump);

        // GRABAR

        // si manda nextIteration
        if (nextIteration)
        {
            controller._currentRecord.Add(new SombraStorage.SombraAction(new Vector2(0, 0), false, SombraStorage.ActionType.JUMP, timeStamp));
            controller.stopRecording();
        }

        // movimiento 
        if (lastMoveInput != moveInput)
        {
            controller._currentRecord.Add(new SombraStorage.SombraAction(moveInput, true, SombraStorage.ActionType.MOVE, timeStamp));
            //controller._currentRecord.Add(new SombraStorage.SombraAction(moveInput, true, SombraStorage.ActionType.AIM, timeStamp));
            lastMoveInput = moveInput;
        }

        if (lastShoot != shoot)
        {
            controller._currentRecord.Add(new SombraStorage.SombraAction(new Vector2(0,0), shoot, SombraStorage.ActionType.SHOOT, timeStamp));
            lastShoot = shoot;
        }

        if (lastJump != jump)
        {
            controller._currentRecord.Add(new SombraStorage.SombraAction(new Vector2(0, 0), jump, SombraStorage.ActionType.JUMP, timeStamp));
            lastShoot = jump;
        }

    }
}
