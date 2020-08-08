using System;
using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform visualHolder;
    public Vector3 openOffset;
    public float openTime;
    public float closeTime;

    private Vector3 _closedPosition;
    private bool _isOpen;
    private bool _trigger;

    private void Awake()
    {
        _closedPosition = visualHolder.localPosition;
        _isOpen = false;
        _trigger = false;
    }

    private void Start()
    {
        GameManager.Instance.pausableSystemManager.doorSystem.OnUnpause += Close;
    }

    private void OnDestroy()
    {
        GameManager.Instance.pausableSystemManager.doorSystem.OnUnpause -= Close;
    }

    private void Update()
    {
        if (GameManager.Instance.pausableSystemManager.doorSystem.CanOpen && _trigger)
            Open();
        else Close();
    }

    public void Open()
    {
        if (_isOpen)
            return;

        _isOpen = true;
        MoveDoor(_closedPosition + openOffset, openTime);
    }

    public void Close()
    {
        if (!_isOpen)
            return;

        _isOpen = false;
        MoveDoor(_closedPosition, closeTime);
    }

    private void MoveDoor(Vector3 localPos, float time) => visualHolder.DOLocalMove(localPos, time);

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() == null)
            return;

        _trigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() == null)
            return;

        _trigger = false;
    }
}