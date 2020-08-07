using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public new Camera camera;
    
    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if(EventSystem.current.IsPointerOverGameObject()) return;
            
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
        {
            navMeshAgent.SetDestination(hit.point);
        }
    }
}
