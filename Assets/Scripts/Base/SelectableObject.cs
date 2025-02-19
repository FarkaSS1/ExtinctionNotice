using UnityEngine;

public abstract class SelectableObject : MonoBehaviour
{
    protected Renderer objectRenderer;
    private Color originalColor;
    public Color selectedColor = Color.green; // Highlight color when selected

    protected int cost = 300; // Default placeholder cost
    protected string costType = "elementX"; // Placeholder cost type

    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
            originalColor = objectRenderer.material.color;

        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing a Collider! Adding BoxCollider.");
            gameObject.AddComponent<BoxCollider>(); // Legal and fine to auto-add
        }
    }

    public virtual void Select()
    {
        Debug.Log($"{gameObject.name} Selected! Cost: {cost} {costType}");
        if (objectRenderer != null)
            objectRenderer.material.color = selectedColor; // Highlight selection

        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.SetSelectedObject(this);
        }
    }

    public virtual void Deselect()
    {
        Debug.Log($"{gameObject.name} Deselected!");
        if (objectRenderer != null)
            objectRenderer.material.color = originalColor; // Restore original color
    }

    public virtual int GetCost()
    {
        return cost;
    }

    public virtual string GetCostType()
    {
        return costType;
    }
}
