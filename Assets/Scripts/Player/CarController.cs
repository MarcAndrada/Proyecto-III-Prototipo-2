using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Input"), SerializeField]
    private InputActionReference accelerateAction;
    [SerializeField]
    private InputActionReference horizontalAction;
    [SerializeField]
    private InputActionReference driftRightAction;
    [SerializeField]
    private InputActionReference driftLeftAction;

    [Space, Header("Drive Variables"), SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float accelerationSpeed;
    [SerializeField]
    private float horizontalSpeed;
    [SerializeField]
    private float rotationSpeed;

    [Space, Header("Drift Variables"), SerializeField]
    private float driftSpeed;
    [SerializeField]
    private float driftDrag;
    public enum Drift { NONE, RIGHT, LEFT };
    private Drift driftDirection;


    private bool accelerating;
    private bool driftingRight;
    private bool driftingLeft;

    private float horizontalValue;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        accelerateAction.action.started += AccelerateAction;
        accelerateAction.action.canceled += AccelerateAction;

        horizontalAction.action.started += HorizontalAction;
        horizontalAction.action.performed += HorizontalAction;
        horizontalAction.action.canceled += HorizontalAction;

        driftRightAction.action.started += DriftRightAction;
        driftRightAction.action.canceled += DriftRightAction;
        
        driftLeftAction.action.started += DriftLeftAction;
        driftLeftAction.action.canceled += DriftLeftAction;
    }
    private void OnDisable()
    {
        accelerateAction.action.started -= AccelerateAction;
        accelerateAction.action.canceled -= AccelerateAction;

        horizontalAction.action.started -= HorizontalAction;
        horizontalAction.action.performed -= HorizontalAction;
        horizontalAction.action.canceled -= HorizontalAction;

        driftRightAction.action.started -= DriftRightAction;
        driftRightAction.action.canceled -= DriftRightAction;

        driftLeftAction.action.started -= DriftLeftAction;
        driftLeftAction.action.canceled -= DriftLeftAction;
    }
    private void AccelerateAction(InputAction.CallbackContext obj)
    {
        accelerating = obj.ReadValueAsButton();
    }

    private void HorizontalAction(InputAction.CallbackContext obj)
    {
        horizontalValue = obj.ReadValue<float>();
    }

    private void DriftRightAction(InputAction.CallbackContext obj)
    {
        driftingRight = obj.ReadValueAsButton();
        if (driftingRight)
        {
            driftDirection = Drift.RIGHT;
            Debug.Log("Esta drifteando a la derecha");
        }
        else
            StopDriftingOnSide();
    }
    private void DriftLeftAction(InputAction.CallbackContext obj)
    {
        driftingLeft = obj.ReadValueAsButton();
        if (driftingLeft)
        {
            driftDirection = Drift.LEFT;
            Debug.Log("Esta drifteando a la izquierda");
        }
        else
            StopDriftingOnSide();
    }


    void FixedUpdate()
    {
        if (driftDirection == Drift.NONE)
            DriveBehaviour();
        else
            DriftBehaviour();

    }

    #region Driving
    private void DriveBehaviour()
    {
        Accelerate();
        HorizontalMovement();
        LookAtDirection();
    }
    private void Accelerate()
    {
        if (accelerating)
            rb.AddForce(transform.forward * accelerationSpeed * Time.fixedDeltaTime, ForceMode.Force);
            
        if (rb.velocity.magnitude >= maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
    }
    private void HorizontalMovement()
    {
        rb.position = transform.position + transform.right * horizontalValue * horizontalSpeed * Time.fixedDeltaTime;
    }
    private void LookAtDirection()
    {
        Vector3 startLookDir = transform.forward;
        Vector3 destinyLookDir = rb.velocity.normalized;

        transform.forward = Vector3.Slerp(startLookDir, destinyLookDir, Time.fixedDeltaTime * rotationSpeed);
    }

    #endregion

    #region Drift
    private void DriftBehaviour()
    {
        HorizontalMovement();
        DriftMovement();
        LookAtDirection();
    }
    private void StopDriftingOnSide()
    {
        if (driftingRight)
            driftDirection = Drift.RIGHT;
        else if (driftingLeft)
            driftDirection = Drift.LEFT;
        else
            StopDrift();


        switch (driftDirection)
        {
            case Drift.NONE:
                Debug.Log("No esta drifteando a ningun lado");
                break;
            case Drift.RIGHT:
                Debug.Log("Esta drifteando a la derecha, porque ha dejado de hacerlo en la izquierda");
                break;
            case Drift.LEFT:
                Debug.Log("Esta drifteando a la izquierda, porque ha dejado de hacerlo en la derecha");
                break;
            default:
                break;
        }
    }
    private void StopDrift()
    {
        driftDirection = Drift.NONE;
        //Rotar hacia el forward
    }
    private void DriftMovement()
    {
        //Calcular direccion de giro
        Vector3 rotationDirection = driftDirection == Drift.RIGHT ? transform.right : -transform.right;

        //Velocidad de direccion de drift
        Vector3 driftDir = Vector3.Slerp(rb.velocity.normalized, rotationDirection, Time.fixedDeltaTime * driftSpeed);

        rb.velocity = driftDir * (rb.velocity.magnitude - (driftDrag * Time.fixedDeltaTime));
    }
    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(rb)
            Gizmos.DrawLine(transform.position, transform.position + rb.velocity);
    }

}
