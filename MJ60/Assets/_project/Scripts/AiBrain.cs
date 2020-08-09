using System;
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

    public float viewAngel;
    public float viewDistance;
    public NavMeshAgent navMeshAgent;
    public float waitTime;
    public float chaseWaitTime;
    public float rotateOnWaitTime;
    public Ease rotationEase;
    public PathStyle pathStyle = PathStyle.PingPong;
    public Transform[] waypoints;
    public Transform alarmWaypoint;

    private int _currentWaypoint;
    private AiState _state;
    private int _waypointNext = 1;
    private Timer _timer;
    private bool _canSee;

    private void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
            waypoints = new[] {transform};
        if (alarmWaypoint == null)
            alarmWaypoint = transform;

        _currentWaypoint = 0;
        _state = AiState.FollowPath;
        navMeshAgent.SetDestination(waypoints[_currentWaypoint].position);
        _timer = 0;
        _canSee = false;
    }

    private void Update()
    {
        CheckPlayer();
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
        if (!_timer.Tick(Time.deltaTime)) return;

        navMeshAgent.SetDestination(waypoints[_currentWaypoint].position);
        _state = AiState.FollowPath;
    }

    private void UpdateOnAlarmWait()
    {
        if (GameManager.Instance.pausableSystemManager.alarmSystem.IsAlarmed) return;

        _state = AiState.FollowPath;
        navMeshAgent.SetDestination(waypoints[_currentWaypoint].position);
    }

    private void UpdateOnAlarm()
    {
        if (!GameManager.Instance.pausableSystemManager.alarmSystem.IsAlarmed)
        {
            _state = AiState.FollowPath;
            navMeshAgent.SetDestination(waypoints[_currentWaypoint].position);
            return;
        }

        if (!PathComplete()) return;

        _state = AiState.AlarmWait;
        transform.DOLookAt(alarmWaypoint.position + alarmWaypoint.forward, rotateOnWaitTime,
            AxisConstraint.Y).SetEase(rotationEase);
    }

    private void UpdateOnChase()
    {
        if (!PathComplete()) return;

        _state = AiState.ChaseWait;
        _timer = chaseWaitTime;
    }

    private void UpdateOnFollowPath()
    {
        if (CheckForAlarm()) return;
        if (!PathComplete()) return;

        _state = AiState.Wait;
        _timer = waitTime;
        transform.DOLookAt(waypoints[_currentWaypoint].position + waypoints[_currentWaypoint].forward, rotateOnWaitTime,
            AxisConstraint.Y).SetEase(rotationEase);
    }

    private void UpdateOnWait()
    {
        if (CheckForAlarm()) return;
        if (!_timer.Tick(Time.deltaTime)) return;

        _state = AiState.FollowPath;
        NextWaypoint();
        navMeshAgent.SetDestination(waypoints[_currentWaypoint].position);
    }

    private bool CheckForAlarm()
    {
        if (!GameManager.Instance.pausableSystemManager.alarmSystem.IsAlarmed)
            return false;

        navMeshAgent.SetDestination(alarmWaypoint.position);
        _state = AiState.Alarm;
        return true;
    }

    private void CheckPlayer()
    {
        if (!_canSee)
            return;
        
        var player = GameManager.Instance.player;
        var direction = (player.position - transform.position).normalized;
        if (Mathf.Abs(Vector3.Angle(direction, transform.forward)) > viewAngel)
            return;

        _state = AiState.Chase;
        navMeshAgent.SetDestination(player.position);
        GameManager.Instance.pausableSystemManager.alarmSystem.Alarm();
    }

    private void FixedUpdate()
    {
        var player = GameManager.Instance.player;
        var direction = (player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out var hitInfo, viewDistance))
        {
            _canSee = hitInfo.transform == player;
        }
    }

    private bool PathComplete()
    {
        return Vector3.Distance(transform.position, navMeshAgent.destination) <= navMeshAgent.stoppingDistance;

        // if (navMeshAgent.pathPending)
        //     return false;
        // if (!(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance))
        //     return false;
        // return !navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f;
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