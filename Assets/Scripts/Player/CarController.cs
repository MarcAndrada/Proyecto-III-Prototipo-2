using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{

    [Header("Input"), SerializeField]
    private InputActionReference accelerateAction;
    [SerializeField]
    private InputActionReference horizontalAction;
    [SerializeField]
    private InputActionReference driftAction;

    [Space, Header("Speed Variables"), SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float accelerationSpeed;
    [SerializeField]
    private float horizontalSpeed;

    private bool accelerating;
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

    }

    private void OnDisable()
    {
        accelerateAction.action.started -= AccelerateAction;
        accelerateAction.action.canceled -= AccelerateAction;

        horizontalAction.action.started -= HorizontalAction;
        horizontalAction.action.performed -= HorizontalAction;
        horizontalAction.action.canceled -= HorizontalAction;
        
    }
    private void AccelerateAction(InputAction.CallbackContext obj)
    {
        accelerating = obj.ReadValueAsButton();
    }

    private void HorizontalAction(InputAction.CallbackContext obj)
    {
        horizontalValue = obj.ReadValue<float>();
    }

    private void DriftAction(InputAction.CallbackContext obj)
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Accelerate();
        HorizontalMovement();
    }

    private void Accelerate()
    {
        Debug.Log("La aceleracion es " + accelerating);
        Debug.Log("Velocidad | " + rb.velocity.magnitude);

        if (accelerating)
            rb.AddForce(transform.forward * accelerationSpeed * Time.fixedDeltaTime, ForceMode.Force);
            
        if (rb.velocity.magnitude >= maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
    }

    private void HorizontalMovement()
    {
        rb.position = transform.position + transform.right * horizontalValue * horizontalSpeed * Time.fixedDeltaTime;
    }

    
}
