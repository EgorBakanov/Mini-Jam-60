using UnityEngine;

public class Terminal : MonoBehaviour
{
    public PausableSystem system;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() == null)
            return;

        if (!GameManager.Instance.pausableSystemManager.availableSystems.Add(system))
            return;
        
        GameManager.Instance.uiManager.Add(system);
    }
}
