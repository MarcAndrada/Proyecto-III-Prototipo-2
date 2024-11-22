using UnityEngine;

public class BalanceObject : InteractableObject
{
    public override void ActivateObject()
    {
        //Quitar todo el dinero
        if (GameManager.Instance.state == GameManager.GameState.PLAYER_TURN)
        {
            //Quitar el dinero la player
        }
        else
        {
            //Quitar el dinero al enemigo
        }
            


        int randNum = Random.Range(0, 2);

        if (randNum == 0)
        {
            //Dar 3 objetos   
            Debug.Log("JACKPOT");
        }
        else
        {
            Debug.Log("CAGASTE");
        }

        Destroy(gameObject);
    }
}
