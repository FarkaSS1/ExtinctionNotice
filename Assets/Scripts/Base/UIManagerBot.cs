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
        blueprintRenderer = blueprint.GetComponent<Renderer>();
        MakeBlueprintTransparent();

        if (blueprint == null)
        {
            Debug.LogError("Instantiation failed! The blueprint is NULL.");
            return;
        }

        Debug.Log($"Blueprint instantiated successfully: {blueprint.name}");

        if (blueprint.GetComponent<BasicTower>() == null)
        {
            blueprint.AddComponent<BasicTower>();
            Debug.LogWarning("BasicTower was missing and has been added.");
        }

        // Debug the components attached to the instantiated blueprint
        foreach (var component in blueprint.GetComponents<MonoBehaviour>())
        {
            Debug.Log($"Blueprint has component: {component.GetType()}");
        }

        MonoBehaviour[] components = blueprint.GetComponents<MonoBehaviour>();

        if (components.Length == 0)
        {
            Debug.LogError("Blueprint has NO MonoBehaviour components! Check if scripts are attached to the prefab.");
        }
        else
        {
            foreach (var component in components)
            {
                Debug.Log($"Blueprint has component: {component.GetType()}");
            }
        }


        // Fetch cost from the actual tower class
        SelectableObject towerData = blueprint.GetComponent<SelectableObject>();
        if (towerData != null)
        {
            Debug.Log("Fetched SelectableObject successfully.");
            currentTowerCost = towerData.GetCost();
            currentTowerCostType = towerData.GetCostType();
            Debug.Log($"Tower cost: {currentTowerCost} {currentTowerCostType}");
        }
        else
        {
            Debug.LogError("Tower prefab is missing a SelectableObject-derived component! Using default values.");
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
            // THIS CAN BE REMOVED IF WE FIX THE SCALING ISSUE IN THE PREFAB
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

        // Check if within allowed range
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
        Debug.Log($"Checking affordability for {currentTowerCostType} with cost {currentTowerCost}");
        if (string.IsNullOrEmpty(currentTowerCostType))
        {
            Debug.LogError("Invalid cost type detected! Make sure the tower prefab has a valid costType.");
        }


        // Check if player has enough resources
        if (gameStateManager.CanAfford(currentTowerCostType, currentTowerCost))
        {
            gameStateManager.RemoveResources(currentTowerCostType, currentTowerCost);

            // Instantiate real tower
            GameObject newTower = Instantiate(towerPrefab, blueprint.transform.position, Quaternion.identity);

            SelectableObject towerComponent = newTower.GetComponent<SelectableObject>();
            if (towerComponent != null)
            {
                currentTowerCost = towerComponent.GetCost();
                currentTowerCostType = towerComponent.GetCostType();
            }
            else
            {
                Debug.LogError("Placed tower is missing Tower component!");
            }

            isBuildingTower = false;
            Destroy(blueprint);
        }
    }


    private void UpdateBlueprintColor()
    {
        if (blueprint == null) return;

        float distanceToHub = Vector3.Distance(blueprint.transform.position, centralHub.position);

        bool isAffordable = gameStateManager.CanAfford(currentTowerCostType, currentTowerCost);
        bool isInValidRange = (distanceToHub >= minBuildDistance && distanceToHub <= maxBuildDistance);

        if (isAffordable && isInValidRange)
        {
            blueprintRenderer.material.color = new Color(0, 1, 0, 0.5f); // Green if valid
        }
        else
        {
            blueprintRenderer.material.color = new Color(1, 0, 0, 0.5f); // Red if invalid
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
