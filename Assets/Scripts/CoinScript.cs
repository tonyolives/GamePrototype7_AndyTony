using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public float attractionSpeed = 2.4f; // Speed of attraction/repulsion
    public float detectionRange = 1.8f; // Distance where the effect is applied
    private Transform player;
    private SpriteRenderer playerSprite;
    private Rigidbody2D rb;
    private bool isRedCoin;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSprite = player.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Check if this coin is Red or Blue
        isRedCoin = gameObject.CompareTag("RedCoin");
    }

    void FixedUpdate()
    {
        if (player == null) {
            Debug.Log("Player is missing!");
            return;
        }

        // calculate distance to player
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > detectionRange) return; // only react if within range

        bool playerIsRed = (playerSprite.color == Color.red);
        bool shouldMoveTowardPlayer = (playerIsRed && !isRedCoin) || (!playerIsRed && isRedCoin);

        // calculate direction
        Vector2 direction = (player.position - transform.position).normalized;
        if (!shouldMoveTowardPlayer) direction *= -1; // reverse if moving away

        // check if path clear using RayCast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, LayerMask.GetMask("Wall"));
        if (hit.collider == null) // if no wall is hit, move the coin
        {
            rb.MovePosition(rb.position + direction * attractionSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WinGround"))
        {
            Debug.Log("Coin is inside win area!");
        }
    }
}
