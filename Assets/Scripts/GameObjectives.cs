using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class GameObjectives : MonoBehaviour
{
    /*
    1. Find necessary objects to hook OnEvents(OnEventDothis) with methods ()
    2. Planning: Gameobjectives for Weapon Upgrade:
        - Eliminate 10 enemies
        - Accumulate a total of 2100 resources 
        - Place 5 towers
    3. When completed Weapon Upgrade drops in form of supply drop near the base. 
    4. Press "Q" to view quests
    */
    [Header("Connected in Unity Inspector:")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RawImage imageBackground;
    [SerializeField] private TextMeshProUGUI textEnemiesDefeated;
    [SerializeField] private TextMeshProUGUI textTowersPlaced;
    [SerializeField] private TextMeshProUGUI textAmountElementX;

    [Header("Conditions Weapon Upgrade Quest")]
    private int nEnemiesDefeated = 0;
    private int nTowersPlaced = 0;
    private int nElementXAccumulated = 0;

    [Header("Objects (find these in Find function to get events etc)")]
    private EnemyHealth enemyHealthObject;
    private UIManagerBot UIManagerBotObject;
    private GameStateManager gameStateManagerObject;
    private EnemySpawner enemySpawnerObject;
    private Pistol pistolObject;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(FadeIn());
            StartCoroutine(DelayAndFadeOut(3)); // 3 seconds pause before fade-out
        }

        if (nEnemiesDefeated >= 10 && nTowersPlaced >= 5 && nElementXAccumulated >= 2100)
        {
            CompleteQuestWeaponUpgrade();
        }
    }

    private void OnEnable()
    {
        DisableUIComponents();

        FindEnemyHealthObject();
        FindEnemyUIManagerBotObject();
        FindEnemyGameStateManagerObject();
        FindEnemySpawnerObject();
        FindPistolObject();

        enemyHealthObject.OnEnemyDie += OnEnemyDeath;
        UIManagerBot.OnBuildingPlaced += OnTowerPlaced;
        gameStateManagerObject.OnElementXUpdated += OnElementXUpdated;
        enemySpawnerObject.OnEnemySpawned += OnEnemySpawned;
    }



    private void CompleteQuestWeaponUpgrade()
    {
        UpgradePistol upgradePistol = pistolObject.AddComponent<UpgradePistol>();

        upgradePistol.UpgradeWeapon();

        // Unsubscribe from listeners
        enemyHealthObject.OnEnemyDie -= OnEnemyDeath;
        UIManagerBot.OnBuildingPlaced -= OnTowerPlaced;
        gameStateManagerObject.OnElementXUpdated -= OnElementXUpdated;

        Debug.Log("Weapon Pistol Quest Completed");
        Destroy(this);
    }


    // Event Handlers:
    private void OnEnemySpawned(EnemyHealth newEnemy)
    {
        if (newEnemy != null)
        {
            newEnemy.OnEnemyDie += OnEnemyDeath;
        }
    }

    private void OnEnemyDeath()
    {
        nEnemiesDefeated++;
        textEnemiesDefeated.text = $"{nEnemiesDefeated}/10 Enemies Defeated";
    }

    private void OnTowerPlaced(GameObject @object)
    {
        // Can reference @object to get name of the building placed
        nTowersPlaced++;
        textTowersPlaced.text = $"{nTowersPlaced}/5 Towers Placed";
    }

    private void OnElementXUpdated(int nElementX)
    {
        nElementXAccumulated = nElementX;
        textAmountElementX.text = $"{nElementXAccumulated}/2100 ElementX Gathered";
    }

    // Helper methods:
    IEnumerator FadeIn()
    {
        float fadeDuration = 1f;
        float time = 0f;
        EnableUIComponents();

        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.unscaledDeltaTime; // Use unscaled delta time for fade duration
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    IEnumerator DelayAndFadeOut(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(FadeOut());  
    }

    IEnumerator FadeOut()
    {
        float fadeDuration = 1f; // Duration of the fade out
        float time = 0f;

        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            time += Time.unscaledDeltaTime; // Use unscaled delta time for fade duration
            yield return null;
        }

        canvasGroup.alpha = 0f;  // Ensure it ends with full transparency
    }

    private void FindEnemyHealthObject()
    {
        enemyHealthObject = FindObjectOfType<EnemyHealth>();
        if (!enemyHealthObject) { Debug.LogError("Gameobjectives.cs: enemy health object not found"); }
    }
    private void FindEnemyUIManagerBotObject()
    {
        UIManagerBotObject = FindObjectOfType<UIManagerBot>();
        if (!UIManagerBotObject) { Debug.LogError("Gameobjectives.cs: UIManagerBotObject not found"); }
    }
    private void FindEnemyGameStateManagerObject()
    {
        gameStateManagerObject = FindObjectOfType<GameStateManager>();
        if (!gameStateManagerObject) { Debug.LogError("Gameobjectives.cs: gameStateManagerObject not found"); }
    }

    private void FindEnemySpawnerObject()
    {
        enemySpawnerObject = FindObjectOfType<EnemySpawner>();
        if (!enemySpawnerObject) { Debug.LogError("Gameobjectives.cs: enemySpawnerObject not found"); }
    }

    private void FindPistolObject()
    {
        pistolObject = FindObjectOfType<Pistol>();
        if (!pistolObject) { Debug.LogError("Gameobjectives.cs: Object with Pistol script attached not found"); }
    }

    private void DisableUIComponents()
    {
        imageBackground.gameObject.SetActive(false);
        textEnemiesDefeated.gameObject.SetActive(false);
        textTowersPlaced.gameObject.SetActive(false);
        textAmountElementX.gameObject.SetActive(false);
    }

    private void EnableUIComponents()
    {
        imageBackground.gameObject.SetActive(true);
        textEnemiesDefeated.gameObject.SetActive(true);
        textTowersPlaced.gameObject.SetActive(true);
        textAmountElementX.gameObject.SetActive(true);
    }
}