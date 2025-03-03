using UnityEngine;

public class DropItem : MonoBehaviour
{
    public string resourceType;  // Assigned when instantiated
    public int amount;           // Assigned when instantiated

    [SerializeField] private float pickupRadius = 2f; // How close the player needs to be
    private bool isCollected = false;

    private void Update()
    {
        DetectPlayerAndPickup();
    }

    private void DetectPlayerAndPickup()
    {
        if (isCollected) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player")) // Check if it's the player
            {
                Collect(collider.gameObject);
                break;
            }
        }
    }

    private void Collect(GameObject player)
    {
        if (isCollected) return;
        isCollected = true;

        // Add resource to game state
        GameStateManager.Instance.AddResource(resourceType, amount);

        Debug.Log($"Player picked up {amount} of {resourceType}!");

        // Destroy the drop object
        Destroy(gameObject);
    }
}
