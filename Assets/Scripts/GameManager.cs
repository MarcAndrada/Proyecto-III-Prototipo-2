using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public enum GameState { COIN_FLIP, PLAYER_TURN, AI_TURN }
    public enum ActionState { START, WHEEL_SPIN, ACTION, SHOWING_ACTION, RESULTS }

    [HideInInspector]
    public List<Store.ItemType> playerItemsUsed;
    [HideInInspector]
    public List<Store.ItemType> enemyItemsUsed;

    [Space, Header("Item Prefabs"), SerializedDictionary("Item", "Prefab")]
    public SerializedDictionary<Store.ItemType, InteractableObjectsInfo> itemsPrefabs;

    [field: SerializeField]
    public PlayerLookActionsController playerLookController {  get; private set; }

    [field: Space, Header("GameState"), SerializeField]
    public GameState state { get; private set; }
    [field: SerializeField]
    public ActionState actionState { get; private set; }
    public int stateOrder { get; private set; }


    [field: Space, Header("Slot"), SerializeField]
    public SlotMachineController playerSlot {  get; private set; }
    [field: SerializeField]
    public SlotMachineController enemySlot { get; private set; }
    [field: SerializeField]
    public int slotWidth { get; private set; }
    [field: SerializeField]
    public int slotHeight { get; private set; }
    [field: SerializeField]
    public int actionResultIconDuration { get; private set; }
    private bool playerTurnSkipped;
    private bool enemyTurnSkipped;

    [Space, Header("Slot Icons"), SerializedDictionary("Type", "Sprite")]
    public SerializedDictionary<SlotIcon.IconType, Sprite> iconSprite;
    [field: SerializeField]
    public float iconXOffset { get; private set; }
    [field: SerializeField]
    public float iconYOffset { get; private set; }

    [field: Space, Header("Hanged mans"), SerializeField]
    public GameObject playerDummy {  get; private set; }
    [field: SerializeField]
    public GameObject enemyDummy {  get; private set; }
    [SerializeField]
    private int maxHealth;
    [field: SerializeField]
    public int playerHangedManHealth { get; private set; }
    [field: SerializeField]
    public int enemyHangedManHealth { get; private set; }
    [field: SerializeField]
    public float ropeOffset;
    [field: SerializeField]
    public float moveUpSpeed { get; private set; }
    [field: SerializeField]
    public float moveDownSpeed { get; private set; }

    

    [field: Space, Header("Cigarrette"), SerializeField]
    public Transform cigarretteTransform {  get; private set; }
    [field: SerializeField]
    public ParticleSystem segarroSmoke { get; private set; }

    [field: Space, Header("Coin"), SerializeField]
    private CoinFlip coinController;
    [SerializeField]
    private ScrollingText scrollingText;
    private bool gameEnded = false;

    [Space, Header("Buttons Materials"), SerializeField]
    private Material buttonMaterialActive;
    [SerializeField]
    private Material buttonMaterialDisabled;
    [SerializeField] 
    private Renderer[] buttons;

    [Space, Header("End Game Canvas"), SerializeField]
    private GameObject winCanvas;
    [SerializeField]
    private GameObject loseCanvas;

    [Space, Header("Audio"), SerializeField]
    private string gameState;
    [SerializeField]
    private string playerTurnState;
    [SerializeField]
    private string enemyTurnState;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);

        Instance = this;
    }

    private void Start()
    {
        playerItemsUsed = new List<Store.ItemType>();
        enemyItemsUsed = new List<Store.ItemType>();

        state = GameState.COIN_FLIP;
        actionState = ActionState.START;
        AddRandomItem(true);
        AddRandomItem(false);
        
        foreach (Renderer item in buttons)
            item.material = buttonMaterialDisabled;
        
        FlipCoin();
    }

    private void ChangeGameState(GameState _nextState)
    {
        if (gameEnded) return;

        switch (state)
        {
            case GameState.COIN_FLIP:
                break;
            case GameState.PLAYER_TURN:
                playerSlot.GetComponent<ItemsFeedbackController>().ClearItemScreen();
                break;
            case GameState.AI_TURN:
                enemySlot.GetComponent<ItemsFeedbackController>().ClearItemScreen();
                break;
            default:
                break;
        }

        state = _nextState;

        switch (_nextState)
        {
            case GameState.COIN_FLIP:
                //Dar nuevos items
                AddRandomItem(true);
                AddRandomItem(false);

                for (int i = 0; i < 10; i++)
                {
                    playerItemsUsed.Remove(Store.ItemType.BALANCE);
                    playerItemsUsed.Remove(Store.ItemType.JOKER);
                    playerItemsUsed.Remove(Store.ItemType.CIGARRETTE);

                    enemyItemsUsed.Remove(Store.ItemType.BALANCE);
                    enemyItemsUsed.Remove(Store.ItemType.JOKER);
                    enemyItemsUsed.Remove(Store.ItemType.CIGARRETTE);
                }

                if (UsedItem(enemyItemsUsed, Store.ItemType.INTERRUPTOR) || playerTurnSkipped)
                {
                    if (!playerTurnSkipped)
                    {
                        stateOrder = 0;
                        ChangeToNextGameState();
                        return;
                    }

                    enemySlot.GetComponent<ItemsFeedbackController>().TurnOnRivalScreen();
                    playerTurnSkipped = false;
                    enemyItemsUsed.Remove(Store.ItemType.INTERRUPTOR);
                    enemyItemsUsed.Remove(Store.ItemType.RED_COIN);
                    playerItemsUsed.Remove(Store.ItemType.RED_COIN);
                    CheckSkippedTurn();
                }

                if (UsedItem(playerItemsUsed, Store.ItemType.INTERRUPTOR) || enemyTurnSkipped)
                {
                    if (!enemyTurnSkipped)
                    {
                        stateOrder = 1;
                        ChangeToNextGameState();
                        return;
                    }

                    playerSlot.GetComponent<ItemsFeedbackController>().TurnOnRivalScreen();
                    enemyTurnSkipped = false;
                    playerItemsUsed.Remove(Store.ItemType.INTERRUPTOR);
                    playerItemsUsed.Remove(Store.ItemType.RED_COIN);
                    enemyItemsUsed.Remove(Store.ItemType.RED_COIN);
                    CheckSkippedTurn();
                }

                FlipCoin();

                break;
            case GameState.PLAYER_TURN:
                if (playerTurnSkipped)
                    enemySlot.GetComponent<ItemsFeedbackController>().TurnOnRivalScreen();

                if (UsedItem(enemyItemsUsed, Store.ItemType.INTERRUPTOR))
                {
                    Debug.Log("Skip player turn");
                    playerTurnSkipped = true;
                    enemyItemsUsed.Remove(Store.ItemType.INTERRUPTOR);
                    enemyItemsUsed.Remove(Store.ItemType.RED_COIN);
                    playerItemsUsed.Remove(Store.ItemType.RED_COIN);


                    ChangeToNextGameState();
                    scrollingText.SetText("Skipped Your Turn");
                    return;
                }

                AkUnitySoundEngine.SetState(gameState, playerTurnState);
                scrollingText.SetText("Your Turn");
                break;
            case GameState.AI_TURN:
                if (enemyTurnSkipped)
                    playerSlot.GetComponent<ItemsFeedbackController>().TurnOnRivalScreen();

                //Resetear los Items del enemigo
                if (UsedItem(playerItemsUsed, Store.ItemType.INTERRUPTOR))
                {
                    Debug.Log("Skip enemy turn");
                    enemyTurnSkipped = true;
                    playerItemsUsed.Remove(Store.ItemType.INTERRUPTOR);
                    playerItemsUsed.Remove(Store.ItemType.RED_COIN);
                    enemyItemsUsed.Remove(Store.ItemType.RED_COIN);

                    ChangeToNextGameState();
                    scrollingText.SetText("Skipped Enemy Turn");
                    return;
                }

                AkUnitySoundEngine.SetState(gameState, enemyTurnState);
                scrollingText.SetText("Enemy Turn");
                enemySlot.GetComponent<EnemyBehaviour>().StartTurn();
                break;
            default:
                break;
        }
    }

    public void FinishActionState()
    {

        if (gameEnded) return;
        
        switch (actionState)
        {
            case ActionState.START:
                actionState = ActionState.WHEEL_SPIN;
                if(state == GameState.PLAYER_TURN)
                    playerLookController.AddAction(PlayerLookActionsController.LookAtActions.LOCK_MAIN, 1);
                else
                    playerLookController.AddAction(PlayerLookActionsController.LookAtActions.ENEMY_TURN, 1);
                break;
            case ActionState.WHEEL_SPIN:
                if (state == GameState.PLAYER_TURN)
                {
                    foreach (Renderer item in buttons)
                        item.material = buttonMaterialActive;
                    
                    playerLookController.AddAction(PlayerLookActionsController.LookAtActions.NORMAL_CAMERA, 1);
                }
                else
                    playerLookController.AddAction(PlayerLookActionsController.LookAtActions.ENEMY_TURN, 1);
                actionState = ActionState.ACTION;
                break;
            case ActionState.ACTION:
                actionState = ActionState.SHOWING_ACTION;
                if (state == GameState.PLAYER_TURN)
                {
                    foreach (Renderer item in buttons)
                        item.material = buttonMaterialDisabled;
                    
                    playerLookController.AddAction(PlayerLookActionsController.LookAtActions.LOCK_MAIN, 1);
                }
                else
                    playerLookController.AddAction(PlayerLookActionsController.LookAtActions.ENEMY_TURN, 1);
                break;
            case ActionState.SHOWING_ACTION:
                actionState = ActionState.RESULTS;
                playerLookController.AddAction(PlayerLookActionsController.LookAtActions.RESULTS, 1);
                break;
            case ActionState.RESULTS:
                //Cambiar la camara del player a la central y setearle los triggers que tocan
                playerLookController.AddAction(PlayerLookActionsController.LookAtActions.NORMAL_CAMERA, 1);
                //Cambiar de turno y empezar de nuevo las acciones
                WinLoseCondition();
                ChangeToNextGameState();
                actionState = ActionState.START;
                break;
            default:
                break;
        }
    }

    public void ChangeToNextGameState()
    {
        if (stateOrder == 0) //El player tira primero
        {
            switch (state)
            {
                case GameState.COIN_FLIP:
                    ChangeGameState(GameState.PLAYER_TURN);
                    break;
                case GameState.PLAYER_TURN:
                    ChangeGameState(GameState.AI_TURN);
                    break;
                case GameState.AI_TURN:
                    //Flipear Moneda
                    ChangeGameState(GameState.COIN_FLIP);
                    break;
                default:
                    break;
            }
        }
        else //El Enemigo Tira primero
        {
            switch (state)
            {
                case GameState.COIN_FLIP:
                    ChangeGameState(GameState.AI_TURN);
                    break;
                case GameState.PLAYER_TURN:
                    //Flipear Moneda
                    ChangeGameState(GameState.COIN_FLIP);
                    break;
                case GameState.AI_TURN:
                    ChangeGameState(GameState.PLAYER_TURN);
                    break;
                default:
                    break;
            }
        }
    }

    private void FlipCoin()
    {
        playerLookController.AddAction(PlayerLookActionsController.LookAtActions.ENEMY_TURN, 1);

        if (UsedItem(playerItemsUsed, Store.ItemType.RED_COIN))
        {
            //Animacion Moneda Roja

            stateOrder = 0;
            playerItemsUsed.Remove(Store.ItemType.RED_COIN);
            coinController.PlayAnim("RedCoin");
            return;
        }
        else if (UsedItem(enemyItemsUsed, Store.ItemType.RED_COIN))
        {
            //Anim moneda Roja
            stateOrder = 1;
            enemyItemsUsed.Remove(Store.ItemType.RED_COIN);
            coinController.PlayAnim("RedCoin");
            return;
        }


        stateOrder = Random.Range(0, 2);

        //Animacion moneda normal
        //(Esta animacion llama a una evento que llama al ChangeToNextGameState();)
        string stateString = stateOrder == 0 ? "PlayerCoin" : "EnemyCoin";
        coinController.PlayAnim(stateString);


    }
    private void CheckSkippedTurn()
    {
        if (enemyTurnSkipped)
        {
            playerItemsUsed.Remove(Store.ItemType.RED_COIN);
            enemyItemsUsed.Remove(Store.ItemType.RED_COIN);
            ChangeToNextGameState();
        }
        else if (playerTurnSkipped)
        {
            playerItemsUsed.Remove(Store.ItemType.RED_COIN);
            enemyItemsUsed.Remove(Store.ItemType.RED_COIN);
            ChangeToNextGameState();
        }
    }
    public void ChangeHealth(bool _isPlayer, int _healthChange)
    {
        if (_isPlayer)
            playerHangedManHealth = Mathf.Clamp(playerHangedManHealth + _healthChange, 0, maxHealth);
        else
            enemyHangedManHealth = Mathf.Clamp(enemyHangedManHealth + _healthChange, 0, maxHealth);
    }

    private void WinLoseCondition()
    {
        if (playerHangedManHealth <= 0)
        {
            gameEnded = true;
            playerLookController.AddAction(PlayerLookActionsController.LookAtActions.NORMAL_CAMERA, 1);
            scrollingText.SetTextLoop("YOU LOSE");
            //Mostrar menu de derrota
            loseCanvas.SetActive(true);
        }
        if (enemyHangedManHealth <= 0)
        {
            gameEnded = true;
            playerLookController.AddAction(PlayerLookActionsController.LookAtActions.NORMAL_CAMERA, 1);
            scrollingText.SetTextLoop("YOU WIN");
            //Mostrar menu de victoria
            winCanvas.SetActive(true);
        }
    }
    private bool UsedItem(List<Store.ItemType> _itemList, Store.ItemType _itemToSearch)
    {
        foreach (Store.ItemType item in _itemList)
        {
            if(item == _itemToSearch)
                return true;
        }

        return false;
    }
    public void ItemUsed(Store.ItemType _itemUsed)
    {
        if (state == GameState.PLAYER_TURN)
        {
            if (playerItemsUsed.Contains(_itemUsed))
                return;

            playerItemsUsed.Add(_itemUsed);
            playerSlot.GetComponent<ItemsFeedbackController>().ItemUsed(_itemUsed);
        }
        else if (state == GameState.AI_TURN)
        {
            if (enemyItemsUsed.Contains(_itemUsed))
                return;

            enemyItemsUsed.Add(_itemUsed);
            enemySlot.GetComponent<ItemsFeedbackController>().ItemUsed(_itemUsed);
        }
    }

    public void AddRandomItem(bool _toPlayer)
    {
        Store.ItemType randomItem = (Store.ItemType)Random.Range(0, (int)Store.ItemType.BALANCE + 1);
        if (_toPlayer)
        {
            if (!itemsPrefabs[randomItem].unlocked)
            {
                AddRandomItem(_toPlayer);
                return;
            }
            playerSlot.GetComponentInChildren<InventoryManager>().AddItem(randomItem);
        }
        else
        {
            enemySlot.GetComponentInChildren<InventoryManager>().AddItem(randomItem);
        }
    }
}
