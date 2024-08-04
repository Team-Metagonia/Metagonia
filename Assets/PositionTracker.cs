using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _lastPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody component is missing.");
        }
    }

    private void Start()
    {
        _lastPosition = _rigidbody.position;
        Debug.Log($"Initial Position: {_lastPosition}");
    }

    private void FixedUpdate()
    {
        Vector3 currentPosition = _rigidbody.position;

        if (currentPosition != _lastPosition)
        {
            Debug.Log($"Position changed from {_lastPosition} to {currentPosition}");
            _lastPosition = currentPosition;
        }
    }
}