using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    private Vector2 input;
    float moveLimiter = 0.7f;
    public float runSpeed = 20.0f;
    public float moveForce = 13.0f;
    public float friction = -10.0f;
    private SpriteRenderer _spriteRenderer;
    public AudioClip victoryClip;
    public AudioClip deathClip;
    private AudioSource audioSource;
    private bool isDying = false; // New flag to track dying state
    private bool isNeutral = false;
    private Color originalColor;
    private Coroutine neutralStateCoroutine;

    // Add this method to your PlayerScript
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
        // Save original color and set neutral state
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

        // audio source component to access victory noise
        audioSource = GetComponent<AudioSource>();
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
        // Skip movement if player is dying
        if (isDying)
        {
            // Immediately stop all movement
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = input * runSpeed;

        /*
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().linearVelocity, runSpeed);
        GetComponent<Rigidbody2D>().AddForce(input.normalized * moveForce);
        if (input.normalized.Equals(Vector2.zero))
        {
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().linearVelocity, 0.0f);
            GetComponent<Rigidbody2D>().AddForce(input.normalized * friction);
        }*/
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
        PlayDeathSound();

        yield return new WaitForSeconds(1.0f);

        // next line after delay
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartFlashingAndRestart()
    {
        PlayVictorySound();
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

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayVictorySound()
    {
        audioSource.clip = victoryClip;
        audioSource.Play();
    }

    public void PlayDeathSound()
    {
        audioSource.clip = deathClip;
        audioSource.Play();
    }
}