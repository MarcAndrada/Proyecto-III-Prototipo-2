using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public enum ItemType
    {
        JOKER,
        INTERRUPTOR,
        CIGARRETTE,
        BALANCE,
        JOYSTICK,
        RED_COIN
    }
    [Header("Items")]
    [SerializeField] private GameObject[] shopItems;
    [SerializeField] private Sprite[] itemSprites;
    [SerializeField] private int[] itemPrices;
    
    [Header("Containers")]
    [SerializeField] private int maxItems;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private InventoryManager inventory;
    
    private int[] randomItemIndexes;
    private List<Button> generatedButtons = new List<Button>();

    void Start()
    {
        if (!buttonContainer)
        {
            buttonContainer = transform;
        }
        GenerateShopItems();
    }
    private void GenerateShopItems()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
        
        randomItemIndexes = new int[maxItems];

        for (int i = 0; i < randomItemIndexes.Length; i++)
        {
            int randomIndex = Random.Range(0, shopItems.Length);
            randomItemIndexes[i] = randomIndex;

            GameObject buttonObj = Instantiate(buttonPrefab, buttonContainer);

            Button button = buttonObj.GetComponent<Button>();
            Image buttonImage = button.GetComponent<Image>();
            TextMeshProUGUI priceText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            buttonImage.sprite = itemSprites[randomIndex];
            priceText.text = $"${itemPrices[randomIndex]}";
            
            generatedButtons.Add(button);

            int index = i;
            button.onClick.AddListener(() => BuyItem(index));
        }
    }
    private void BuyItem(int buttonIndex)
    {
        
    }
}
