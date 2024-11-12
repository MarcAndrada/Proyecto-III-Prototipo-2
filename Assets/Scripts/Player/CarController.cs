using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Input"), SerializeField]
    private InputActionReference moveRightWheelAction;
    [SerializeField]
    private InputActionReference moveLeftWheelAction;
    [SerializeField]
    private InputActionReference breakRightAction;
    [SerializeField]
    private InputActionReference breakLeftAction;

    [Space, Header("Move Variables"), SerializeField]
    private float impulseForce;
    [SerializeField]
    private Vector2 impulseTorque;

    [Space, Header("Break Variables"), SerializeField]
    private float minSpeedToBreak;
    [SerializeField]
    private float breakSpeed;
    [SerializeField]
    private float breakMovingTorque;
    [SerializeField]
    private float breakStaticTorque;
    private bool breakRight;
    private bool breakLeft;

    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        moveRightWheelAction.action.started += MoveRightWheelAction;
        moveLeftWheelAction.action.started += MoveLeftWheelAction;

        breakRightAction.action.started += BreakRightAction;
        breakRightAction.action.canceled += BreakRightAction;
        breakLeftAction.action.started += BreakLeftAction;
        breakLeftAction.action.canceled += BreakLeftAction;
    }
    private void OnDisable()
    {
        moveRightWheelAction.action.started -= MoveRightWheelAction;
        moveLeftWheelAction.action.started -= MoveLeftWheelAction;

        breakRightAction.action.started -= BreakRightAction;
        breakRightAction.action.canceled -= BreakRightAction;
        breakLeftAction.action.started -= BreakLeftAction;
        breakLeftAction.action.canceled -= BreakLeftAction;
    }

    #region Input
    private void MoveRightWheelAction(InputAction.CallbackContext obj)
    {
        MoveWheel(impulseForce, Random.Range(impulseTorque.x, impulseTorque.y));
    }
    private void MoveLeftWheelAction(InputAction.CallbackContext obj)
    {
        MoveWheel(impulseForce, -Random.Range(impulseTorque.x, impulseTorque.y));
    }

    private void BreakRightAction(InputAction.CallbackContext obj)
    {
        breakRight = obj.ReadValueAsButton();
    }
    private void BreakLeftAction(InputAction.CallbackContext obj)
    {
        breakLeft = obj.ReadValueAsButton();
    }
    #endregion

    private void FixedUpdate()
    {
        SetChairSpeed();
        CarBreaks();
    }

    private void SetChairSpeed()
    {
        Vector3 noYVelocity = rb.velocity;
        noYVelocity.y = 0;
        Vector3 forwardVelocity = transform.forward * noYVelocity.magnitude;
        forwardVelocity.y = rb.velocity.y;


        rb.velocity = forwardVelocity;
    }
    private void MoveWheel(float _impulseForce, float _torqueForce)
    {
        rb.AddForce(transform.forward * _impulseForce, ForceMode.Impulse);

        rb.AddRelativeTorque(0, _torqueForce, 0, ForceMode.Impulse);
    }

    private void CarBreaks()
    {
        if (!breakRight && !breakLeft)
            return;

        float torqueIntensity = breakStaticTorque;
        if (rb.velocity.magnitude >= minSpeedToBreak) 
        { 
            BreakDrag();
            torqueIntensity = breakMovingTorque;
        }

        BreakTorque(torqueIntensity);

    }
    private void BreakDrag()
    {
        Vector3 velocityNoY = rb.velocity;
        velocityNoY.y = 0;

        float multiplier = 0;

        multiplier += breakRight ? 1 : 0;
        multiplier += breakLeft ? 1 : 0;



        float finalMagnitude = velocityNoY.magnitude - breakSpeed * multiplier * Time.fixedDeltaTime;

        Vector3 finalVelocity = velocityNoY.normalized * finalMagnitude;
        finalVelocity.y = rb.velocity.y;

        rb.velocity = finalVelocity;

    }
    private void BreakTorque(float _torqueIntensity)
    {
        float breakDirection = 0;
        breakDirection += breakRight ? 1 : 0;
        breakDirection += breakLeft ? -1 : 0;


        if (breakDirection == 0)
            return;

        float torqueForce = _torqueIntensity * Time.fixedDeltaTime * breakDirection;
        rb.AddRelativeTorque(0, torqueForce, 0, ForceMode.Force);

    }
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
