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

    public GameObject[] GetStoredItems()
    {
        return storedItems;
    }
}