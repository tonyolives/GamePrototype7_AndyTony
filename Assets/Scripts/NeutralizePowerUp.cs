using UnityEngine;

public class NeutralizePowerUp : MonoBehaviour
{
    public float powerUpDuration = 10.0f;
    public float rotationSpeed = 50.0f;
    public AudioClip pickupSound;

    private AudioSource audioSource;

    void Start()
    {
        // Make sure it has a proper collider
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }

        // Make sure it has a sprite renderer with green color
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("NeutralizePowerUp needs a SpriteRenderer component!");
        }
        else
        {
            spriteRenderer.color = Color.green;
        }

        // Set up audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Simple rotation effect
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScript playerScript = other.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                Debug.Log("Powerup collected! Player neutralized.");

                // Play sound effect
                if (pickupSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(pickupSound);
                }

                playerScript.ActivateNeutralState(powerUpDuration);
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject, pickupSound != null ? pickupSound.length + 0.5f : powerUpDuration + 1.0f);
            }
        }
    }
}
