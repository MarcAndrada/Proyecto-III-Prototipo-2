using System.Collections.Generic;
using UnityEngine;

public class PlayerLookActionsController : MonoBehaviour
{

    [SerializeField]
    private CameraSwitcher cameraController;

    public enum LookAtActions { NORMAL_CAMERA, RESULTS, ENEMY_TURN, LOCK_MAIN }

    private List<(LookAtActions, float)> actionsList = new List<(LookAtActions, float)>();
    public bool canDoNextAction { get; private set; }

    private float timePassed;

    private void Start()
    {
        canDoNextAction = true;
    }
    private void Update()
    {
        CheckIfCanDoNextAction();
        CheckIfCurrentActionDone();
    }

    private void CheckIfCanDoNextAction()
    {
        if (!canDoNextAction || actionsList.Count <= 0)
            return;

        switch (actionsList[0].Item1)
        {
            case LookAtActions.NORMAL_CAMERA:
                ResetCamera();
                break;
            case LookAtActions.RESULTS:
                LookAt("Right");
                break;
            case LookAtActions.ENEMY_TURN:
                LookAt("Top");
                break;
            case LookAtActions.LOCK_MAIN:
                LookAt("Main");
                break;
            default:
                break;
        }

        canDoNextAction = false;
    }

    private void CheckIfCurrentActionDone()
    {
        if (canDoNextAction)
            return;

        timePassed += Time.deltaTime;

        if(timePassed >= actionsList[0].Item2)
        {
            actionsList.RemoveAt(0);
            timePassed = 0f;
            canDoNextAction = true;
        }

    }

    public void AddAction(LookAtActions _action, float _duration)
    {
        actionsList.Add((_action, _duration));
    }

    private void ResetCamera()
    {
        cameraController.DeactivateTriggers();
        cameraController.SetCanMove();
        cameraController.SetMovementCameraTriggers("Main", cameraController.GetCameraPositionAndRotation("Main"));
    }
    private void LookAt(string _direction)
    {
        cameraController.DeactivateTriggers();
        cameraController.SetCameraDestination(cameraController.GetCameraPositionAndRotation(_direction));
    }
}
