using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PausableSystemManager pausableSystemManager;
    public UiManager uiManager;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        pausableSystemManager.testSystem.OnPause += () => Debug.Log("pause");
        pausableSystemManager.testSystem.OnUnpause += () => Debug.Log("unpause");
        pausableSystemManager.testSystem.OnValueChanged += (val) => Debug.Log($"val = {val}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            pausableSystemManager.testSystem.Pause();
        }
    }
}