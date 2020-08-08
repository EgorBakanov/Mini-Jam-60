using System;
using System.Collections;
using System.Collections.Generic;
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
        Alarm,
        AlarmWait,
    }
    
    public NavMeshAgent navMeshAgent;
    public AiState startState = AiState.Wait;
    public float waitTime;
    public PathStyle pathStyle = PathStyle.PingPong;
    public Transform[] waypoints;

    private int _currentWaypoint;
    private AiState _state;
    private int _waypointNext = 1;
    private Timer _timer;

    private void Start()
    {
        if (waypoints == null)
            waypoints = new[] {transform};

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
        throw new NotImplementedException();
    }

    private void UpdateOnWait()
    {
        throw new NotImplementedException();
    }

    private void NextWaypoint()
    {
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
