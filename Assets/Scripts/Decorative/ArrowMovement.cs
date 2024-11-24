using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private Vector2 startPoint;
    [SerializeField] private Vector2 endPoint;
    [SerializeField] private float speed = 1f;

    private RectTransform rectTransform;
    private bool movingToEnd = true;
    private float progress = 0f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (movingToEnd)
            progress += Time.deltaTime * speed;
        else
            progress -= Time.deltaTime * speed;

        progress = Mathf.Clamp01(progress);
        
        rectTransform.anchoredPosition = Vector2.Lerp(startPoint, endPoint, progress);

        if (progress >= 1f)
            movingToEnd = false;
        else if (progress <= 0f)
            movingToEnd = true;
        
    }
}
