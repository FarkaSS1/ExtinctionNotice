using UnityEngine;

public class ChomperDrop : MonoBehaviour, IDroppable
{
    [SerializeField] private GameObject dropPrefab; // Assign "ElementX" in Inspector
    [SerializeField] private string resourceType = "elementX";
    [SerializeField] private int dropAmount = 10;

    public GameObject GetDropPrefab() => dropPrefab;
    public string GetResourceType() => resourceType;
    public int GetDropAmount() => dropAmount;
}
