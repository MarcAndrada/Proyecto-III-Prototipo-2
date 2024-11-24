using UnityEngine;

public class JokerObject : InteractableObject
{
    public override void ActivateObject()
    {
        if (GameManager.Instance.state == GameManager.GameState.PLAYER_TURN)
            GameManager.Instance.playerItemsUsed.Add(Store.ItemType.JOKER);
        else if (GameManager.Instance.state == GameManager.GameState.AI_TURN)
            GameManager.Instance.enemyItemsUsed.Add(Store.ItemType.JOKER);

        GameManager.Instance.ItemUsed(Store.ItemType.JOKER);

        Destroy(gameObject);
    }
}