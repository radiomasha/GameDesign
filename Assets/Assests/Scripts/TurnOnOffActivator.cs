using System;
using UnityEngine;

public class TurnOnOffActivator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _activator;
    [SerializeField] private Transform _hand;
    [SerializeField] private Transform _ship;
    [SerializeField] private GameObject _smallSphere;
    

    [Header("Flight Settings")]
    [SerializeField] private float _movementRadius = 1f;
    [SerializeField] private float _rotationSensitivity = 1f;
    [SerializeField] private float _followSpeed = 5f;

    [Header("Return Settings")]
    [SerializeField] private float _returnDelay = 3f;
    [SerializeField] private float _returnSpeed = 2f;

    public static event Action OnHandInSphere;

    private Vector3 _initialShipPosition;
    private Vector3 _triggerCenter;
    private float _triggerRadius;
    private bool _isHandInVolume = false;
    public bool _isActivated = false;
    private float _exitTime = -1f;
    private Vector3 _targetPosition;
    private bool isStarted = false;

    private void Awake()
    {
        _initialShipPosition = _ship.position;
        _triggerCenter = transform.position;

        SphereCollider sc = GetComponent<SphereCollider>();
        if (sc != null)
            _triggerRadius = transform.lossyScale.x * sc.radius; // масштаб учитывается
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("FingerCollider"))
        {
            
            _isHandInVolume = true;
            _exitTime = -1f;
            _isActivated = true;
            _smallSphere?.SetActive(false);
            _activator?.SetActive(true);

            OnHandInSphere?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FingerCollider"))
        {
            _isHandInVolume = false;
            _exitTime = Time.time;

            _activator?.SetActive(false);
            _smallSphere?.SetActive(true);
        }
    }

    private void Update()
    {
        if (!_isActivated) return;

        if (_isHandInVolume)
        {
            UpdateTargetFromHand(); // Обновляем цель по руке
            UpdateShipRotation();
            MoveShipToTarget(_followSpeed);
        }
        else
        {
            HandleReturnTimer();
        }

        
    }

    private void UpdateTargetFromHand()
    {
        Vector3 handOffset = _hand.position - _triggerCenter;
        Vector2 relative2D = new Vector2(handOffset.x, handOffset.y) / _triggerRadius;

        if (relative2D.magnitude > 1f)
            relative2D = relative2D.normalized;

        _targetPosition = _initialShipPosition + new Vector3(relative2D.x, relative2D.y, 0f) * _movementRadius;
    }

    private void HandleReturnTimer()
    {
        if (_exitTime > 0 && Time.time - _exitTime > _returnDelay)
        {
            _targetPosition = _initialShipPosition;
            MoveShipToTarget(_returnSpeed);

            if (Vector3.Distance(_ship.position, _initialShipPosition) < 0.01f)
            {
                _isActivated = false;
                _exitTime = -1f;
                _ship.rotation = Quaternion.identity;
            }
        }
    }

    private void MoveShipToTarget(float speed)
    {
        _ship.position = Vector3.Lerp(_ship.position, _targetPosition, Time.deltaTime * speed);
    }

    private void UpdateShipRotation()
    {
        float zRotation = _hand.rotation.eulerAngles.z * _rotationSensitivity;
        _ship.rotation = Quaternion.Euler(0, 0, zRotation);
    }
}
