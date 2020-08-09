using UnityEngine;

namespace _project.Scripts
{
    public class LoseTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform == GameManager.Instance.player)
            {
                Lose();
            }
        }

        private void Lose()
        {
            Debug.Log("LOSE!");
        }
    }
}