using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    private Vector2 input;
    public float moveForce = 30.0f;
    private SpriteRenderer _spriteRenderer;
    public AudioClip victoryClip;
    public AudioClip deathClip;
    private AudioSource audioSource;
    private bool isDying = false; // New flag to track dying state
    private bool isNeutral = false;
    private Color originalColor;
    private Coroutine neutralStateCoroutine;
    public BackgroundMusicController bkrd;
    
    [SerializeField] private AudioManager audioManager;

    public void ActivateNeutralState(float duration)
    {
        // Cancel any existing neutral state
        if (neutralStateCoroutine != null)
            StopCoroutine(neutralStateCoroutine);

        // Start new neutral state
        neutralStateCoroutine = StartCoroutine(NeutralStateRoutine(duration));
    }

    private IEnumerator NeutralStateRoutine(float duration)
    {
        // save original color and set neutral state
        isNeutral = true;
        originalColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.green;

        Debug.Log("Neutral state activated for " + duration + " seconds");

        // Wait for duration
        yield return new WaitForSeconds(duration);

        // Return to normal
        isNeutral = false;
        _spriteRenderer.color = originalColor;

        Debug.Log("Neutral state deactivated");
    }

    // Add this public method so other scripts can check if player is neutral
    public bool IsInNeutralState()
    {
        return isNeutral;
    }
    void Start()
    {
        // sprite renderer component to access color - default is RED
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.red;

        // rigid body component to access velocity
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = 2f;

        // audio source component to access victory noise
        audioSource = GetComponent<AudioSource>();

        bkrd = FindFirstObjectByType<BackgroundMusicController>();
    }

    void Update()
    {
        // Skip input processing if player is dying
        if (isDying)
            return;

        // movement
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input.Normalize();

        // on spacebar click
        // on spacebar click - only allow color change if not in neutral state
        if (Input.GetKeyDown(KeyCode.Space) && !isNeutral)
        {
            // change color
            if (_spriteRenderer.color == Color.red) _spriteRenderer.color = Color.blue;
            else if (_spriteRenderer.color == Color.blue) _spriteRenderer.color = Color.red;
            // see if you can pass through blue wall
            UpdateWallCollision();
            audioManager.Swap();	
        }

        // on R click
        if (Input.GetKeyDown(KeyCode.R))
        {
            // reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        // skip movement if player is dying
        if (isDying)
        {
            // immediately stop all movement
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // player isnt dying! handle movement now
        HandleMovement();
    }

    void HandleMovement() {
        // apply movement force (if player trying to move)
        if (input.sqrMagnitude > 0.01f)
        {
            rb.AddForce(input * moveForce);
        }
    }

    void UpdateWallCollision()
    {
        GameObject[] blueWalls = GameObject.FindGameObjectsWithTag("BluePassWall");
        GameObject[] redWalls = GameObject.FindGameObjectsWithTag("RedPassWall");

        foreach (GameObject wall in blueWalls)
        {
            Collider2D wallCollider = wall.GetComponent<Collider2D>();
            if (wallCollider != null)
            {
                wallCollider.enabled = (_spriteRenderer.color == Color.red); // Blue walls are solid if player is red
            }
        }

        foreach (GameObject wall in redWalls)
        {
            Collider2D wallCollider = wall.GetComponent<Collider2D>();
            if (wallCollider != null)
            {
                wallCollider.enabled = (_spriteRenderer.color == Color.blue); // Red walls are solid if player is blue
            }
        }
    }

    // collision activation
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"Collided with: {other.gameObject.name}");

        // hit enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit!");
            CameraScript.Instance.ShakeCamera(0.3f, 0.2f);
            StartCoroutine(DieWithDelay());
        }
    }

    private IEnumerator DieWithDelay()
    {
        Debug.Log("DYING...");

        // Set dying flag to true to stop movement
        isDying = true;

        // Immediately stop movement
        rb.linearVelocity = Vector2.zero;

        // Play death sound
        audioManager.Death();

        yield return new WaitForSeconds(1.0f);

        // next line after delay
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartFlashingAndRestart()
    {
        audioManager.Win();
        StartCoroutine(FlashGreenAndRestart());
    }

    private IEnumerator FlashGreenAndRestart()
    {
        // Set dying flag to true to stop movement during victory animation
        isDying = true;

        // Immediately stop movement
        rb.linearVelocity = Vector2.zero;

        for (int i = 0; i < 6; i++) // Flash 6 times (2 seconds)
        {
            _spriteRenderer.color = Color.green;
            yield return new WaitForSeconds(0.2f);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }

        bkrd.isPlaying = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}