using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform[] inventorySlots;
    [SerializeField] private GameObject[] storedItems;

    void Awake()
    {
        storedItems = new GameObject[inventorySlots.Length];
    }

    public bool AddItem(GameObject item)
    {
        for (int i = 0; i < storedItems.Length; i++)
        {
            if (storedItems[i] == null)
            {
                storedItems[i] = Instantiate(item, inventorySlots[i]);
                storedItems[i].transform.position = inventorySlots[i].position;
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