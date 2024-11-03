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
    public enum Drift { NONE = 0, RIGHT = 1, LEFT = 2 };
    private Drift driftDirection;


    private bool accelerating;
    private bool driftingRight;
    private bool driftingLeft;

    private float horizontalValue;

    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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

    #region Input
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
            SetDriftDirection(Drift.RIGHT);
        else
            StopDriftingOnSide();
    }
    private void DriftLeftAction(InputAction.CallbackContext obj)
    {
        driftingLeft = obj.ReadValueAsButton();
        if (driftingLeft)
            SetDriftDirection(Drift.LEFT);
        else
            StopDriftingOnSide();
    }

    #endregion

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
        animator.SetFloat("HorizontalDirection", horizontalValue);
    }
    private void LookAtDirection()
    {
        if (rb.velocity.magnitude < 4)
            return;

        Vector3 startLookDir = transform.forward;
        Vector3 destinyLookDir = rb.velocity.normalized;

        transform.forward = Vector3.Lerp(startLookDir, destinyLookDir, Time.fixedDeltaTime * rotationSpeed);
    }

    #endregion

    #region Drift
    private void SetDriftDirection(Drift _driftDir)
    {
        driftDirection = _driftDir;
        animator.SetInteger("DriftState", (int)driftDirection);
    }
    private void DriftBehaviour()
    {
        HorizontalMovement();
        DriftMovement();
        LookAtDirection();
    }
    private void StopDriftingOnSide()
    {
        if (driftingRight)
            SetDriftDirection(Drift.RIGHT);
        else if (driftingLeft)
            SetDriftDirection(Drift.LEFT);
        else
            StopDrift();

    }
    private void StopDrift()
    {
        SetDriftDirection(Drift.NONE);
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
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3);

        if (rb)
        {
            Vector3 destinyPosition = transform.position + rb.velocity;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, destinyPosition);
            //destinyPosition.y = transform.position.y;
            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(transform.position, destinyPosition);
        }
    }

}
