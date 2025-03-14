using System;
using UnityEngine;
using UnityEngine.InputSystem;

#region Game Feel Settings

[System.Serializable]
public class FallMultiplierSettings
{
    [Tooltip("Activar/desactivar el multiplicador de caída")]
    public bool enabled = true;
    [Tooltip("Multiplicador de gravedad cuando el jugador está cayendo")]
    public float fallMultiplier = 2f;
}

[System.Serializable]
public class FallControlSettings
{
    [Tooltip("Activar/desactivar el limitador de velocidad de caída")]
    public bool enabled = true;
    [Tooltip("Limitador de velocidad cuando el jugador está cayendo")]
    public float maxFallSpeed = 50f;
}

[System.Serializable]
public class JumpBufferSettings
{
    [Tooltip("Activar/desactivar el buffer de salto")]
    public bool enabled = true;
    [Tooltip("Tiempo (en segundos) durante el cual se guarda la intención de salto")]
    public float jumpBufferTime = 0.1f;
}

[System.Serializable]
public class JumpCutSettings
{
    [Tooltip("Activar/desactivar el salto variable (jump cut)")]
    public bool enabled = true;
    [Tooltip("Multiplicador para reducir la velocidad vertical al soltar el salto antes del pico")]
    public float jumpCutMultiplier = 0.5f;
}

[System.Serializable]
public class CoyoteJumpSettings
{
    [Tooltip("Activar/desactivar el coyote jump")]
    public bool enabled = true;
    [Tooltip("Tiempo (en segundos) durante el cual se permite saltar después de salir del suelo")]
    public float coyoteTime = 0.1f;
}

[System.Serializable]
public class SmoothMovementSettings
{
    [Tooltip("Activar/desactivar la aceleración y desaceleración suaves")]
    public bool enabled = true;
    [Tooltip("Tiempo en segundos para alcanzar la velocidad máxima")]
    public float accelerationTime = 0.1f;
    [Tooltip("Tiempo en segundos para detenerse cuando no hay input")]
    public float decelerationTime = 0.1f;
    [Tooltip("Si está activado, el jugador cambiará de dirección instantáneamente sin inercia")]
    public bool instantTurn = false;
}

#endregion

/**
 * PlayerMovement implementa:
 * 
 *      - Movimiento horizontal.
 *      - Salto vertical si el jugador está en el suelo (detección con raycast y layers físicas)
 *      - Multiplicador de caída
 *      - Limitador de velocidad en caida
 *      - Buffer de salto
 *      - Salto con fuerza variable
 *      - Coyote Jump
 *      - Movimiento suavizado
 *      
 * La activación, desactivación y parametrización de estos 6 comportamientos se realiza de forma cómoda desde el Inspector.
 * 
 * Se recomienda subir considerablemente la gravedad en la simulación física para mejorar el game feeling del salto.
 */

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Sombras")]
    public bool _recording;

    [SerializeField]
    public SombrasController _controller;

    double _startTime = 0;

    public GameObject colliderOnDead;


    [Header("Movimiento")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    [Header("Game Feel Settings")]
    [SerializeField] FallMultiplierSettings fallMultiplierSettings;
    [SerializeField] FallControlSettings fallControlSettings;
    [SerializeField] JumpBufferSettings jumpBufferSettings;
    [SerializeField] JumpCutSettings jumpCutSettings;
    [SerializeField] CoyoteJumpSettings coyoteJumpSettings;
    [SerializeField] SmoothMovementSettings smoothMovementSettings;

    [Header("Ground Detection")]
    [Tooltip("Capas que serán detectadas como suelo")]
    [SerializeField] LayerMask groundLayers;
    [Tooltip("Distancia del Raycast para detectar el suelo")]
    [SerializeField] float groundCheckDistance = 0.1f;
    [Tooltip("Posición desde donde se lanza el Raycast (relativa al jugador)")]
    [SerializeField] Vector2 groundCheckOffset = Vector2.zero;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool onGround;
    private bool shouldJump;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float currentSpeed;
    private float velocitySmoothing;

    private PlayerAudio playerAudio;




    private bool active = true;
    

    bool _jumpButtonPressed = false;
    public int _controllerIndex = 0; 



    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        _startTime = Time.fixedTime;
        playerAudio = GetComponentInChildren<PlayerAudio>();
    }

    public void DisableMovement()
    {
        active = false;
    }

    public void EnableMovement()
    {
        active = true;
    }

    // Método para manejar el movimiento (llamado por el Input System)
    public void OnMove(InputAction.CallbackContext context) {
        var customContext = SombraStorage.convertCallbackContext(context);

        if (_recording)
        {
            //record(customContext,SombraStorage.ActionType.MOVE);
        }
        OnMove(customContext);
    }

    // Método para manejar el salto (llamado por el Input System)
    public void OnJump(InputAction.CallbackContext context) {
        var customContext = SombraStorage.convertCallbackContext(context);
        if (_recording)
        {
            //record(customContext, SombraStorage.ActionType.JUMP);
        }
        //OnJump(customContext);
    }

    public void OnStopRecording(InputAction.CallbackContext context)
    {
        var customContext = SombraStorage.convertCallbackContext(context);
        if (context.started && _recording)
        {
            //record(customContext, SombraStorage.ActionType.STOP_RECORDING);

            _controller.stopRecording();
        }
    }



    // Método para manejar el movimiento (custom para guardar los callbacks)
    public void OnMove(SombraStorage.CustomCallbackContext context) {
        moveInput = context.valueVector2;
        //print(moveInput);
    }

    // Método para manejar el salto (custom para guardar los callbacks)
    public void OnJump(SombraStorage.CustomCallbackContext context) {


        if (!_recording)
        {
            //print("Started: " +context.started + " Canceled: " + context.canceled);

        }


        if (context.started) {
            if (jumpBufferSettings.enabled) {
                jumpBufferCounter = jumpBufferSettings.jumpBufferTime;
            } else {
                // Si el buffer está desactivado, intenta saltar de inmediato (aplicando coyote jump si está activo)
                if (onGround || (coyoteJumpSettings.enabled && coyoteTimeCounter > 0)) {
                    shouldJump = true;
                }
            }
        }
        // Al soltar el botón de salto, si el salto variable está activo, se aplica el Jump Cut.
        if (context.canceled) {
            if (jumpCutSettings.enabled) {
                JumpCut();
            }
        }
    }   





    // Aplica el salto variable: si el jugador suelta el botón mientras sube, reduce la velocidad vertical.
    private void JumpCut() {
        if (rb.linearVelocity.y > 0) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutSettings.jumpCutMultiplier);
        }
    }

    private void Update() {

        // Deteccion de suelo basada en raycast
        onGround = CheckGround();

        // Preparamos el salto si se cumplen las condiciones para saltar. El salto realmente se ejecuta en fixedUpdate()
        if (jumpBufferSettings.enabled && jumpBufferCounter > 0 && 
            (onGround || (coyoteJumpSettings.enabled && coyoteTimeCounter > 0))) {
            shouldJump = true;
        }

        // Actualiza el contador de coyote jump si está activo.
        if (onGround) {
            coyoteTimeCounter = coyoteJumpSettings.enabled ? coyoteJumpSettings.coyoteTime : 0;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Decrementa el contador del buffer de salto si está activo.
        if (jumpBufferCounter > 0) {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    //de momento mira solo jump y move
    void getFixedInput()
    {
        //getMove
        SombraStorage.CustomCallbackContext moveContext = new SombraStorage.CustomCallbackContext();

        moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);

        moveContext.valueVector2 = moveInput;
        

        //getJump


        SombraStorage.CustomCallbackContext jumpContext = new SombraStorage.CustomCallbackContext();

        jumpContext.started = false;
        jumpContext.canceled = false;

        bool thisFrameJumpButtonPressed = Input.GetButton("Jump");

        //si ha cambiado su estado
        if (thisFrameJumpButtonPressed != _jumpButtonPressed) {

            //si se ha pulsado
            if (thisFrameJumpButtonPressed)
            {
                jumpContext.started = true;
                jumpContext.canceled = false;
            }
            else // si se ha dejado de pulsar
            {
                jumpContext.started = false;
                jumpContext.canceled = true;

                print("canceleeed");

            }
            _jumpButtonPressed = thisFrameJumpButtonPressed;

            OnJump(jumpContext);
        }



        //getShoot


        //getAim


        //stop recording
        SombraStorage.CustomCallbackContext stopContext = new SombraStorage.CustomCallbackContext();

        stopContext.started = false;    

        if (Input.GetButton("Cbutton"))
        {
            stopContext.started = true;
        }

        //record de los inputs
        record(moveContext,SombraStorage.ActionType.MOVE);
        record(jumpContext,SombraStorage.ActionType.JUMP);
       // print("Action Nº: " + _controller._currentRecord.Count +  " Started: " +jumpContext.started + " Canceled: " + jumpContext.canceled);




        if (stopContext.started)
        {
            record(stopContext, SombraStorage.ActionType.STOP_RECORDING);
            _controller.stopRecording();
        }

    }

    void askInput()
    {
        if (SombraStorage.Instance._records[_controllerIndex].Count >  _controller._sombrasIndices[_controllerIndex])
        {

            double currTime = Time.fixedTime - _startTime;
            double actionTime = SombraStorage.Instance._records[_controllerIndex][_controller._sombrasIndices[_controllerIndex]].time;

            //print("CurrTime: " + currTime);
            //print("ActionTime: "+  actionTime);


            while(currTime >= actionTime && _controller._sombrasIndices[_controllerIndex] < SombraStorage.Instance._records[_controllerIndex].Count)
            {
                actionTime = SombraStorage.Instance._records[_controllerIndex][_controller._sombrasIndices[_controllerIndex]].time;
                SombraStorage.runAction(SombraStorage.Instance._records[_controllerIndex][_controller._sombrasIndices[_controllerIndex]], _controller._sombrasActivas[_controllerIndex]);
                _controller._sombrasIndices[_controllerIndex]++;
            }
        }
        else if (SombraStorage.Instance._records[_controllerIndex].Count ==  _controller._sombrasIndices[_controllerIndex]) //cuando se ha dejado de detectar el input
        {
            //cancelar todos los inputs
            if (_controller._sombrasActivas[_controllerIndex] != null)
            {
                _controller._sombrasActivas[_controllerIndex].colliderOnDead.SetActive(true);
                _controller._sombrasIndices[_controllerIndex]++;
            }
        }

    }


    private void FixedUpdate() {
        // Calcula la velocidad horizontal según la entrada.

        if (_recording) { 
            
            getFixedInput();
        
        }
        else //ask for input in sombraController
        {
            //print("asking");
            askInput();
        }



        if (active)
        {
            float horizontalVelocity = moveInput.x * moveSpeed;
            // Obtiene la velocidad vertical actual.
            float verticalVelocity = rb.linearVelocity.y;

            if (shouldJump)
            {
                verticalVelocity = jumpForce;
                shouldJump = false;

                // Reseteamos Coyote Jump y Jump Buffer al saltar
                coyoteTimeCounter = 0;
                jumpBufferCounter = 0;

                playerAudio.PlayJump();
            }

            // Si el jugador está cayendo y el multiplicador de caída está activo, se aplica mayor gravedad.
            if (fallMultiplierSettings.enabled && verticalVelocity < 0)
            {
                verticalVelocity += Physics2D.gravity.y * (fallMultiplierSettings.fallMultiplier - 1) * Time.fixedDeltaTime;
            }

            // Aplicamos el limitador de velocidad en caida
            if (fallControlSettings.enabled && verticalVelocity < -fallControlSettings.maxFallSpeed)
            {
                verticalVelocity = -fallControlSettings.maxFallSpeed;
            }

            // Suavizamos el movimiento si el smooth está habilitado
            if (smoothMovementSettings.enabled)
            {
                if (moveInput.x != 0)
                {
                    // Aplicamos aceleración si hay input.
                    currentSpeed = Mathf.SmoothDamp(
                        currentSpeed,
                        horizontalVelocity,
                        ref velocitySmoothing,
                        smoothMovementSettings.accelerationTime
                    );
                }
                else
                {
                    // Aplicamos desaceleración si no hay input.
                    currentSpeed = Mathf.SmoothDamp(
                        currentSpeed,
                        0,
                        ref velocitySmoothing,
                        smoothMovementSettings.decelerationTime
                    );
                }

                // Permite cambiar de dirección instantáneamente si está activado.
                if (smoothMovementSettings.instantTurn && moveInput.x != 0)
                {
                    currentSpeed = horizontalVelocity;
                }
            }
            else
            {
                currentSpeed = horizontalVelocity;
            }

            rb.linearVelocity = new Vector2(currentSpeed, verticalVelocity);
        }
    }

    /// <summary>
    /// Lanza un Raycast hacia abajo para detectar si estamos en el suelo.
    /// </summary>
    /// <returns>True si el Raycast detecta el suelo, False en caso contrario.</returns>
    private bool CheckGround() {
        Vector2 rayOrigin = (Vector2)transform.position + groundCheckOffset;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayers);
        Debug.DrawRay(rayOrigin, Vector2.down * groundCheckDistance, hit ? Color.green : Color.red); // Debug visual

        return hit.collider != null;
    }


    #region Sombras


    public void setRecording(bool value) { 
    
        _recording = value;
    }


    public void record(SombraStorage.CustomCallbackContext callback,SombraStorage.ActionType type)
    {
        SombraStorage.SombraAction sombraAction = new SombraStorage.SombraAction();

        //agregar la accion a la lista de acciones actuales
        sombraAction.callback = callback;
        sombraAction.time = Time.fixedTime - _startTime;
        sombraAction.type = type;


        _controller._currentRecord.Add(sombraAction);
    }


    private void OnDestroy()
    {
        if (_recording)
        {
            //_controller.stopRecording();
        }
    }

    #endregion





}
