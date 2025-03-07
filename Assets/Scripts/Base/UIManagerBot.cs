using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UIManagerBot : MonoBehaviour
{
    public Button buildTowerButton;
    public Button buildMineButton;
    public Button destroyTowerButton;
    public GameObject towerPrefab; // Prefab for the tower
    public GameObject minePrefab; // Prefab for the mine
    public GameStateManager gameStateManager; // Reference to GameStateManager
    public GameObject buyBackButton; // Reference to the buy back button
    private bool isBuildingTower = false;
    private GameObject blueprint; // The preview object
    private Renderer blueprintRenderer; // Store renderer for color changes
    private SelectableObject selectedObject;

    private int currentTowerCost;
    private string currentTowerCostType;

    public float minBuildDistance = 5f;  // Prevents building too close
    public float maxBuildDistance = 30f; // Ensures building stays within range
    public Transform centralHub; // Assign in Inspector

    private GameObject currentPrefabToBuild;

    // Event
    public static event Action<GameObject> OnBuildingPlaced;

    [Header("UI Elements")]
    public TMP_Text costTextTower;
    public TMP_Text costTextMine;
    public Image costIconTower;
    public Image costIconMine;
    public Sprite defaultIcon;
    public EventSystem eventSystem;


    public static UIManagerBot Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }

        Instance = this;
        Debug.Log("Instance set UIMANAGER");
    }

    private void Start()
    {
        InitializeBuildButtons();

        buildTowerButton.onClick.AddListener(() => HandleBuildTowerButtonClick());
        buildMineButton.onClick.AddListener(() => HandleBuildMineButtonClick());
        destroyTowerButton.onClick.AddListener(OnDestroyTowerButtonClick);
        HideBuyBackButton();
    }

    private void Update()
    {
        if (isBuildingTower)
        {
            UpdateBlueprintPosition();

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement()) // Left-click to place tower
            {
                TryPlaceTower(currentPrefabToBuild);
            }

            if (Input.GetMouseButtonDown(1)) // Right-click to cancel
            {
                CancelBuildingMode();
            }
        }
        CheckAndUpdateButtonCosts();
    }


    public void HandleBuildTowerButtonClick()
    {
        if (isBuildingTower)
        {
            CancelBuildingMode(); // Cancel the current building mode if a blueprint is already active
        }
        OnBuildButtonClick(towerPrefab);
    }

    public void HandleBuildMineButtonClick()
    {
        if (isBuildingTower)
        {
            CancelBuildingMode(); // Cancel the current building mode if a blueprint is already active
        }
        OnBuildButtonClick(minePrefab);
    }

    public void HandleSellButtonClick()
    {
        if (selectedObject != null)
        {
            OnDestroyTowerButtonClick();
        }
    }

    private void InitializeBuildButtons()
    {
        IconManager.InitializeIcons();

        // Setup Tower Button UI
        UpdateButtonUI(buildTowerButton, towerPrefab, costTextTower, costIconTower);

        // Setup Mine Button UI
        UpdateButtonUI(buildMineButton, minePrefab, costTextMine, costIconMine);
    }

    private bool IsPointerOverUIElement()
    {
        return eventSystem.IsPointerOverGameObject();
    }


    private void UpdateButtonUI(Button button, GameObject prefab, TMP_Text costText, Image costIcon)
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab for button {button.name} is NULL.");
            return;
        }

        SelectableObject objectData = prefab.GetComponentInChildren<SelectableObject>();
        if (objectData != null)
        {
            int cost = objectData.Cost;
            string costType = objectData.CostType;

            costText.text = cost.ToString();
            costIcon.sprite = IconManager.GetIcon(costType) ?? defaultIcon;
            costIcon.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError($"Prefab {prefab.name} is missing SelectableObject component.");
            costText.text = "N/A";
            costIcon.sprite = defaultIcon;
        }
    }

    private void CheckAndUpdateButtonCosts()
    {
        UpdateButtonUI(buildTowerButton, towerPrefab, costTextTower, costIconTower);
        UpdateButtonUI(buildMineButton, minePrefab, costTextMine, costIconMine);
    }

    private void OnBuildButtonClick(GameObject prefabToSpawn)
    {
        if (prefabToSpawn == null)
        {
            Debug.LogError("Prefab is NULL! Check the UIManager inspector reference.");
            return;
        }

        blueprint = Instantiate(prefabToSpawn);
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

        MakeBlueprintTransparent();

        // Ensure the object has a SelectableObject-derived component
        SelectableObject objectData = blueprint.GetComponentInChildren<SelectableObject>();
        if (objectData != null)
        {
            currentTowerCost = objectData.Cost;
            currentTowerCostType = objectData.CostType;
            Debug.Log($"Blueprint cost set: {currentTowerCost} {currentTowerCostType}");
        }
        else
        {
            Debug.LogError("Prefab is missing a SelectableObject-derived component! Using default values.");
            currentTowerCost = 0;
            currentTowerCostType = "";
        }

        currentPrefabToBuild = prefabToSpawn;
        isBuildingTower = true;
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

    private void UpdateBlueprintPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 fixedPosition = hit.point;
            fixedPosition.y = 0; // Adjust this value to match your game

            blueprint.transform.position = fixedPosition;
            blueprint.transform.rotation = Quaternion.identity; // Prevent unwanted rotation

            UpdateBlueprintColor();
        }
    }

    private void TryPlaceTower(GameObject buildingPrefab)
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
        if (IsOverlappingWithOtherBuildings())
        {
            Debug.LogWarning("Cannot place building here, it's overlapping with another building.");
            return;
        }
        if (gameStateManager.MaximumCapacityReached(buildingPrefab))
        {
            Debug.LogWarning("Building limit reached for this type!");
            return;
        }

        gameStateManager.RemoveResources(currentTowerCostType, currentTowerCost);
        gameStateManager.BuildStructure(buildingPrefab);

        // Instantiate the real tower at the blueprint position
        GameObject newBuilding = Instantiate(buildingPrefab, blueprint.transform.position, Quaternion.identity);

        SelectableObject towerComponent = newBuilding.GetComponentInChildren<SelectableObject>();
        if (towerComponent != null)
        {
            Debug.Log($"{towerComponent.name} placed successfully with cost: {towerComponent.Cost} {towerComponent.CostType}");
            OnBuildingPlaced?.Invoke(newBuilding);
        }
        else
        {
            Debug.LogError("Placed tower is missing SelectableObject component!");
        }

        // Activate the buildings functionality only after placement
        AttackTower placedTower = newBuilding.GetComponentInChildren<AttackTower>();
        if (placedTower != null)
        {
            placedTower.PlacedTower();
        }

        Mine placedQuarry = newBuilding.GetComponent<Mine>();
        if (placedQuarry != null)
        {
            placedQuarry.PlacedMine();
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

        bool isOverlapping = IsOverlappingWithOtherBuildings();

        Color newColor = (isAffordable && isInValidRange && !isOverlapping) ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
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
        if (isBuildingTower)
        {
            return;
        }
        if (blueprint != null)
        {
            Destroy(blueprint);
        }
        if (selectedObject != null)
        {
            int refundAmount = selectedObject.GetCost(); // Refund 100% of cost for now
            gameStateManager.AddResource(selectedObject.GetCostType(), refundAmount);
            gameStateManager.RemoveStructure(selectedObject.gameObject);
            Destroy(selectedObject.gameObject);
            selectedObject = null;
        }
    }

    public void SetSelectedObject(SelectableObject newSelection)
    {
        selectedObject = newSelection;
    }

    public void ShowBuyBackButton()
    {
        if (buyBackButton != null)
        {
            buyBackButton.SetActive(true);
        }
    }

    public void HideBuyBackButton()
    {
        if (buyBackButton != null)
        {
            buyBackButton.SetActive(false);
        }
    }

    private bool IsOverlappingWithOtherBuildings()
    {
        if (blueprint == null) return false;

        Collider[] blueprintColliders = blueprint.GetComponentsInChildren<Collider>();
        if (blueprintColliders.Length == 0)
        {
            Debug.LogWarning("Blueprint has no colliders! Skipping overlap check.");
            return false;
        }

        foreach (var blueprintCollider in blueprintColliders)
        {
            Vector3 position = blueprintCollider.bounds.center;
            Vector3 size = blueprintCollider.bounds.extents; // Use extents instead of size/2 for better accuracy

            Collider[] colliders = Physics.OverlapBox(position, size, Quaternion.identity);
            foreach (var col in colliders)
            {
                // Ignore collisions with the blueprint's own colliders
                if (Array.Exists(blueprintColliders, c => c == col)) continue;

                if (col.gameObject.CompareTag("Tower") || col.gameObject.CompareTag("Base"))
                {
                    return true; // Found an overlapping building
                }
            }
        }

        return false;
    }
}
