using UnityEngine;

public abstract class SelectableObject : MonoBehaviour
{
    protected Renderer towerRenderer;
    protected GameObject blueprint;
    private Color originalColor;
    public Color selectedColor = Color.green;

    [SerializeField] private int cost = 300;
    [SerializeField] private string costType = "elementX";

    private UIManagerBot uiManager;



    public virtual void Start()
    {
        // Check for Collider
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning($"[{gameObject.name}] Missing Collider! Adding BoxCollider.");
            gameObject.AddComponent<BoxCollider>();
        }
    }
    private void OnEnable()
    {
        FindUIManager();
    }

    private void FindUIManager()
    {
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManagerBot>();
            if (uiManager != null)
            {
                Debug.Log($"[{gameObject.name}] Successfully connected to UIManager");
            }
        }
    }

    public virtual void Select()
    {

        towerRenderer = this.GetComponentInChildren<Renderer>();

        Debug.Log($"[{gameObject.name}] Selected! Cost: {cost} {costType}");

        if (towerRenderer != null)
        {
            towerRenderer.material.color = selectedColor;
        }

        if (uiManager == null)
        {
            FindUIManager();
        }

        if (uiManager != null)
        {
            uiManager.SetSelectedObject(this);
        }
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
