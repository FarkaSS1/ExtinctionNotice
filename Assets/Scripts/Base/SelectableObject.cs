using UnityEngine;

public abstract class SelectableObject : MonoBehaviour
{
    protected Renderer objectRenderer;
    protected Renderer towerRenderer;
    protected GameObject blueprint;
    private Color originalColor;
    public Color selectedColor = Color.green; // Highlight color when selected

    [SerializeField] private int cost = 300; // Default placeholder cost
    [SerializeField] private string costType = "elementX"; // Placeholder cost type

    private UIManager uiManager;

   public virtual void Start()
{
    objectRenderer = GetComponentInChildren<Renderer>();

    if (objectRenderer != null && objectRenderer.material.HasProperty("_Color"))
    {
        originalColor = objectRenderer.material.color;
        Debug.Log($"{gameObject.name} Renderer and material color property found.");
    }
    else
    {
        Debug.LogWarning($"{gameObject.name} Renderer or material color property is missing!");
    }

    if (GetComponent<Collider>() == null)
    {
        Debug.LogWarning($"{gameObject.name} is missing a Collider! Adding BoxCollider.");
        gameObject.AddComponent<BoxCollider>();
    }

    uiManager = FindObjectOfType<UIManager>(); // Store reference once
}
    public virtual void Select()
    {
        towerRenderer = this.GetComponentInChildren<Renderer>();
        Debug.Log($"{gameObject.name} Selected! Cost: {cost} {costType}");

        if (towerRenderer != null)
            towerRenderer.material.color = selectedColor;

        if (uiManager != null)
            uiManager.SetSelectedObject(this);
    }

    public virtual void Deselect()
    {
        towerRenderer = this.GetComponentInChildren<Renderer>();
        Debug.Log($"{gameObject.name} Deselected!");

        if (towerRenderer != null)
            towerRenderer.material.color = originalColor;
    }

    internal abstract int GetCost();
    internal abstract string GetCostType();

    public int Cost => cost;
    public string CostType => costType;
}
