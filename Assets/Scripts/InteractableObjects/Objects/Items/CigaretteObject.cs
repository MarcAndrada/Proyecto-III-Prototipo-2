using UnityEngine;

public class CigaretteObject : InteractableObject
{
    [Space, Header("Segarro"), SerializeField]
    private float cigarretteSpeed;
    private bool usingCigarrette;
    private float cigarretteProcess;
    private Vector3 starterPos;
    private Quaternion starterRot;


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

        StopHovering();
    }

    private void Update()
    {
        if (usingCigarrette && cigarretteProcess < 1)
        {
            MoveCigarretteToMouth();
        }
        else
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

    }
}
