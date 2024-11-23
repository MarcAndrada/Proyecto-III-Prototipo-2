using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceObject : InteractableObject
{


    private List<Store.ItemType> itemsGenerated = new List<Store.ItemType>();
    public override void ActivateObject()
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

        if (moneyCont.GetCoinAmount() < 0)
            return;
        moneyCont.RemoveCoins(moneyCont.GetCoinAmount());

        int randNum = Random.Range(0, 2);

        if (randNum == 0)
        {
            //Dar 3 objetos   
            Debug.Log("JACKPOT");

            for (int i = 0; i < 3; i++)
            {
                Store.ItemType item = (Store.ItemType)Random.Range(0, (int)Store.ItemType.BALANCE);
                if (itemsGenerated.Contains(item))
                {
                        i--;
                        continue;
                }

                itemsGenerated.Add(item);

                GameObject itemObject = Instantiate(GameManager.Instance.itemsPrefabs[item], Vector3.zero, Quaternion.identity);
                inventoryManager.AddItem(itemObject);
            }
        }
        else
        {
            Debug.Log("Cagaste");
        }

        Destroy(gameObject);
    }
}
