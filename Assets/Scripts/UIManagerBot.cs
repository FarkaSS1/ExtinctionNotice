using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button buildTowerButton;
    public Button destroyTowerButton;
    public GameObject towerPrefab; // Prefab for the tower
    //public GameObject SelectionManager; // Reference to SelectionManager

    private bool isBuildingTower = false;
    private SelectableObject selectedObject;

    private void Start()
    {
        buildTowerButton.onClick.AddListener(OnBuildTowerButtonClick);
        destroyTowerButton.onClick.AddListener(OnDestroyTowerButtonClick);
    }

    private void Update()
    {
        if (isBuildingTower)
        {
            if (Input.GetMouseButtonDown(0)) // Left click to place tower
            {
                PlaceTowerAtMousePosition();
            }
        }
    }

    // Called when the "Build Tower" button is clicked
    private void OnBuildTowerButtonClick()
    {
        isBuildingTower = true; // Activate building mode
    }

    // Called when the "Destroy Tower" button is clicked
    private void OnDestroyTowerButtonClick()
    {
        //SetSelectedObject();
        if (selectedObject != null)
        {
            Destroy(selectedObject.gameObject);
            selectedObject = null; // Deselect after destruction
        }
    }

    private void PlaceTowerAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject newTower = Instantiate(towerPrefab, hit.point, Quaternion.identity); // Create the tower

            // Ensure the tower has a SelectableObject component
            if (!newTower.GetComponent<SelectableObject>())
            {
                newTower.AddComponent<SelectableObject>();
            }

            isBuildingTower = false; // Exit build mode
        }
    }

    public void SetSelectedObject(SelectableObject newSelection)
    {
        selectedObject = newSelection;
    }
}
