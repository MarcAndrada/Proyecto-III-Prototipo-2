using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangedManController : MonoBehaviour
{
    [SerializeField]
    private bool isPlayer;

    [SerializeField]
    private float minYAlive;   
    private Vector3 destinyPosition;
    [SerializeField]
    private GameObject body;

    // Start is called before the first frame update
    void Start()
    {
        int currentHealth = isPlayer ? GameManager.Instance.playerHangedManHealth : GameManager.Instance.enemyHangedManHealth;
        CalculateDestinyPos(currentHealth);
        transform.position = destinyPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.actionState == GameManager.ActionState.RESULTS)
        {
            int currentHealth = isPlayer ? GameManager.Instance.playerHangedManHealth : GameManager.Instance.enemyHangedManHealth;
            CalculateDestinyPos(currentHealth);
            MoveBodyToDestiny();
            CheckIfCrushBody(currentHealth);
        }
    }

    private void CalculateDestinyPos(int _currentHealth)
    {
        destinyPosition = 
            new Vector3(transform.position.x, minYAlive, transform.position.z) +
            (Vector3.up * _currentHealth * GameManager.Instance.ropeOffset);
    }
    private void MoveBodyToDestiny()
    {
        float currentSpeed;

        if(GameManager.Instance.state == GameManager.GameState.PLAYER_TURN)
            currentSpeed = isPlayer ? GameManager.Instance.moveUpSpeed : GameManager.Instance.moveDownSpeed;
        else
            currentSpeed = isPlayer ? GameManager.Instance.moveDownSpeed : GameManager.Instance.moveUpSpeed;

        transform.position = Vector3.Lerp(transform.position, destinyPosition, Time.deltaTime * currentSpeed);
    }

    private void CheckIfCrushBody(int _currentHealth)
    {
        if (_currentHealth > 0 
            || Vector3.Distance(new Vector3(transform.position.x, minYAlive, transform.position.z), transform.position) < 1 
            && body.activeInHierarchy
            )
            return;

        //Triturar al player
        body.SetActive(false);  

    }
}
