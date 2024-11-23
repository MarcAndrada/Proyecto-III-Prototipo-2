using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoneyController : MonoBehaviour
{
    [SerializeField]
    private Transform coinSpawnPos;
    [SerializeField] 
    private Vector2 coinSpawnOffset;
    [SerializeField]
    private GameObject coinPrefab;
    [SerializeField]
    private float spawnRandomRotationForce;
    [SerializeField]
    private MoneyCanvasController moneyCanvas;

    private List<GameObject> coinList = new List<GameObject>();

    private int totalCoins;

    private void Start()
    {
        moneyCanvas.UpdateCoins(totalCoins);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddCoins(1);
        }
    }

    public void AddCoins(int _addedCoins)
    {
        totalCoins += _addedCoins;

        for (int i = 0; i < _addedCoins; i++)
        {
            //Spawnear moneda
            Vector3 randomPos = new Vector3(
                Random.Range(-coinSpawnOffset.x, coinSpawnOffset.x),
                Random.Range(-coinSpawnOffset.y, coinSpawnOffset.y),
                Random.Range(-coinSpawnOffset.x, coinSpawnOffset.x));

            Vector3 spawnPos = coinSpawnPos.transform.position + randomPos;

            GameObject newCoin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);

            newCoin.GetComponent<Rigidbody>().angularVelocity = new Vector3(
                Random.Range(-spawnRandomRotationForce, spawnRandomRotationForce),
                Random.Range(-spawnRandomRotationForce, spawnRandomRotationForce),
                Random.Range(-spawnRandomRotationForce, spawnRandomRotationForce)
                );

            coinList.Add(newCoin);
        }

        moneyCanvas.UpdateCoins(totalCoins) ;
    }

    public bool RemoveCoins(int _coinAmount)
    {
        if(_coinAmount > totalCoins)
            return false;

        for (int i = 0; i < _coinAmount; i++)
        {
            //Restar Moneda
            totalCoins--;

            //Destruir la ultima Moneda
            Destroy(coinList[coinList.Count - 1]);
            //Quitar la ultima moneda de la lista
            coinList.RemoveAt(coinList.Count - 1);
        }

        moneyCanvas.UpdateCoins(totalCoins);

        return true;
    }

    public int GetCoinAmount() { return totalCoins; }   
}
