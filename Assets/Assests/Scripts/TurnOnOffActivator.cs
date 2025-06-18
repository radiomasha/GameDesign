using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TurnOnOffActivator : MonoBehaviour
{
    [SerializeField] private GameObject _activator;
    [SerializeField] private Transform _fingerTip;
    [SerializeField] private Transform _ship;
    [SerializeField] private Transform _hand;
    [SerializeField] private float _rotationSensitivity = 10f;
    [SerializeField] private float _scaleFactor = 20f;
    //[SerializeField] private GameEvent activate;
    [SerializeField] private GameObject _smallSphere;

    public static event Action OnHandInSphere;

    private float _maxDistance;
    private Vector3 _initialHandPosition;
    private Vector3 _initialShipPosition;
    private Quaternion _initialShipRotation;
    private bool _isHandInVolume = false;
    private bool _isActivated = false;

    private void Awake()
    {
        _maxDistance = GetComponent<SphereCollider>().radius;
        _initialShipPosition = _ship.position;
        _initialShipRotation = _ship.rotation;
    }

    private void OnEnable()
    {
        GetPosition.OnPositionGot += Activated;
    }

    private void OnDisable()
    {
        GetPosition.OnPositionGot -= Activated;
    }

    private void Update()
    {
        if (_isActivated)
        {
            Navigate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.IsChildOf(_fingerTip))
        {
            _isHandInVolume = true;

            if (_activator != null)
                _activator.SetActive(true);

           // activate.Occurred(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.IsChildOf(_fingerTip))
        {
            _isHandInVolume = false;
            _isActivated = false;

            if (_activator != null)
                _activator.SetActive(false);

            if (_smallSphere != null)
                _smallSphere.SetActive(true);

            _ship.position = _initialShipPosition;
            _ship.rotation = _initialShipRotation;
        }
    }

    public void Activated()
    {
        if (!_isHandInVolume) return;

        if (_activator != null)
            _activator.SetActive(true);

        _isActivated = true;
        _initialHandPosition = _hand.position;

        OnHandInSphere?.Invoke();
    }

    private void Navigate()
    {
        Vector3 handOffset = _hand.position - _initialHandPosition;
        Vector3 newPosition = _initialShipPosition + handOffset * _scaleFactor;

        if (handOffset.magnitude <= _maxDistance)
        {
            _ship.position = newPosition;
        }

        float zRotation = _hand.rotation.eulerAngles.z * _rotationSensitivity;
        Vector3 currentEuler = _ship.rotation.eulerAngles;
        _ship.rotation = Quaternion.Euler(currentEuler.x, currentEuler.y, zRotation);
    }
}
