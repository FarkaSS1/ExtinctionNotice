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

    private int currentTowerCost;
    private string currentTowerCostType;

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
        blueprint = Instantiate(towerPrefab);
        blueprintRenderer = blueprint.GetComponent<Renderer>();
        MakeBlueprintTransparent();

        // Debug the components attached to the instantiated blueprint
        foreach (var component in blueprint.GetComponents<MonoBehaviour>())
        {
            Debug.Log($"Blueprint has component: {component.GetType()}");
        }

        // Fetch cost from the actual tower class
        TowerOne towerData = blueprint.GetComponent<TowerOne>();
        if (towerData != null)
        {
            Debug.Log("Fetched TowerOne component successfully.");
            currentTowerCost = towerData.GetCost();
            currentTowerCostType = towerData.GetCostType();
            Debug.Log($"Tower cost: {currentTowerCost} {currentTowerCostType}");
        }
        else
        {
            Debug.LogError("Tower prefab is missing TowerOne component! Using default values.");
            currentTowerCost = 400; // Fallback
            currentTowerCostType = "elementX";
        }

        isBuildingTower = true;
    }



    private void UpdateBlueprintPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 fixedPosition = hit.point;

            //  Ensure the Y value is locked to terrain height
            fixedPosition.y = 1.5f; // Adjust this value to match your game

            blueprint.transform.position = fixedPosition;
            blueprint.transform.rotation = Quaternion.identity; // Prevent unwanted rotation

           

            UpdateBlueprintColor();
        }
    }



    private void TryPlaceTower()
    {
        if (gameStateManager.CanAfford(currentTowerCostType, currentTowerCost))
        {
            gameStateManager.RemoveResources(currentTowerCostType, currentTowerCost);

            // Instantiate the real tower and get the correct cost from the new instance
            GameObject newTower = Instantiate(towerPrefab, blueprint.transform.position, Quaternion.identity);
            TowerOne towerComponent = newTower.GetComponent<TowerOne>();

            if (towerComponent != null)
            {
                // Ensure correct cost values
                currentTowerCost = towerComponent.GetCost();
                currentTowerCostType = towerComponent.GetCostType();
            }
            else
            {
                Debug.LogError("Placed tower is missing TowerOne component!");
            }

            isBuildingTower = false;
            Destroy(blueprint);
        }
    }


    private void UpdateBlueprintColor()
    {
        if (gameStateManager.CanAfford(currentTowerCostType, currentTowerCost))
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
            int refundAmount = selectedObject.GetCost(); // Refund 100% of cost for now
            gameStateManager.AddResource(selectedObject.GetCostType(), refundAmount);
            Destroy(selectedObject.gameObject);
            selectedObject = null;
        }
    }

    public void SetSelectedObject(SelectableObject newSelection)
    {
        selectedObject = newSelection;
    }
}
