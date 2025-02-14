using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Button buildTowerButton;
    public Button destroyTowerButton;
    public GameObject towerPrefab; // Prefab for the tower
    public GameStateManager gameStateManager; // Reference to GameStateManager

    private bool isBuildingTower = false;
    private GameObject blueprint; // The preview object
    private Renderer blueprintRenderer; // Store renderer for color changes
    private SelectableObject selectedObject;

    // Placeholder cost, ideally this should be stored per tower type
    int towerCost = 500;
    string towerTypeCost = "elementX";

    private void Start()
    {
        buildTowerButton.onClick.AddListener(OnBuildTowerButtonClick);
        destroyTowerButton.onClick.AddListener(OnDestroyTowerButtonClick);
    }

    private void Update()
    {
        if (isBuildingTower)
        {
            UpdateBlueprintPosition();

            if (Input.GetMouseButtonDown(0)) // Left click to place tower
            {
                TryPlaceTower();
            }
        }
    }

    private void OnBuildTowerButtonClick()
    {
        if (blueprint == null)
        {
            blueprint = Instantiate(towerPrefab); // Create preview object
            blueprintRenderer = blueprint.GetComponent<Renderer>();
            MakeBlueprintTransparent();
        }
        isBuildingTower = true;
    }

    private void UpdateBlueprintPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            blueprint.transform.position = hit.point; // Move blueprint to cursor position
            UpdateBlueprintColor();
        }
    }

    private void TryPlaceTower()
    {

        if (gameStateManager.CanAfford(towerTypeCost,towerCost))
        {
            gameStateManager.RemoveResources(towerTypeCost, towerCost);
            Instantiate(towerPrefab, blueprint.transform.position, Quaternion.identity);
            isBuildingTower = false;
            Destroy(blueprint);
        }
    }

    private void UpdateBlueprintColor()
    {
        if (gameStateManager.CanAfford(towerTypeCost, towerCost))
        {
            blueprintRenderer.material.color = new Color(0, 1, 0, 0.5f); // Green if affordable
        }
        else
        {
            blueprintRenderer.material.color = new Color(1, 0, 0, 0.5f); // Red if not affordable
        }
    }

    private void MakeBlueprintTransparent()
    {
        if (blueprintRenderer != null)
        {
            Color transparentColor = blueprintRenderer.material.color;
            transparentColor.a = 0.5f; // Make it semi-transparent
            blueprintRenderer.material.color = transparentColor;
        }
    }

    private void OnDestroyTowerButtonClick()
    {
        if (selectedObject != null)
        {
            int refundAmount = selectedObject.cost; // Refund 100% of cost for now
            gameStateManager.AddResource(towerTypeCost, refundAmount);
            Destroy(selectedObject.gameObject);
            selectedObject = null;
        }
    }

    public void SetSelectedObject(SelectableObject newSelection)
    {
        selectedObject = newSelection;
    }
}
