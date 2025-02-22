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

    public float minBuildDistance = 5f;  // Prevents building too close
    public float maxBuildDistance = 30f; // Ensures building stays within range
    public Transform centralHub; // Assign in Inspector

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

            if (Input.GetMouseButtonDown(0)) // Left-click to place tower
            {
                TryPlaceTower();
            }

            if (Input.GetMouseButtonDown(1)) // Right-click to cancel
            {
                CancelBuildingMode();
            }
        }
    }

    private void CancelBuildingMode()
    {
        Debug.Log("Building mode cancelled!");
        isBuildingTower = false;
        if (blueprint != null)
        {
            Destroy(blueprint);
        }
    }

    private void OnBuildTowerButtonClick()
{
    if (towerPrefab == null)
    {
        Debug.LogError("Tower prefab is NULL! Check the UIManager inspector reference.");
        return;
    }

    blueprint = Instantiate(towerPrefab);
    if (blueprint == null)
    {
        Debug.LogError("Instantiation failed! The blueprint is NULL.");
        return;
    }

    Debug.Log($"Blueprint instantiated successfully: {blueprint.name}");

    blueprintRenderer = blueprint.GetComponentInChildren<Renderer>();
    if (blueprintRenderer == null)
    {
        Debug.LogWarning("Blueprint Renderer not found! Trying to get from child objects.");
        blueprintRenderer = blueprint.GetComponentInChildren<Renderer>();
    }

    //MakeBlueprintTransparent();

    // Ensure the tower has a SelectableObject-derived component
    SelectableObject towerData = blueprint.GetComponentInChildren<SelectableObject>();
    if (towerData != null)
    {
        currentTowerCost = towerData.Cost;
        currentTowerCostType = towerData.CostType;
        Debug.Log($"Blueprint cost set: {currentTowerCost} {currentTowerCostType}");
    }
    else
    {
        Debug.LogError("Tower prefab is missing a SelectableObject-derived component! Using default values.");
        currentTowerCost = 0;
        currentTowerCostType = "";
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
            fixedPosition.y = 1.5f; // Adjust this value to match your game

            blueprint.transform.position = fixedPosition;
            blueprint.transform.rotation = Quaternion.identity; // Prevent unwanted rotation

            UpdateBlueprintColor();
        }
    }

    private void TryPlaceTower()
{
    if (blueprint == null) return;

    float distanceToHub = Vector3.Distance(blueprint.transform.position, centralHub.position);

    if (distanceToHub < minBuildDistance)
    {
        Debug.Log("Too close to CentralHub! Choose another spot.");
        return;
    }
    if (distanceToHub > maxBuildDistance)
    {
        Debug.Log("Too far from CentralHub! Choose another spot.");
        return;
    }

    if (string.IsNullOrEmpty(currentTowerCostType))
    {
        Debug.LogError("Invalid cost type detected! Cannot proceed with tower placement.");
        return;
    }

    if (!gameStateManager.CanAfford(currentTowerCostType, currentTowerCost))
    {
        Debug.LogWarning("Not enough resources to build this tower.");
        return;
    }

    gameStateManager.RemoveResources(currentTowerCostType, currentTowerCost);

    // Instantiate the real tower at the blueprint's position
    GameObject newTower = Instantiate(towerPrefab, blueprint.transform.position, Quaternion.identity);

    SelectableObject towerComponent = newTower.GetComponentInChildren<SelectableObject>();
    if (towerComponent != null)
    {
        Debug.Log($"Tower placed successfully with cost: {towerComponent.Cost} {towerComponent.CostType}");
    }
    else
    {
        Debug.LogError("Placed tower is missing SelectableObject component!");
    }

    isBuildingTower = false;
    Destroy(blueprint);
}

    private void UpdateBlueprintColor()
    {
        if (blueprint == null || blueprintRenderer == null) return;

        float distanceToHub = Vector3.Distance(blueprint.transform.position, centralHub.position);

        bool isAffordable = gameStateManager.CanAfford(currentTowerCostType, currentTowerCost);
        bool isInValidRange = (distanceToHub >= minBuildDistance && distanceToHub <= maxBuildDistance);

        Color newColor = (isAffordable && isInValidRange) ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
        blueprintRenderer.material.color = newColor;
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