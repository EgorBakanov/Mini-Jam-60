using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public new Camera camera;
    public LayerMask floorLayer;

    private bool _shouldMove;

    private void Start()
    {
        GameManager.Instance.player = transform;
        _shouldMove = false;
    }

    private void Update() => _shouldMove = Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject();

    private void FixedUpdate()
    {
        if(!_shouldMove)
            return;
        
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, floorLayer))
            return;

        var direction = hit.point - transform.position;
        direction.y = 0;
        direction.Normalize();
        
        var newDir = Vector3.RotateTowards(transform.forward, direction,
            navMeshAgent.angularSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        navMeshAgent.Move(Time.deltaTime * navMeshAgent.speed * direction);

        // navMeshAgent.SetDestination(hit.point);
    }
}