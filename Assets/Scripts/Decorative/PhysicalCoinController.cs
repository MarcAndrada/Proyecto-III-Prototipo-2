using UnityEngine;

public class PhysicalCoinController : MonoBehaviour
{
    private bool hasCollided;

    private AudioSource source;

    private void Awake()
    {
        hasCollided = false;
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!hasCollided)
        {
            source.Play();
            hasCollided = true;
        }
    }

}
