using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{

    protected override void Awake() {
        base.Awake(); // Initialize health from base class
    }


    protected override void Die() {
        base.Die();
    }

}
