using UnityEngine;

[CreateAssetMenu(fileName = "Info", menuName = "ScriptableObjects/InteractableObject")]
public class InteractableObjectsInfo : ScriptableObject
{
    [field: SerializeField]
    public string objectName {  get; private set; }
    [field: SerializeField, TextArea]
    public string objectDescription {  get; private set; }
    [field: SerializeField]
    public GameObject objectPrefab { get; private set; }
    [field: SerializeField]
    public bool unlocked;
    [field: SerializeField]
    public GameObject medalPrefab { get; private set; }
}
