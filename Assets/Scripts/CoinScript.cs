using UnityEngine;
public class CoinScript : MonoBehaviour
{
    public float attractionSpeed = 2.4f; // Speed of attraction/repulsion
    public float detectionRange = 3.5f; // Distance where the effect is applied
    private Transform player;
    private SpriteRenderer playerSprite;
    private PlayerScript playerScript; // Add this line to declare the variable
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool isRedCoin;
    private bool collected = false;

    [SerializeField] private AudioManager audioManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSprite = player.GetComponent<SpriteRenderer>();
        playerScript = player.GetComponent<PlayerScript>(); // Initialize it here
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        // Check if this coin is Red or Blue
        isRedCoin = gameObject.CompareTag("RedCoin");
    }

    void FixedUpdate()
    {
        if (collected || player == null)
        {
            return;
        }

        // calculate distance to player
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > detectionRange) return; // only react if within range

        // Check if player is in neutral state
        if (playerScript != null && playerScript.IsInNeutralState())
        {
            // Always move toward player
            Vector2 direction1 = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction1 * attractionSpeed * Time.fixedDeltaTime);
            return;
        }

        // Regular color-based behavior
        bool playerIsRed = (playerSprite.color == Color.red);
        bool shouldMoveTowardPlayer = (playerIsRed && !isRedCoin) || (!playerIsRed && isRedCoin);

        // calculate direction
        Vector2 direction = (player.position - transform.position).normalized;
        if (!shouldMoveTowardPlayer) direction *= -1; // reverse if moving away

        // check if path clear using RayCast
        RaycastHit2D hitWall = Physics2D.Raycast(transform.position, direction, 0.5f, LayerMask.GetMask("Wall"));
        RaycastHit2D hitBox = Physics2D.Raycast(transform.position, direction, 0.5f, LayerMask.GetMask("Breakable"));

        if (hitWall.collider == null && hitBox.collider == null) // if nothing is hit, move the coin
        {
            rb.MovePosition(rb.position + direction * attractionSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WinGround"))
        {
            Debug.Log("Coin is inside win area!");
            collected = true; // Mark as collected to prevent multiple triggers
            rb.linearVelocity = Vector2.zero; // Stop moving
           
            audioManager.CoinCollect();	

            // notify GameManager
            GameManager.instance.CoinCollected(); // Notify GameManager
            // Disable sprite and collider but keep the object until sound finishes
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            // Destroy after sound finishes
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}