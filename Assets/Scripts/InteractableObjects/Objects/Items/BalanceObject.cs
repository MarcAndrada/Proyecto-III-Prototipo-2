using System.Collections.Generic;
using UnityEngine;

public class BalanceObject : InteractableObject
{
    int randNum;

    private List<Store.ItemType> itemsGenerated = new List<Store.ItemType>();

    [SerializeField]
    private AK.Wwise.Event balanceMoveEvent;
    [SerializeField]
    private AK.Wwise.Event balanceWinEvent;
    [SerializeField]
    private AK.Wwise.Event balanceLoseEvent;

    public override void ActivateObject()
    {
        transform.parent = null;

        animator.enabled = true;
        //Play a la animacion
        randNum = Random.Range(0, 2);
        string animString;
        if (randNum == 0)
            animString = "Win";
        else
            animString = "Lose";

        animator.SetTrigger(animString);
        
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public override void UseObject()
    {
        InventoryManager inventoryManager = null;
        MoneyController moneyCont = null;
        //Quitar todo el dinero
        if (GameManager.Instance.state == GameManager.GameState.PLAYER_TURN)
        {
            //Quitar el dinero la player
            moneyCont = GameManager.Instance.playerSlot.GetComponent<MoneyController>();
            inventoryManager = GameManager.Instance.playerSlot.GetComponentInChildren<InventoryManager>();
        }
        else
        {
            //Quitar el dinero al enemigo
            moneyCont = GameManager.Instance.enemySlot.GetComponent<MoneyController>();
            inventoryManager = GameManager.Instance.enemySlot.GetComponentInChildren<InventoryManager>();
        }

        moneyCont.RemoveCoins(moneyCont.GetCoinAmount());
        GameManager.Instance.ItemUsed(Store.ItemType.BALANCE);
        
        if (randNum == 0)
        {
            AkUnitySoundEngine.PostEvent(balanceWinEvent.Id, gameObject);

            //Dar 3 objetos   
            for (int i = 0; i < 3; i++)
            {
                Store.ItemType item = (Store.ItemType)Random.Range(0, (int)Store.ItemType.BALANCE);
                if (itemsGenerated.Contains(item))
                {
                    i--;
                    continue;
                }

                inventoryManager.AddItem(item);
                itemsGenerated.Add(item);
            }
        }
        else
        {
            AkUnitySoundEngine.PostEvent(balanceLoseEvent.Id, gameObject);
        }
    }

    public void PlayBalanceSound()
    {
        AkUnitySoundEngine.PostEvent(balanceMoveEvent.Id, gameObject);
    }
}
