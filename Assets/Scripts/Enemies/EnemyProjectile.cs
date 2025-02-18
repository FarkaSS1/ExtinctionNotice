using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
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

        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit the Player!");

            if (other.GetComponent<HealthSystem>())
            {
                Debug.Log("Applying damage to Player...");
                other.GetComponent<HealthSystem>().TakeDamage(damage);
            }
        }

        Debug.Log("Destroying projectile...");
        Destroy(gameObject); // Destroy spit on impact
    }


}
