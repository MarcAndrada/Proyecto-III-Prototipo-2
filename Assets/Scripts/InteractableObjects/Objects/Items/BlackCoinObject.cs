using UnityEngine;

public class BlackCoinObject : InteractableObject
{
    private AudioSource source;

    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();
    }
    public override void UseObject()
    {
        GameManager.Instance.ItemUsed(Store.ItemType.RED_COIN);
        source.Play();
    }
}
