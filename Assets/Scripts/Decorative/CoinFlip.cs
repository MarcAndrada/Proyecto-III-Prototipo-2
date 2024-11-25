using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CoinFlip : MonoBehaviour
{
    [SerializeField] private Sprite headsSprite;
    [SerializeField] private Sprite tailsSprite;
    [SerializeField] private Image coinImage;
    
    private bool isFlipping = false;
    private bool isHeads;
    private bool shouldDeactivate = false;
    private RectTransform rectTransform;
    
    [Header("Timers")]
    [SerializeField] private float flipDuration = 1f;
    [SerializeField] private float deactivateDelay = 1f;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float vibrationDuration = 0.2f;
    [SerializeField] private float vibrationIntensity = 10f;
    
    private float flipTime = 0f;
    private float deactivateTimer = 0f;
    private float slideTime = 0f;
    private float vibrationTime = 0f;

    // Original Positions
    private Vector3 originalScale;
    private Vector2 originalPosition;
    private Vector2 targetPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); 
    }

    private void Start()
    {
        originalPosition = rectTransform.anchoredPosition;
        targetPosition = originalPosition + new Vector2(0, -Screen.height * 2f);
    }
    
    void Update()
    {
        if (isFlipping)
        {
            flipTime += Time.deltaTime;

            coinImage.transform.localScale = new Vector3(1, Mathf.Sin(flipTime * Mathf.PI * 4), 1);

            if (flipTime >= flipDuration)
            {
                EndFlip();
            }
        }
        
        if (shouldDeactivate)
        {
            deactivateTimer += Time.deltaTime;

            if (deactivateTimer >= deactivateDelay)
            {
                slideTime += Time.deltaTime;

                float t = Mathf.Clamp01(slideTime / slideDuration);
                rectTransform.anchoredPosition = Vector2.Lerp(originalPosition, targetPosition, t);

                if (slideTime >= slideDuration)
                {
                    coinImage.gameObject.SetActive(false);
                    shouldDeactivate = false;
                    slideTime = 0f; 
                    rectTransform.anchoredPosition = originalPosition;
                }
            }
        }
        
        if (vibrationTime > 0f)
        {
            vibrationTime -= Time.deltaTime;
            Vector2 vibration = new Vector2(Random.Range(-vibrationIntensity, vibrationIntensity), Random.Range(-vibrationIntensity, vibrationIntensity));
            rectTransform.anchoredPosition += vibration;

            if (vibrationTime <= 0f)
            {
                rectTransform.anchoredPosition = originalPosition;
            }
        }
    }

    public void FlipCoin()
    {
        if (isFlipping) return;

        isFlipping = true;
        flipTime = 0f;
        coinImage.gameObject.SetActive(true);
        isHeads = Random.value > 0.5f;
        
        rectTransform.anchoredPosition = originalPosition;
    }

    private void EndFlip()
    {
        coinImage.sprite = isHeads ? headsSprite : tailsSprite;

        coinImage.transform.localScale = Vector3.one;

        isFlipping = false;
        
        vibrationTime = vibrationDuration;
        
        deactivateTimer = 0f;
        shouldDeactivate = true;
    }

    public bool GetIsFlipping()
    {
        return isFlipping;
    }
    public bool Result()
    {
        return isHeads;
    }
}
