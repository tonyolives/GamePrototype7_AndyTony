using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;
    public float runSpeed = 20.0f;
    private SpriteRenderer _spriteRenderer;
    private AudioSource audioSource;

    void Start ()
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
        // movement
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        // on spacebar click
        if(Input.GetKeyDown(KeyCode.Space)) {
            // change color
            if (_spriteRenderer.color == Color.red) _spriteRenderer.color = Color.blue;
            else if (_spriteRenderer.color == Color.blue) _spriteRenderer.color = Color.red;
            // see if you can pass through blue wall
            UpdateWallCollision();
        }

        // on R click
        if(Input.GetKeyDown(KeyCode.R)) {
            // reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        } 

    rb.linearVelocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
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

    public void StartFlashingAndRestart()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // Play sound effect
        }
        StartCoroutine(FlashGreenAndRestart());
    }

    private IEnumerator FlashGreenAndRestart()
    {
        for (int i = 0; i < 6; i++) // Flash 6 times (2 seconds)
        {
            _spriteRenderer.color = Color.green;
            yield return new WaitForSeconds(0.2f);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
