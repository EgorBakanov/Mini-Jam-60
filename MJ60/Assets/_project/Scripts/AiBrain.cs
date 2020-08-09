using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
    public Light viewLight;
    public float viewLightChangeTime;
    public Color normalState;
    public Color chaseState;
    public Color alarmState;
    public float cantNavigateShake;

    private int _currentWaypoint;
    private AiState _state;
    private int _waypointNext = 1;
    private Timer _timer;
    private bool _detectPlayer;
    private float _speed;

    private void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
            waypoints = new[] {transform};
        if (alarmWaypoint == null)
            alarmWaypoint = transform;

        _currentWaypoint = 0;
        _state = AiState.FollowPath;
        viewLight.DOColor(normalState, viewLightChangeTime);
        navMeshAgent.SetDestination(waypoints[_currentWaypoint].position);
        _timer = 0;
        _detectPlayer = false;
        _speed = navMeshAgent.speed;
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
        viewLight.DOColor(normalState, viewLightChangeTime);
        _state = AiState.FollowPath;
    }

    private void UpdateOnAlarmWait()
    {
        if (GameManager.Instance.pausableSystemManager.alarmSystem.IsAlarmed) return;

        _state = AiState.FollowPath;
        viewLight.DOColor(normalState, viewLightChangeTime);
        navMeshAgent.SetDestination(waypoints[_currentWaypoint].position);
    }

    private void UpdateOnAlarm()
    {
        if (!GameManager.Instance.pausableSystemManager.alarmSystem.IsAlarmed)
        {
            _state = AiState.FollowPath;
            viewLight.DOColor(normalState, viewLightChangeTime);
            navMeshAgent.SetDestination(waypoints[_currentWaypoint].position);
            return;
        }

        if (!PathCompleted()) return;

        _state = AiState.AlarmWait;
        transform.DOLookAt(alarmWaypoint.position + alarmWaypoint.forward, rotateOnWaitTime,
            AxisConstraint.Y).SetEase(rotationEase);
    }

    private void UpdateOnChase()
    {
        if (!PathCompleted()) return;

        _state = AiState.ChaseWait;
        _timer = chaseWaitTime;
    }

    private void UpdateOnFollowPath()
    {
        if (CheckForAlarm()) return;
        if (!PathCompleted()) return;

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
        viewLight.DOColor(alarmState, viewLightChangeTime);
        _state = AiState.Alarm;
        return true;
    }

    private void CheckPlayer()
    {
        if (!_detectPlayer)
            return;

        var player = GameManager.Instance.player;
        var direction = (player.position - transform.position).normalized;
        if (Mathf.Abs(Vector3.Angle(direction, transform.forward)) > viewAngel)
            return;

        _state = AiState.Chase;
        viewLight.DOColor(chaseState, viewLightChangeTime);
        navMeshAgent.SetDestination(player.position);
        GameManager.Instance.pausableSystemManager.alarmSystem.Alarm();
    }

    private void FixedUpdate()
    {
        CheckCanSeePlayer();

        if (GameManager.Instance.pausableSystemManager.navigationSystem.IsPaused && !PathCompleted())
        {
            var direction = transform.forward + transform.right * Random.Range(-cantNavigateShake, cantNavigateShake);
            direction.Normalize();

            var newDir = Vector3.RotateTowards(transform.forward, direction,
                navMeshAgent.angularSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            navMeshAgent.Move(Time.deltaTime * _speed * direction);
        }

        navMeshAgent.speed = GameManager.Instance.pausableSystemManager.navigationSystem.IsPaused ? 0f : _speed;
    }

    private void CheckCanSeePlayer()
    {
        var player = GameManager.Instance.player;
        var direction = player.position - transform.position;
        if (direction.sqrMagnitude > viewDistance * viewDistance)
        {
            _detectPlayer = false;
            return;
        }
        
        if (Physics.Raycast(transform.position, direction, out var hitInfo))
            _detectPlayer = hitInfo.transform == player;
    }

    private bool PathCompleted()
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