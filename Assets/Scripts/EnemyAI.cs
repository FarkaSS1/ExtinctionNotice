using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Transform baseTarget;
    private Transform player;
    public float playerDetectionRange = 5f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Find the base using the "Base" tag
        GameObject baseObject = GameObject.FindWithTag("Base");
        if (baseObject != null)
        {
            baseTarget = baseObject.transform;
            Debug.Log("Base found: " + baseTarget.name);
        }
        else
        {
            Debug.LogError("Base not found! Make sure it has the 'Base' tag.");
        }

        // Find the player using the "Player" tag
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            Debug.Log("Player found: " + player.name);
        }
        else
        {
            Debug.LogError("Player not found! Make sure it has the 'Player' tag.");
        }

        // Set initial destination to base
        if (baseTarget != null)
        {
            agent.SetDestination(baseTarget.position);
            Debug.Log("Enemy moving toward base: " + baseTarget.position);
        }
    }

    void Update()
    {
        if (baseTarget == null || agent == null) return;

        float distanceToPlayer = player != null ? Vector3.Distance(transform.position, player.position) : Mathf.Infinity;

        // If player is close, switch target to player
        if (distanceToPlayer <= playerDetectionRange)
        {
            agent.SetDestination(player.position);
            Debug.Log("Enemy detected player! Moving toward player at " + player.position);
        }
        else
        {
            agent.SetDestination(baseTarget.position);
            Debug.Log("Enemy moving toward base...");
        }
        float moveSpeed = agent.velocity.magnitude;
        animator.SetFloat("Speed", moveSpeed);
        Debug.Log("AI Speed: " + moveSpeed);
    }
}
