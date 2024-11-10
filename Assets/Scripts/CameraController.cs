using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToFollow;

    [Space, SerializeField]
    private float cameraSpeed;

    [Space, SerializeField]
    private Vector3 offsetFromObject;
    [SerializeField]
    private float cameraDistance;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, CalculateObjectOffset(), Time.deltaTime * cameraSpeed);

        transform.forward = (objectToFollow.transform.position - transform.position).normalized;

    }
    private Vector3 CalculateObjectOffset()
    {
        offsetFromObject = offsetFromObject.normalized;
        return objectToFollow.transform.position + offsetFromObject * cameraDistance;
    }


    private void OnDrawGizmosSelected()
    {
        offsetFromObject = offsetFromObject.normalized;
        if (!objectToFollow)
            return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(objectToFollow.transform.position, objectToFollow.transform.position + offsetFromObject * cameraDistance);

    }

}
