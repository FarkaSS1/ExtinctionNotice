using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private SelectableObject selectedObject;
    private Transform lastSelectedObject;
    private Color originalColor;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            Debug.Log("Mouse Clicked at: " + Input.mousePosition);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("Raycast Hit: " + hit.collider.gameObject.name + " at " + hit.point);

                SelectableObject newSelection = hit.collider.GetComponent<SelectableObject>();

                if (newSelection != null)
                {
                    Debug.Log("Selectable Object Found: " + hit.collider.gameObject.name);

                    if (selectedObject != null)
                        selectedObject.Deselect(); // Deselect previous

                    selectedObject = newSelection;
                    selectedObject.Select(); // Select new
                }
                else
                {
                    Debug.Log("Object is NOT Selectable: " + hit.collider.gameObject.name);

                    if (selectedObject != null)
                    {
                        selectedObject.Deselect();
                        selectedObject = null;
                    }
                }
            }
            else
            {
                Debug.Log("Raycast did NOT hit anything.");
            }
        }
    }

    public void Deselect()
    {
        if (selectedObject != null)
        {
            selectedObject.Deselect();
            selectedObject = null;
        }
    }
}