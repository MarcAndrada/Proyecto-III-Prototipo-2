using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private SlotMachineController slotMachineController;
    private TurnController turnController;
    private MoneyController moneyController;
    private InventoryManager inventoryManager;

    [SerializeField]
    private float timeToStartTurn;
    [SerializeField]
    private float timeToSpinWheel;
    [SerializeField]
    private float stopWheelTime;
    [SerializeField]
    private float actionDelay;
    // Start is called before the first frame update
    private void Awake()
    {
        slotMachineController = GetComponent<SlotMachineController>();
        turnController = GetComponent<TurnController>();
        moneyController = GetComponent<MoneyController>();
        inventoryManager = GetComponentInChildren<InventoryManager>();
    }

    void Start()
    {
        //Añadir un item random al inventario
        GameManager.Instance.AddRandomItem(false);
    }

    private void Behaviour()
    {
        if (GameManager.Instance.state != GameManager.GameState.AI_TURN)
            return;

        switch (GameManager.Instance.actionState)
        {
            case GameManager.ActionState.START:
                Invoke("SpinWheel", timeToSpinWheel);
                break;
            case GameManager.ActionState.WHEEL_SPIN:
                break;
            case GameManager.ActionState.ACTION:
                ActionsBehaviour();
                break;
            case GameManager.ActionState.SHOWING_ACTION:

                break;
            case GameManager.ActionState.RESULTS:

                break;
            default:
                break;
        }
    }

    private void SpinWheel()
    {
        slotMachineController.SpinWheel();

        Invoke("Behaviour", stopWheelTime);
    }

    private void ActionsBehaviour()
    {
        //Si hay objetos hacer 50/50 por cada objeto 
        GameObject[] items = inventoryManager.GetStoredItems();
        for (int i = 0; i < items.Length; i++)
        {
            int randNum = Random.Range(0, 2);
            if(items[i] != null && randNum == 0)
            {
                //Usar Item
                items[i].GetComponent<InteractableObject>().ActivateObject();
                Invoke("Behaviour", actionDelay);
                return;
            }
        }
                

        //Seleccionar un boton random
        int randButton = Random.Range(0, 5);

        switch (randButton)
        {
            case 0:
                turnController.DirectionButtonPressed(new Vector2Int(0, 0), new Vector2Int(1, 1));
                break;
            case 1:
                turnController.DirectionButtonPressed(new Vector2Int(0, 0), new Vector2Int(1, 0));
                break;
            case 2:
                turnController.DirectionButtonPressed(new Vector2Int(0, 1), new Vector2Int(1, 0));
                break;
            case 3:
                turnController.DirectionButtonPressed(new Vector2Int(0, 2), new Vector2Int(1, 0));
                break;
            case 4:
                turnController.DirectionButtonPressed(new Vector2Int(0, 2), new Vector2Int(1, -1));
                break;
            default:
                break;
        }


    }

    public void StartTurn()
    {

        Invoke("Behaviour", timeToStartTurn);
    }
}
