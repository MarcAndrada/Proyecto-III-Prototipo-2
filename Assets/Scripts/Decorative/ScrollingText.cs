using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    [SerializeField] private float speed = 500f;

    private RectTransform _rectTransform;
    private TextMeshProUGUI _textMeshPro;
    
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private bool _isScrolling = false;
    private bool _isLooping = false;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string message)
    {
        _textMeshPro.text = message;
        _startPosition = new Vector3(-_rectTransform.rect.width, _rectTransform.localPosition.y, 0);
        _endPosition = new Vector3(Screen.width + _rectTransform.rect.width, _rectTransform.localPosition.y, 0);
        _rectTransform.localPosition = _startPosition;
        _isScrolling = true;
    }
    public void SetTextLoop(string message)
    {
        _textMeshPro.text = message;
        _startPosition = new Vector3(-_rectTransform.rect.width, _rectTransform.localPosition.y, 0);
        _endPosition = new Vector3(Screen.width + _rectTransform.rect.width, _rectTransform.localPosition.y, 0);
        _rectTransform.localPosition = _startPosition;
        _isScrolling = true;
        _isLooping = true;
    }
    private void Update()
    {
        if (_isScrolling)
        {
            _rectTransform.localPosition = Vector3.MoveTowards(_rectTransform.localPosition, _endPosition, speed * Time.deltaTime);

            if (_rectTransform.localPosition.x >= _endPosition.x)
            {
                _isScrolling = false;
            }
        }
        if (_isLooping)
        {
            _rectTransform.localPosition = Vector3.MoveTowards(_rectTransform.localPosition, _endPosition, speed * Time.deltaTime);

            if (_rectTransform.localPosition.x >= _endPosition.x)
            {
                _rectTransform.localPosition = _startPosition;
            }
        }
    }
}
