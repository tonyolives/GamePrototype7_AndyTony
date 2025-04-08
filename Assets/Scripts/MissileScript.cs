using UnityEngine;

public class MissileScript : MonoBehaviour
{
    public float baseSpeed = 3f; // missile's base forward speed
    public float explosionRadius = 2.5f; // radius for explosion damage
    public float lifetime = 4f; // time before missile self-destructs
    public float forcePower = 7f; // max attractive / repelling force
    public float influenceRange = 3f; // distance at which polarity force applies
    public LayerMask destructibleLayer; // layer mask for destructible objects

    private Transform player;
    private SpriteRenderer playerSprite;
    private Rigidbody2D rb;
    private bool isRedMissile;
    private Vector3 initialDirection;
    private Vector2 currentVelocity;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSprite = player.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // check color of missile
        isRedMissile = gameObject.CompareTag("RedMissile");

        // store launch direction (based on spawn rotation)
        initialDirection = transform.up;

        // set initial velocity
        currentVelocity = transform.up * baseSpeed;

        // detonate after set time
        Invoke("Explode", lifetime);
    }

    void FixedUpdate()
    {
        if (!player) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > influenceRange)
        {
            rb.linearVelocity = currentVelocity;
            return;
        }

        // determine polarity interaction
        Vector2 direction = (player.position - transform.position).normalized;
        bool playerIsRed = playerSprite.color == Color.red;
        bool shouldAttract = (playerIsRed && !isRedMissile) || (!playerIsRed && isRedMissile);

        if (!shouldAttract) direction *= -1;

        // closer = stronger force
        float strength = Mathf.Lerp(forcePower * 0.6f, forcePower, 1f - (distance / influenceRange));
        currentVelocity = direction * strength;
        rb.linearVelocity = currentVelocity;
    }

    void Explode()
    {
        // damage / destroy destructible environment objects in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, destructibleLayer);
        foreach (var hit in hits)
        {
            Destroy(hit.gameObject);
        }

        Destroy(gameObject); // destroy self
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius); // explosion radius

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, influenceRange); // influence radius
    }
}
