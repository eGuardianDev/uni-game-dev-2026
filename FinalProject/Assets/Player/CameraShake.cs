using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    private Vector3 _originalPos;
    private float _shakeDuration;
    private float _shakeMagnitude;
    private float _dampingSpeed = 1.0f;

    void Awake()
    {
        Instance = this;
        _originalPos = transform.localPosition;
    }

    void Update()
    {
        if (_shakeDuration > 0)
        {
            transform.localPosition = _originalPos + Random.insideUnitSphere * _shakeMagnitude;
            _shakeDuration -= Time.deltaTime * _dampingSpeed;
        }
        else
        {
            _shakeDuration = 0f;
            transform.localPosition = _originalPos;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        _originalPos = transform.localPosition;
        _shakeDuration = duration;
        _shakeMagnitude = magnitude;
    }
}
