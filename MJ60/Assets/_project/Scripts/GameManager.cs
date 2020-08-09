using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PausableSystemManager pausableSystemManager;
    public UiManager uiManager;
    public Transform player;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}