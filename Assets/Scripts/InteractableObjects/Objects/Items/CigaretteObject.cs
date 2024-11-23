using UnityEngine;

public class CigaretteObject : InteractableObject
{
    [Space, Header("Segarro"), SerializeField]
    private float cigarretteSpeed;
    private bool usingCigarrette;
    private float cigarretteProcess;
    private Vector3 starterPos;
    private Quaternion starterRot;
    [SerializeField]
    private float smokeToAdd;
    private float starterSmokeAlpha;
    private float smokeProcess;
    [SerializeField]
    private float smokeSpeed;

    public override void ActivateObject()
    {
        starterPos = transform.position;
        starterRot = transform.rotation;
        cigarretteProcess = 0;
        usingCigarrette = true;
        foreach (Outline item in outline)
        {
            item.enabled = false;
        }
        objectInfo = null;
        outline.Clear();

        starterSmokeAlpha = GameManager.Instance.segarroSmoke.main.startColor.color.a;
        smokeProcess = 0;
        StopHovering();
    }

    private void Update()
    {
        if (usingCigarrette && cigarretteProcess < 1)
        {
            MoveCigarretteToMouth();
        }
        else if(usingCigarrette)
        {
            SmokeCigarrette();
        }
    }

    private void MoveCigarretteToMouth()
    {
        cigarretteProcess += Time.deltaTime * cigarretteSpeed;
        transform.position = Vector3.Lerp(starterPos, GameManager.Instance.cigarretteTransform.position, cigarretteProcess);
        transform.rotation = Quaternion.Lerp(starterRot, GameManager.Instance.cigarretteTransform.rotation, cigarretteProcess);

        if(cigarretteProcess <= 1)
        {
            transform.parent = GameManager.Instance.cigarretteTransform;
        }
    }

    private void SmokeCigarrette()
    {
        smokeProcess += Time.deltaTime * smokeSpeed / 10;

        float finalAlpha = starterSmokeAlpha + smokeToAdd;
        float alphaValue = Mathf.Lerp(starterSmokeAlpha, finalAlpha, smokeProcess);

        ParticleSystem.MainModule mainModule = GameManager.Instance.segarroSmoke.main;
        mainModule.startColor = new Color(1, 1, 1, alphaValue);

        if (smokeProcess >= 1)
            Destroy(gameObject);

    }
}
