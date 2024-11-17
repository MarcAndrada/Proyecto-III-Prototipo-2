using UnityEngine;

public class TurnController : MonoBehaviour
{

    private Vector2Int currentPosition;
    private Vector2Int currentDirection;

    [SerializeField]
    private RectTransform[] connectionIconImages;

    private SlotMachineController slotMachine;
    private MoneyController moneyController;

    private void Awake()
    {
        slotMachine = GetComponent<SlotMachineController>();
        moneyController = GetComponent<MoneyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //De arriba a la izquierda hacia abajo a la derecha
        {
            DirectionButtonPressed(new Vector2Int(0,0), new Vector2Int(1, 1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) //Primera fila
        {
            DirectionButtonPressed(new Vector2Int(0, 0), new Vector2Int(1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) //Segunda fila
        {
            DirectionButtonPressed(new Vector2Int(0, 1), new Vector2Int(1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) //Tercera fila
        {
            DirectionButtonPressed(new Vector2Int(0, 2), new Vector2Int(1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) // De abajo a la izquierda hacia arriba a la derecha
        {
            DirectionButtonPressed(new Vector2Int(0, 2), new Vector2Int(1, -1));
        }


        if (Input.GetKeyDown(KeyCode.Q)) //De arriba a la izquierda hacia abajo a la derecha
        {
            DisplayTurnDirection(new Vector2Int(0, 0), new Vector2Int(1, 1));
        }
        else if (Input.GetKeyDown(KeyCode.W)) //Primera fila
        {
            DisplayTurnDirection(new Vector2Int(0, 0), new Vector2Int(1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.E)) //Segunda fila
        {
            DisplayTurnDirection(new Vector2Int(0, 1), new Vector2Int(1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.R)) //Tercera fila
        {
            DisplayTurnDirection(new Vector2Int(0, 2), new Vector2Int(1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.T)) // De abajo a la izquierda hacia arriba a la derecha
        {
            DisplayTurnDirection(new Vector2Int(0, 2), new Vector2Int(1, -1));
        }

    }
    
    public void DisplayTurnDirection(Vector2Int _starterPos, Vector2Int _direction)
    {
        DisableAllConnectionImages();
        currentPosition = _starterPos;
        currentDirection = _direction;

        while (IsInsideBounds(currentPosition))
        {
            SlotIcon currentIcon = slotMachine.GetSlotIcon(currentPosition.x, currentPosition.y);

            if (currentIcon.type == SlotIcon.IconType.ROTATE)
                currentDirection = currentIcon.GetRotationDirection();
            
            currentPosition += currentDirection;

            if (!IsInsideBounds(currentPosition))
                break;

            SlotIcon nextIcon = slotMachine.GetSlotIcon(currentPosition.x, currentPosition.y);
            
            RectTransform connectionIcon = GetUnusedConnectionImage();

            Vector3 nextIconDirection = nextIcon.transform.position - currentIcon.transform.position;
            //Rotamos el icono
            connectionIcon.right = nextIconDirection.normalized;

            //Seteamos la posicion a la mitad de ambos
            connectionIcon.position = currentIcon.transform.position + (nextIconDirection / 2);

            //Setteamos el largo de la conexion a traves de la distancia
            connectionIcon.sizeDelta = new Vector2(
                Mathf.Abs(currentIcon.GetComponent<RectTransform>().position.x) + Mathf.Abs(nextIcon.GetComponent<RectTransform>().position.x),
                connectionIcon.sizeDelta.y);
        }
    }

    public void DirectionButtonPressed(Vector2Int _starterPos, Vector2Int _direction)
    {
        DisableAllConnectionImages();
        currentPosition = _starterPos;
        currentDirection = _direction;

        int coins = 0, forwardMovements = 0, backwardMovements = 0;
        int coinMultiplier = 1, forwardMultiplier = 1, backwardMultiplier = 1;

        SlotIcon.IconType lastType = SlotIcon.IconType.NONE;

        while (IsInsideBounds(currentPosition)) 
        {
            SlotIcon currentIcon = slotMachine.GetSlotIcon(currentPosition.x, currentPosition.y);
            Debug.Log(currentIcon.type);

            bool haveToMultiply = lastType == currentIcon.type;

            switch (currentIcon.type)
            {
                case SlotIcon.IconType.NONE:
                    break;
                case SlotIcon.IconType.COIN:
                    //Añadir moneda
                    coins++;
                    if (haveToMultiply)
                        coinMultiplier++;
                    break;
                case SlotIcon.IconType.MOVE_FORWARD:
                    //Mover el enemigo hacia adelante
                    forwardMovements++;
                    if(haveToMultiply)
                        forwardMultiplier++;
                    break;
                case SlotIcon.IconType.MOVE_BACKWARDS:
                    //Mover el tuyo hacia delante
                    backwardMovements++;
                    if(haveToMultiply)
                        backwardMultiplier++;
                    break;
                case SlotIcon.IconType.ROTATE:
                    currentDirection = currentIcon.GetRotationDirection();
                    break;
                default:
                    break;
            }

            lastType = currentIcon.type;

            currentPosition += currentDirection;
        }

        //Añadir las monedas
        moneyController.AddCoins(coins * coinMultiplier);
        //Añadir los movimientos frontales
        //Añadir los movimientos traseros

    }


    private bool IsInsideBounds(Vector2 _currentPos)
    {
        return _currentPos.x < GameManager.Instance.slotWidth && _currentPos.x >= 0
            && _currentPos.y < GameManager.Instance.slotHeight && _currentPos.y >= 0;
    }
    private RectTransform GetUnusedConnectionImage()
    {
        foreach (RectTransform item in connectionIconImages)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                item.gameObject.SetActive(true);
                return item;
            }
        }


        return null;


    }
    public void DisableAllConnectionImages()
    {
        foreach (RectTransform item in connectionIconImages)
        {
            item.gameObject.SetActive(false);
        }
    }


}
