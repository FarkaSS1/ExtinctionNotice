using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 25f;
    public float damage = 15f;
    public float lifetime = 5f; // Destroy after 5s

    private Vector3 moveDirection;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime; // Moves in a straight line
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Projectile hit: " + other.gameObject.name);

        if (other.TryGetComponent<IHealth>(out IHealth health))
        {
            Debug.Log("Projectile hit " + other.name + "! Applying " + damage + " damage...");
            health.TakeDamage(damage, transform);
        }
        else
        {
            Debug.Log("Projectile hit " + other.name + " but it has no IHealth component.");
        }


        Debug.Log("Destroying projectile...");
        Destroy(gameObject); // Destroy spit on impact
    }


}
