using UnityEngine;

public class ImageFloatEffect : MonoBehaviour
{
    [Range(0, 10), SerializeField]
    public float speed = 2.0f;

    [Range(1, 3f), SerializeField]
    public float maxExpand = 1.1f;

    [Range(0.2f, 2.5f), SerializeField]
    public float minExpand = 1.02f;

    private Vector3 scaleComp;
    [field: SerializeField]
    public bool canFloat { get;  set; }

    private float currentTime;
    
    void Awake()
    {
        scaleComp = transform.localScale;
    }

    void Update()
    {
        if (canFloat)
        {
            transform.localScale = Vector3.Lerp(scaleComp * minExpand, scaleComp * maxExpand, Mathf.PingPong(Time.unscaledTime * speed, 1));
        }
        else
        {
            transform.localScale = scaleComp;
        }
    }

}
