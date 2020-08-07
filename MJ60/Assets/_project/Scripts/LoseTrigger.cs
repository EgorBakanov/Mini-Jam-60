using System;
using UnityEngine;

namespace _project.Scripts
{
    public class LoseTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>() != null)
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