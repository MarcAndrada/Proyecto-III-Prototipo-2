using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform[] inventorySlots;
    [SerializeField] private GameObject[] storedItems;

    void Start()
    {
        storedItems = new GameObject[inventorySlots.Length];
    }

    public bool AddItem(GameObject item)
    {
        for (int i = 0; i < storedItems.Length; i++)
        {
            if (storedItems[i] == null)
            {
                storedItems[i] = Instantiate(item, inventorySlots[i].position, Quaternion.identity);
                storedItems[i].transform.SetParent(inventorySlots[i]);
                return true;
            }
        }
        return false;
    }

}