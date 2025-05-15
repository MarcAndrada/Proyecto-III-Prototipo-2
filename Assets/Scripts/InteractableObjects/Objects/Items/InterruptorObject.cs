using UnityEngine;

public class InterruptorObject : InteractableObject
{
    private AudioSource source;
    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();
    }
    public override void UseObject()
    {
        GameManager.Instance.ItemUsed(Store.ItemType.INTERRUPTOR);
        source.Play();
    }
}
