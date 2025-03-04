using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

class BossChomperHealth : EnemyHealth
{
    public AudioClip deathSound;
    public AudioClip deathSoundGrowl;
    private AudioSource audioSource;
    private EnemyAI movement;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        movement = GetComponentInParent<EnemyAI>();
        if (!movement) { movement = GetComponentInChildren<EnemyAI>(); }
    }

    protected override void Die()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
            audioSource.PlayOneShot(deathSoundGrowl);
        }

        FreezeBoss();
        StartCoroutine(LoadSceneAfterDelay(3f)); // Wait before loading the scene
    }

    private void FreezeBoss()
    {
        if (movement != null)
        {
            movement.enabled = false; // Disable movement script
        }

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = 0; // Freeze animations
        }
    }


    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        Debug.Log("Changing Scene");
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("VictoryScene");
    }
}
