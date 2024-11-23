using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform mainCameraTransform;
    [SerializeField] private Transform topCameraTransform;
    [SerializeField] private Transform leftCameraTransform;
    [SerializeField] private Transform rightCameraTransform;
    [Space, SerializeField]
    private float cameraMoveSpeed;
    private Vector3 cameraDestinyPos;
    private Quaternion cameraDestinyRot;
    private bool canMove;

    [Header("Canvas Borders")]
    [SerializeField] private RectTransform bottom;
    [SerializeField] private RectTransform top;
    [SerializeField] private RectTransform left;
    [SerializeField] private RectTransform right;
    
    private List<EventTrigger.Entry> entriesEnter;

    private void Start()
    {
        entriesEnter = new List<EventTrigger.Entry>();
        
        mainCamera.transform.position = mainCameraTransform.transform.position;
        mainCamera.transform.rotation = mainCameraTransform.transform.rotation;
        cameraDestinyPos = mainCameraTransform.transform.position;
        cameraDestinyRot = mainCameraTransform.transform.rotation;

        canMove = true;

        AddEventTrigger(top, "Top");
        AddEventTrigger(left, "Left");
        AddEventTrigger(right, "Right");

        Cursor.lockState = CursorLockMode.Confined;        
    }

    private void Update()
    {
        MoveCamera();
    }

    public (Vector3, Quaternion) GetCameraPositionAndRotation(string direction)
    {
        Vector3 currentDestPos = Vector3.zero;
        Quaternion currentDestRot = Quaternion.identity;

        switch (direction)
        {
            case "Top":
                currentDestPos = topCameraTransform.position;
                currentDestRot = topCameraTransform.rotation;
                break;
            case "Left":
                currentDestPos = leftCameraTransform.position;
                currentDestRot = leftCameraTransform.rotation;
                break;
            case "Right":
                currentDestPos = rightCameraTransform.position;
                currentDestRot = rightCameraTransform.rotation;
                break;
            case "Main":
                currentDestPos = mainCameraTransform.position;
                currentDestRot = mainCameraTransform.rotation;
                break;
        }

        return (currentDestPos, currentDestRot);
    }
    public void SetMovementCameraTriggers(string _direction, (Vector3, Quaternion) _destiny)
    {
        if(!canMove)
            return;

        canMove = false;

        SetCameraDestination(_destiny);

        //SetActiveCamera(newVCam);
        DeactivateTriggers();
        if (_direction == "Top")
            AddEventTrigger(bottom, "Main");
        else if (_direction == "Left")
            AddEventTrigger(right, "Main");
        else if (_direction == "Right")
            AddEventTrigger(left, "Main");
        else if (_direction == "Main")
        {
            AddEventTrigger(top, "Top");
            AddEventTrigger(left, "Left");
            AddEventTrigger(right, "Right");
        }
    }
    public void SetCameraDestination((Vector3, Quaternion) _destiny)
    {
        cameraDestinyPos = _destiny.Item1;
        cameraDestinyRot = _destiny.Item2;
    }
    public void SetCanMove()
    {
        canMove = true;
    }


    private void MoveCamera()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraDestinyPos, Time.deltaTime * cameraMoveSpeed);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, cameraDestinyRot, Time.deltaTime * cameraMoveSpeed);
    }
    private void AddEventTrigger(RectTransform rect, string direction)
    {
        EventTrigger trigger = rect.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entryEnter.callback.AddListener((eventData) => { SetMovementCameraTriggers(direction, GetCameraPositionAndRotation(direction)); });
        entriesEnter.Add(entryEnter); 
        trigger.triggers.Add(entryEnter);
        
        EventTrigger.Entry entryExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        entryExit.callback.AddListener((eventData) => { SetCanMove(); });
        trigger.triggers.Add(entryExit);

    }
    public void DeactivateTriggers()
    {
        foreach (EventTrigger.Entry entry in entriesEnter)
        {
            entry.callback.RemoveAllListeners();
        }
    }
   
}
