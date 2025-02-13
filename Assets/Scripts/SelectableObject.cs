using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color originalColor;
    public Color selectedColor = Color.green; // Highlight color when selected

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
            originalColor = objectRenderer.material.color;

        if (GetComponent<Collider>() == null)
        {
            Debug.LogError(gameObject.name + " is missing a Collider! Adding BoxCollider.");
            gameObject.AddComponent<BoxCollider>(); // Auto-add a BoxCollider if missing
        }
    }


    public void Select()
    {
        Debug.Log(gameObject.name + " Selected!");
        if (objectRenderer != null)
            objectRenderer.material.color = selectedColor; // Change color when selected
    }

    public void Deselect()
    {
        Debug.Log(gameObject.name + " Deselected!");
        if (objectRenderer != null)
            objectRenderer.material.color = originalColor; // Revert to original color
    }
}
