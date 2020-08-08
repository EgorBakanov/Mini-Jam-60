﻿using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class AiBrain : MonoBehaviour
{
    public enum PathStyle
    {
        PingPong,
        Circle,
    }

    public enum AiState
    {
        Wait,
        FollowPath,
        Chase,
        ChaseWait,
        Alarm,
        AlarmWait,
    }

    public NavMeshAgent navMeshAgent;
    public AiState startState = AiState.Wait;
    public float waitTime;
    public float rotateOnWaitTime;
    public PathStyle pathStyle = PathStyle.PingPong;
    public Transform[] waypoints;
    public Transform alarmWaypoint;

    private int _currentWaypoint;
    private AiState _state;
    private int _waypointNext = 1;
    private Timer _timer;

    private void Start()
    {
        if (waypoints == null)
            waypoints = new[] {transform};
        if (alarmWaypoint == null)
            alarmWaypoint = transform;

        _currentWaypoint = 0;
        _state = startState;
        _timer = 0;
    }

    private void Update()
    {
        _timer.Tick(Time.deltaTime);
        switch (_state)
        {
            case AiState.Wait:
                UpdateOnWait();
                break;
            case AiState.FollowPath:
                UpdateOnFollowPath();
                break;
            case AiState.Chase:
                UpdateOnChase();
                break;
            case AiState.Alarm:
                UpdateOnAlarm();
                break;
            case AiState.AlarmWait:
                UpdateOnAlarmWait();
                break;
            case AiState.ChaseWait:
                UpdateOnChaseWait();
                break;
        }
    }

    private void UpdateOnChaseWait()
    {
        if (_timer.IsDone)
        {
            navMeshAgent.SetDestination(waypoints[_currentWaypoint].position);
        }
    }

    private void UpdateOnAlarmWait()
    {
        throw new NotImplementedException();
    }

    private void UpdateOnAlarm()
    {
        throw new NotImplementedException();
    }

    private void UpdateOnChase()
    {
        throw new NotImplementedException();
    }

    private void UpdateOnFollowPath()
    {
        if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            _timer = waitTime;
            _state = AiState.Wait;
            navMeshAgent.transform.DORotate(waypoints[_currentWaypoint].forward, rotateOnWaitTime);
        }
    }

    private void UpdateOnWait()
    {
        if (_timer.IsDone)
        {
            NextWaypoint();
            navMeshAgent.SetDestination(waypoints[_currentWaypoint].position);
        }
    }

    private bool CheckForAlarm()
    {
        if (!GameManager.Instance.pausableSystemManager.alarmSystem.IsAlarmed)
            return false;


        return true;
    }

    private void NextWaypoint()
    {
        if (waypoints.Length == 1)
        {
            _currentWaypoint = 0;
            return;
        }

        switch (pathStyle)
        {
            case PathStyle.PingPong:
                if (_currentWaypoint == 0)
                    _waypointNext = 1;
                if (_currentWaypoint == waypoints.Length - 1)
                    _waypointNext = -1;
                _currentWaypoint += _waypointNext;
                break;
            case PathStyle.Circle:
                _currentWaypoint += _waypointNext;
                _currentWaypoint %= waypoints.Length;
                break;
        }
    }
}