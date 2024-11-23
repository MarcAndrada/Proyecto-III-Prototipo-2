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

    [Header("Explosion Effect")]
    [SerializeField] private ParticleSystem bloodParticles;
    [SerializeField] private GameObject fleshObject;
    private bool playerExploded = false;
    
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

    private void FleshExplode()
    {
        // Activate blood particles at the body's position
        if (bloodParticles != null)
        {
            ParticleSystem instantiatedParticles = Instantiate(bloodParticles, body.transform.position, Quaternion.identity);
            instantiatedParticles.Play();
        }

        for (int i = 0; i < 5; i++)
        {
            if (fleshObject != null)
            {
                GameObject flesh = Instantiate(fleshObject, body.transform.position, Random.rotation);

                Rigidbody rb = flesh.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 randomDirection = Random.insideUnitSphere.normalized; 
                    float randomForce = Random.Range(5f, 10f);                  
                    rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);
                }
            }
        }
    }
    private void CheckIfCrushBody(int _currentHealth)
    {
        if (_currentHealth > 0 
            || Vector3.Distance(new Vector3(transform.position.x, minYAlive, transform.position.z), transform.position) < 1 
            && body.activeInHierarchy
            )
            return;
        
        //Triturar al player
        if (!playerExploded)
        {
            playerExploded = true;
            FleshExplode();
            body.SetActive(false);  
        }

    }
}
