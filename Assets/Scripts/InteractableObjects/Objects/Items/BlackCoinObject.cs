public class BlackCoinObject : InteractableObject
{
    public override void ActivateObject()
    {
        GameManager.Instance.ItemUsed(Store.ItemType.RED_COIN);

        Destroy(gameObject);
    }
}
