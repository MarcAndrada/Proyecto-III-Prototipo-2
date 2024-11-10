using System;
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


    [Serializable]
    private struct Wheel
    {
        public WheelCollider wheelCol;
        public float currentTorque;
        public float currentBrakeForce;
    }
    [Space, Header("Wheels"), SerializeField]
    private Wheel rightWheel;
    private Wheel leftWheel;
    private Wheel tinyRightWheel;
    private Wheel tinyLeftWheel;


    [Space, Header("Move Variables"), SerializeField]
    private float impulseForce;
    [SerializeField]
    private float maxWheelSpeed;
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
        //MoveWheel(impulseForce, Random.Range(impulseTorque.x, impulseTorque.y));
    }
    private void MoveLeftWheelAction(InputAction.CallbackContext obj)
    {
        
        //MoveWheel(impulseForce, -Random.Range(impulseTorque.x, impulseTorque.y));
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
        ApplyMotorSpeed();
        CarBrakes();
    }

    private void ApplyMotorSpeed()
    {
        ApplyMotorTorque(rightWheel);
        ApplyMotorTorque(leftWheel);
        ApplyMotorTorque(tinyRightWheel);
        ApplyMotorTorque(tinyLeftWheel);
    }

    private void ApplyMotorTorque(Wheel _wheel)
    {

    }

    private void CarBrakes()
    {
        

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
