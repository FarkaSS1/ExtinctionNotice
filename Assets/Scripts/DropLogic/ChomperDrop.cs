using UnityEngine;

public class ChomperDrop : MonoBehaviour, IDroppable
{
    [SerializeField] private GameObject dropPrefab; // Assign "ElementX" in Inspector
    [SerializeField] private string resourceType = "elementX";
    [SerializeField] private int dropAmount = 10;

    public GameObject GetDropPrefab() => dropPrefab;
    public string GetResourceType() => resourceType;
    public int GetDropAmount() => dropAmount;

    public void Drop()
    {
        GameObject drop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        DropItem dropItem = drop.GetComponent<DropItem>();
        if (dropItem != null)
        {
            dropItem.resourceType = resourceType;
            dropItem.amount = dropAmount;
        }
    }
}
