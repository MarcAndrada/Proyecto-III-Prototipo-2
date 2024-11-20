public class InterruptorObject : InteractableObject
{
    public override void ActivateObject()
    {
        GameManager.Instance.ItemUsed(Store.ItemType.INTERRUPTOR);
        //Poner animacion de clickar boton

        Destroy(gameObject);
    }
}
