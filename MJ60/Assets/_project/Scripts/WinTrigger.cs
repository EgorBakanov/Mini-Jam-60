using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.Instance.player)
        {
            Win();
        }
    }

    private void Win()
    {
        Debug.Log("WIN");
    }
}
