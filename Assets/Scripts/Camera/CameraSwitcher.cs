using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainVCam;
    [SerializeField] private CinemachineVirtualCamera topVCam;
    [SerializeField] private CinemachineVirtualCamera leftVCam;
    [SerializeField] private CinemachineVirtualCamera rightVCam;

    private CinemachineVirtualCamera currentVCam;
    
    [Header("Canvas Borders")]
    [SerializeField] private RectTransform bottom;
    [SerializeField] private RectTransform top;
    [SerializeField] private RectTransform left;
    [SerializeField] private RectTransform right;
    
    private List<EventTrigger.Entry> entriesEnter;

    private void Start()
    {
        entriesEnter = new List<EventTrigger.Entry>();
        currentVCam = mainVCam;
        SetActiveCamera(currentVCam);
        
        AddEventTrigger(top, "Top");
        AddEventTrigger(left, "Left");
        AddEventTrigger(right, "Right"); 
    }

    public void SwitchToCamera(string direction)
    {
        Debug.Log("Pointer Enter");

        CinemachineVirtualCamera newVCam = null;

        switch (direction)
        {
            case "Top":
                newVCam = topVCam;
                break;
            case "Left":
                newVCam = leftVCam;
                break;
            case "Right":
                newVCam = rightVCam;
                break;
            case "Main":
                newVCam = mainVCam;
                break;
        }

        if (newVCam != null && newVCam != currentVCam)
        {
            SetActiveCamera(newVCam);
            DeactivateTriggers();
            if (direction == "Top")
            {
                AddEventTrigger(bottom, "Main");
            }
            if (direction == "Left")
            {
                AddEventTrigger(right, "Main");
            }
            if (direction == "Right")
            {
                AddEventTrigger(left, "Main");
            }
            if (direction == "Main")
            {
                AddEventTrigger(top, "Top");
                AddEventTrigger(left, "Left");
                AddEventTrigger(right, "Right"); 
            }
        }
    }
    private void SetActiveCamera(CinemachineVirtualCamera newVCam)
    {
        if (currentVCam != null)
            currentVCam.Priority = 10;

        currentVCam = newVCam;
        currentVCam.Priority = 20;
    }
    private void AddEventTrigger(RectTransform rect, string direction)
    {
        EventTrigger trigger = rect.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entryEnter.callback.AddListener((eventData) => { SwitchToCamera(direction); });
        entriesEnter.Add(entryEnter); 
        trigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        entryExit.callback.AddListener((eventData) => { OnPointerExitCamera(); });
        trigger.triggers.Add(entryExit);
    }
    private void OnPointerExitCamera()
    {
        Debug.Log("Pointer Exit");
        
    }    
   
    private void DeactivateTriggers()
    {
        foreach (EventTrigger.Entry entry in entriesEnter)
        {
            entry.callback.RemoveAllListeners();
        }
    }
   
}
