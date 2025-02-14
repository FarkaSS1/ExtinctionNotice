using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private EnemyRangedAttack rangedAttack;
    private void Start()
    {
        rangedAttack = GetComponent<EnemyRangedAttack>();
        if (rangedAttack == null)
        {
            Debug.LogError("EnemyRangedAttack component not found on " + gameObject.name);
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
    public void Shoot()
    {
        if (rangedAttack != null)
        {
            rangedAttack.ShootProjectile();
        }
        else
        {
            Debug.LogError("ShootProjectile() cannot be called because EnemyRangedAttack is missing.");
        }
    }
}

