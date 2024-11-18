using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Info", menuName = "ScriptableObjects/InteractableObject")]
public class InteractableObjectsInfo : ScriptableObject
{
    [field: SerializeField]
    public string objectName {  get; private set; }
    [field: SerializeField, TextArea]
    public string objectDescription {  get; private set; }
}
