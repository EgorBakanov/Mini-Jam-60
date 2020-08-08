using UnityEngine;

public class UiManager : MonoBehaviour
{
    public RectTransform systemPauserHolder;
    public SystemPauserUi systemPauserUiPrefab;

    private void Start()
    {
        ClearSystemHolders();
    }

    public void Add(PausableSystem system)
    {
        Instantiate(systemPauserUiPrefab, systemPauserHolder).Init(system);
    }

    public void ClearSystemHolders()
    {
        var children = systemPauserHolder.GetComponentsInChildren<Transform>();
        for (int i = children.Length - 1; i > 0; i--)
        {
            Destroy(children[i].gameObject);
        }
    }
}
