using UnityEngine;

public class AmariCoinScript : MonoBehaviour
{
    public float attractionSpeed = 2.4f; // Speed of attraction/repulsion
    public float detectionRange = 3.5f; // Distance where the effect is applied
    private Transform player;
    private SpriteRenderer playerSprite;
    private Rigidbody2D rb;
    public  Rigidbody2D playerRb;
    private AudioSource audioSource;
    private bool isRedCoin;
    // private bool collected = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSprite = player.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // Check if this coin is Red or Blue
        isRedCoin = gameObject.CompareTag("RedCoin");
    }

    void Update()
    {
        // if (collected || player == null) {
        //     return;
        // }

        // calculate distance to player
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > detectionRange) return; // only react if within range

        bool playerIsRed = (playerSprite.color == Color.red);
        bool shouldMoveTowardPlayer = (playerIsRed && !isRedCoin) || (!playerIsRed && isRedCoin);

        // calculate direction
        Vector2 direction = (player.position - transform.position).normalized;
        if (shouldMoveTowardPlayer) direction *= -1; // reverse if moving away

        if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("Hello!");
            if (playerIsRed == true) 
            {
                playerRb.MovePosition(playerRb.position + direction * attractionSpeed * Time.fixedDeltaTime); 
            }
            if (playerIsRed == false) 
            {
                playerRb.MovePosition(playerRb.position + direction * attractionSpeed * Time.fixedDeltaTime); 
            }
        }        
        
        // check if path clear using RayCast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, LayerMask.GetMask("Wall"));
        if (hit.collider == null) // if no wall is hit, move the coin
        {           

            
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("WinGround"))
    //     {
    //         Debug.Log("Coin is inside win area!");
    //         collected = true; // Mark as collected to prevent multiple triggers
    //         rb.linearVelocity = Vector2.zero; // Stop moving

    //         if (audioSource != null)
    //         {
    //             audioSource.Play(); // Play sound effect
    //         }

    //         // notify GameManager
    //         GameManager.instance.CoinCollected(); // Notify GameManager

    //         // Disable sprite and collider but keep the object until sound finishes
    //         GetComponent<SpriteRenderer>().enabled = false;
    //         GetComponent<Collider2D>().enabled = false;

    //         // Destroy after sound finishes
    //         Destroy(gameObject, audioSource.clip.length);
    //     }
}