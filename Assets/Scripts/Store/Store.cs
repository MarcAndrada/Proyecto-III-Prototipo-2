using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public enum ItemType
    {
        JokerCard,
        Button,
        Cigar,
        JusticeBalance,
        Joystick,
        RedCoin,
        
    }
    [SerializeField] private int startingItems;
    public void Buy()
    {
        Debug.Log("Apretao");
    }
}
