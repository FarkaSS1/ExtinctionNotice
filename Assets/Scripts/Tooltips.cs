using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class Tooltips : MonoBehaviour
{
    public CanvasGroup tooltipCanvas;
    public RawImage rawImageLocateBase;
    public RawImage rawImagePlaceBuildings;
    private CameraModeManager cameraModeManager;
    private UIManagerBot UIManagerBot;
    private WelcomeScreen welcomeScreen;
    private Hive hiveObject;

    [Header("General Tooltip Conditions")]
    private bool gameStarted = false;

    [Header("Place Building Tooltip Conditions")]
    private bool towerPlaced = false;
    private bool minePlaced = false;

    [Header("Locate Hive Tooltip Conditions")]
    private bool bossSpawned = false;


    void Update()
    {
        if (minePlaced) { CompleteToolTipPlaceBuildings(); };
        if (bossSpawned) { CompleteToolTipLocateHive(); };
    }

    private void CompleteToolTipLocateHive()
    {
        cameraModeManager.OnPlayerModeActivated.RemoveListener(OnPlayerModeActivated);
        hiveObject.OnBossSpawned.RemoveListener(OnBossSpawned);

        rawImageLocateBase.gameObject.SetActive(false);
    }

    private void CompleteToolTipPlaceBuildings()
    {
        cameraModeManager.OnBaseViewModeActivated.RemoveListener(OnBaseViewModeActivated);
        UIManagerBot.OnBuildingPlaced -= OnBuildingPlaced;
        
        rawImagePlaceBuildings.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        FindCameraModeManager();
        FindUIManagerBot();
        FindWelcomeScreen();
        FindHiveObject();

        SubscribeToEvents();

        rawImageLocateBase.gameObject.SetActive(false);
        rawImagePlaceBuildings.gameObject.SetActive(false);
    }

    private void SubscribeToEvents()
    {
        cameraModeManager.OnPlayerModeActivated.AddListener(OnPlayerModeActivated);
        cameraModeManager.OnBaseViewModeActivated.AddListener(OnBaseViewModeActivated);
        UIManagerBot.OnBuildingPlaced += OnBuildingPlaced;
        welcomeScreen.OnGameStarted.AddListener(OnGameStarted);
        hiveObject.OnBossSpawned.AddListener(OnBossSpawned);
    }

    private void OnBossSpawned()
    {
        bossSpawned = true;
    }

    private void OnGameStarted()
    {
        gameStarted = true;
        welcomeScreen.OnGameStarted.RemoveListener(OnGameStarted);
    }

    private void OnBuildingPlaced(GameObject @object)
    {
        if (@object.name == "turret_1_2") { towerPlaced = true; }
        ;
        if (@object.name == "MinerElementX(Clone)") { minePlaced = true; }
        ;
    }

    private void OnBaseViewModeActivated()
    {
        if (gameStarted) { rawImagePlaceBuildings.gameObject.SetActive(true); }
    }

    private void OnPlayerModeActivated()
    {
        if (gameStarted) { rawImageLocateBase.gameObject.SetActive(true); }
    }

    private void FindUIManagerBot()
    {
        UIManagerBot = FindObjectOfType<UIManagerBot>();
        if (!UIManagerBot)
        {
            Debug.LogError("Tooltips.cs: UIManagerBot object not found.");
        }
    }

    private void FindCameraModeManager()
    {
        cameraModeManager = FindObjectOfType<CameraModeManager>();
        if (!cameraModeManager)
        {
            Debug.LogError("Tooltips.cs: CameraMode Manager not found.");
        }
    }

    private void FindWelcomeScreen()
    {
        welcomeScreen = FindObjectOfType<WelcomeScreen>();
        if (!welcomeScreen)
        {
            Debug.LogError("Tooltips.cs: WelcomeScreen object not found.");
        }
    }

    private void FindHiveObject()
    {
        hiveObject = FindObjectOfType<Hive>();
        if (!hiveObject)
        {
            Debug.LogError("Tooltips.cs: Hive Object not found.");
        }
    }
}