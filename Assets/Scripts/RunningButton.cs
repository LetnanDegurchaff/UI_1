using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunningButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;

    private RectTransform _rectTransform;
    private Coroutine _coroutine;

    private Vector3 _firstPosition;
    private Vector3 _secondPosition;

    private Image _image;
    private bool IsEsceped;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _coroutine = null;

        _firstPosition = _rectTransform.position;
        _secondPosition = _target.position;

        _image = GetComponent<Image>();
        IsEsceped = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Run());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _image.color = Color.red;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _image.color = Color.white;
    }

    private IEnumerator Run()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        Vector3 startPosition = _rectTransform.position;
        Vector3 targetPosition;

        float elapsedTime = 0.00001f;
        float duration;

        if (IsEsceped)
        {
            targetPosition = _firstPosition;
            duration = (_rectTransform.position.x - targetPosition.x) / _speed;
        }
        else
        {
            targetPosition = _secondPosition;
            duration = (targetPosition.x - _rectTransform.position.x) / _speed;
        }

        IsEsceped = !IsEsceped;

        while (_rectTransform.position != targetPosition)
        {
            _rectTransform.position = Vector3.Lerp
                (startPosition, targetPosition, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return waitForEndOfFrame;
        }
    }
}