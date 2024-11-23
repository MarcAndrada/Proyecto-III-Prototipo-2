using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    [Serializable]
    struct IconUI
    {
        public Image iconImage;
        public TextMeshProUGUI iconObtainedText;
        public TextMeshProUGUI iconMultiplierText;
    }

    [SerializeField]
    private bool isPlayer;

    [SerializeField]
    private IconUI[] iconsUI;


    private Vector2Int currentPosition;
    private Vector2Int currentDirection;

    private List<SlotIcon> selectedIcons;
    private int currentLoopId;
    private SlotIcon.IconType lastIconType;
    private int roundCoins;
    private int roundCoinM;
    private int roundUpMovements;
    private int roundUpMovementM;
    private int roundDownMovements;
    private int roundDownMovementM;


    [SerializeField]
    private RectTransform[] connectionIconImages;

    private SlotMachineController slotMachine;
    private MoneyController moneyController;

    


    private void Awake()
    {
        slotMachine = GetComponent<SlotMachineController>();
        moneyController = GetComponent<MoneyController>();
        selectedIcons = new List<SlotIcon>();
    }
    private void Start()
    {
        foreach (IconUI item in iconsUI)
        {
            item.iconImage.gameObject.SetActive(false);
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
        currentPosition = _starterPos;
        currentDirection = _direction;
        
        selectedIcons.Clear();
        currentLoopId = 0;        
        lastIconType = SlotIcon.IconType.NONE;

        roundCoins = 0;
        roundDownMovements = 0;
        roundUpMovements = 0;
        
        roundCoinM = 1;
        roundDownMovementM = 1;
        roundUpMovementM = 1;

        while (IsInsideBounds(currentPosition)) 
        {
            SlotIcon currentIcon = slotMachine.GetSlotIcon(currentPosition.x, currentPosition.y);


            selectedIcons.Add(currentIcon);

            if (currentIcon.type == SlotIcon.IconType.ROTATE)
            {
                currentDirection = currentIcon.GetRotationDirection();
            }

            currentPosition += currentDirection;

        }

        Invoke("HoverButtonsLoop", GameManager.Instance.actionResultIconDuration);
        GameManager.Instance.FinishActionState(); //Aqui acaba la accion de ACTION y empieza la de SHOWING_ACTION

    }

    private void HoverButtonsLoop()
    {
        List <Store.ItemType> usedItemsType = 
            GameManager.Instance.state == GameManager.GameState.PLAYER_TURN
            ? GameManager.Instance.playerItemsUsed
            : GameManager.Instance.enemyItemsUsed;

        bool haveToMultiply = lastIconType == selectedIcons[currentLoopId].type || usedItemsType.Contains(Store.ItemType.JOKER);

        switch (selectedIcons[currentLoopId].type)
        {
            case SlotIcon.IconType.NONE:
                break;
            case SlotIcon.IconType.COIN:
                //Añadir moneda
                roundCoins++;
                if (haveToMultiply)
                    roundCoinM++;
                break;
            case SlotIcon.IconType.MOVE_FORWARD:
                //Mover el enemigo hacia adelante
                roundDownMovements++;
                if (haveToMultiply)
                    roundDownMovementM++;
                break;
            case SlotIcon.IconType.MOVE_BACKWARDS:
                //Mover el tuyo hacia delante
                roundUpMovements++;
                if (haveToMultiply)
                    roundUpMovementM++;
                break;
            default:
                break;
        }


        if (selectedIcons[currentLoopId].type != SlotIcon.IconType.ROTATE)
            lastIconType = selectedIcons[currentLoopId].type;

        selectedIcons[currentLoopId].backgroundImage.enabled = true;

        UpdatePathUI(selectedIcons[currentLoopId].type);

        currentLoopId++;


        if (selectedIcons.Count > currentLoopId)
            Invoke("HoverButtonsLoop", GameManager.Instance.actionResultIconDuration);
        else
            Invoke("EndHovering", GameManager.Instance.actionResultIconDuration);

    }

    private void UpdatePathUI(SlotIcon.IconType _icon)
    {
        if (_icon == SlotIcon.IconType.ROTATE)
            return;

        IconUI currentIconUI = iconsUI[(int)_icon - 1];

        currentIconUI.iconImage.gameObject.SetActive(true);

        switch (_icon)
        {
            case SlotIcon.IconType.COIN:
                currentIconUI.iconObtainedText.text = roundCoins.ToString();
                currentIconUI.iconMultiplierText.text = roundCoinM.ToString();
                
                break;
            case SlotIcon.IconType.MOVE_FORWARD:
                currentIconUI.iconObtainedText.text = roundDownMovements.ToString();
                currentIconUI.iconMultiplierText.text = roundDownMovementM.ToString();
                break;
            case SlotIcon.IconType.MOVE_BACKWARDS:
                currentIconUI.iconObtainedText.text = roundUpMovements.ToString();
                currentIconUI.iconMultiplierText.text = roundUpMovementM.ToString();
                break;
            default:
                break;
        }
    }

    private void EndHovering()
    {
        //Rotar camara        
        GameManager.Instance.FinishActionState(); //Aqui acaba la accion de SHOWING_ACTION y empieza la de RESULT

        Invoke("GetRewards", GameManager.Instance.actionResultIconDuration);
    }

    private void GetRewards()
    {
        //Añadir las monedas
        moneyController.AddCoins(roundCoins * roundCoinM);
        //Añadir los movimientos frontales
        GameManager.Instance.ChangeHealth(isPlayer, roundUpMovements * roundUpMovementM);
        //Añadir los movimientos traseros
        GameManager.Instance.ChangeHealth(!isPlayer, -roundDownMovements * roundDownMovementM);
        
        DisableAllConnectionImages();

        Invoke("FinishTurn", 2.5f);
    }

    private void FinishTurn()
    {
        foreach (IconUI item in iconsUI)
        {
            item.iconImage.gameObject.SetActive(false);
        }

        GameManager.Instance.FinishActionState(); //Aqui acaba la accion de RESULT y empieza la de START

        List<Store.ItemType> usedItemsType =
            GameManager.Instance.state == GameManager.GameState.PLAYER_TURN
            ? GameManager.Instance.playerItemsUsed
            : GameManager.Instance.enemyItemsUsed;

        if(usedItemsType.Contains(Store.ItemType.JOKER))
            usedItemsType.Remove(Store.ItemType.JOKER);
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
