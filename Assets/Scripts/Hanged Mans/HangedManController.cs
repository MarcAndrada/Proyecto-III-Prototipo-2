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

    private int currentHealth;

    [Header("Explosion Effect")]
    [SerializeField] 
    private ParticleSystem bloodParticles;
    [SerializeField] 
    private GameObject fleshObject;
    private bool playerExploded = false;
    
    void Start()
    {
        currentHealth = isPlayer ? GameManager.Instance.playerHangedManHealth : GameManager.Instance.enemyHangedManHealth;
        CalculateDestinyPos(currentHealth);
        transform.position = destinyPosition;
    }

    void Update()
    {
        currentHealth = isPlayer ? GameManager.Instance.playerHangedManHealth : GameManager.Instance.enemyHangedManHealth;
        if(GameManager.Instance.actionState == GameManager.ActionState.RESULTS)
        {
            CalculateDestinyPos(currentHealth);
            MoveBodyToDestiny();
        }
        CheckIfCrushBody(currentHealth);
    }

    private void CalculateDestinyPos(int _currentHealth)
    {
        destinyPosition = 
            new Vector3(transform.position.x, minYAlive - GameManager.Instance.ropeOffset, transform.position.z) +
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
        if (bloodParticles != null)
        {
            Vector3 particleRotation = new Vector3(0f, 90f, 0f);
            ParticleSystem instantiatedParticles = Instantiate(bloodParticles, body.transform.position, Quaternion.Euler(particleRotation));
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
            || transform.position.y - minYAlive > 0.5f
            || playerExploded
            )
            return;

        //Triturar al player
        playerExploded = true;
        FleshExplode();
        body.SetActive(false);
    }
}
