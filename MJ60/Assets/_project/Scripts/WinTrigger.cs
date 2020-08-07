using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            Win();
        }
    }

    private void Win()
    {
        Debug.Log("WIN");
    }
}
