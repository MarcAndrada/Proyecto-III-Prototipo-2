using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform[] inventorySlots;
    [SerializeField] private GameObject[] storedItems;

    void Awake()
    {
        storedItems = new GameObject[inventorySlots.Length];
    }

    public bool AddItem(Store.ItemType _type)
    {
        for (int i = 0; i < storedItems.Length; i++)
        {
            if (storedItems[i] == null)
            { 
                GameObject newObject = Instantiate(GameManager.Instance.itemsPrefabs[_type].objectPrefab, inventorySlots[i]);
                storedItems[i] = newObject;
                newObject.transform.localPosition = Vector3.zero;
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(GameObject _objectoToRemove)
    {
        for (int i = 0; i < storedItems.Length; i++)
        {
            if (storedItems[i] == _objectoToRemove)
                storedItems[i] = null;
        }
        
    }

    public GameObject[] GetStoredItems()
    {
        return storedItems;
    }
}