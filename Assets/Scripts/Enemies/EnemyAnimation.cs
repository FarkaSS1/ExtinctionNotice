using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private EnemyMeleeAttack meleeAttack;
    private EnemyRangedAttack rangedAttack;

    private void Start()
    {
        rangedAttack = GetComponent<EnemyRangedAttack>();

        if (rangedAttack == null)
        {
            meleeAttack = GetComponent<EnemyMeleeAttack>();

            if (meleeAttack == null)
            {
                Debug.LogError("No valid attack component found on " + gameObject.name);
            }
        }
    }

    public void PlayStep()
    {
        //Debug.Log("PlayStep event triggered, but no action set.");
    }
    public void Grunt() { 
        //Debug.Log("Grunt event triggered, but no action set.");
    }

    public void AttackBegin()
    {
        //Debug.Log("AttackBegin event triggered, but no action set.");
    }
    public void ChomperRunForward()
    {
        //Debug.Log("AttackBegin event triggered, but no action set.");
    }
    public void Spit()
    {
        if (rangedAttack != null)
        {
            rangedAttack.TriggerShoot(); 
        }
        else
        {
            Debug.LogError("ShootProjectile() cannot be called because EnemyRangedAttack is missing.");
        }
    }

    public void Bite()
    {
        if (meleeAttack != null)
        {
            EnemyAI enemyAI = GetComponentInParent<EnemyAI>();
            if (enemyAI != null)
            {
                Transform target = enemyAI.GetCurrentTarget();
                if (target != null)
                {
                    meleeAttack.ApplyMeleeDamage(target);
                }
                else
                {
                    Debug.LogError("Bite() failed: No valid target found for ApplyMeleeDamage().");
                }
            }
            else
            {
                Debug.LogError("Bite() failed: EnemyAI component not found.");
            }
        }
        else
        {
            Debug.LogError("ApplyMeleeDamage() cannot be called because EnemyMeleeAttack is missing.");
        }
    }

}

