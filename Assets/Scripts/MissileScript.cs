using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    public float baseSpeed = 3f; // missile's base forward speed
    public float explosionRadius = 1.2f; // radius for explosion damage
    public float lifetime = 4f; // time before missile self-destructs
    public float forcePower = 7f; // max attractive / repelling force
    public float influenceRange = 3f; // distance at which polarity force applies
    public LayerMask destructibleLayer; // layer mask for destructible objects

    [Header("Magnetic Force Settings")]
    public float attractStrength = 50f; // strength of magnetic pull
    public float repelStrength = 30f; // strength of close-range drop-off
    public float attractPower = 2f; // how fast attraction grows (e.g. gravity = 2)
    public float repelPower = 4f; // how fast repulsion grows (should be > attractPower)
    public float minDistance = 0.5f; // clamp to avoid zero division

    public Transform player;
    private SpriteRenderer playerSprite;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isRedMissile;
    private bool isExploding = false;
    private Vector3 initialDirection;
    private Vector2 currentVelocity;
    
    public AudioManager SFX;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SFX = FindFirstObjectByType<AudioManager>();
        playerSprite = player.GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // check color of missile
        isRedMissile = gameObject.CompareTag("RedMissile");

        // store launch direction (based on spawn rotation)
        initialDirection = transform.up;

        // set initial velocity
        currentVelocity = transform.up * baseSpeed;

        // yellow missile flashing
        StartCoroutine(FlashYellow());

        // detonate after set time
        Invoke("Explode", lifetime);
    }

    void FixedUpdate()
    {
        if (!player) return;

        Vector2 toPlayer = (Vector2)(player.position - transform.position);
        float distance = toPlayer.magnitude;

        if (distance > influenceRange)
        {
            rb.linearVelocity = currentVelocity;
            return;
        }

        // skip if player is neutral
        if (player.GetComponent<PlayerScript>().IsInNeutralState()) return;

        // determine polarity interaction
        bool playerIsRed = playerSprite.color == Color.red;
        bool shouldAttract = (playerIsRed && !isRedMissile) || (!playerIsRed && isRedMissile);

        if (shouldAttract)
        {
            Vector2 dir = toPlayer.normalized;
            float r = Mathf.Max(distance, minDistance); // avoid division by zero

            // van der waals things

            // float F_attr = attractStrength / Mathf.Pow(r, attractPower);
            // float F_rep = repelStrength / Mathf.Pow(r, repelPower);
            // float forceMag = F_attr - F_rep;

            // hard clamp - hard to touch player
            // if (forceMag < 0f) forceMag = 0f;

            // no hard clamp - allows slight forward momentum
            // forceMag = Mathf.Max(forceMag, -baseSpeed);

            float barrierDistance = 0.8f; // tweakable sweet spot
            float smoothFalloff = Mathf.Clamp01((r - minDistance) / (barrierDistance - minDistance));
            float forceMag = Mathf.Lerp(forcePower, forcePower * 0.2f, 1f - smoothFalloff); // strong to soft pull

            Vector2 perp = new Vector2(-dir.y, dir.x); // perpendicular direction
            Vector2 curvedForce = dir * forceMag + perp * (forceMag * 0.4f); // tweak 0.2f for more curve
            currentVelocity = curvedForce;
        }
        else if (!shouldAttract)
        {
            // repulsion: simple linear falloff like before
            Vector2 dir = (-toPlayer).normalized;
            float strength = Mathf.Lerp(forcePower * 0.6f, forcePower, 1f - (distance / influenceRange));
            currentVelocity = dir * strength;
        }

        rb.linearVelocity = currentVelocity;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"MISSILE hit object: {other.gameObject.name}, Layer: {LayerMask.LayerToName(other.gameObject.layer)}");

        // Don't explode if colliding with another missile
        if (other.gameObject.GetComponent<MissileScript>() != null)
        {
            // Missiles collided with each other, don't explode
            return;
        }

        // if not a turret explode
        if (!other.gameObject.CompareTag("Turret"))
        {
            Explode();
        }
    }

    void Explode()
    {
        rb.linearVelocity = Vector2.zero;
        isExploding = true;
        StartCoroutine(ExpandAndExplode());
        SFX.Explosion();
    }

    IEnumerator ExpandAndExplode()
    {
        spriteRenderer.color = Color.yellow;
        float scaleDuration = 0.2f;
        float timer = 0f;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = Vector3.one * explosionRadius * 2f; // scale to match explosion area

        while (timer < scaleDuration)
        {
            timer += Time.deltaTime;
            float t = timer / scaleDuration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        // damage destructible environment objects in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, destructibleLayer);
        foreach (var hit in hits)
        {
            Destroy(hit.gameObject);
        }

        Destroy(gameObject);
    }

    IEnumerator FlashYellow()
    {
        float interval = 0.5f; // starts slow
        float elapsed = 0f;

        while (!isExploding)
        {
            // flash yellow
            spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(interval / 2f);

            // revert to normal color
            if (isRedMissile)
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(interval / 2f);
            }
            else
            {
                spriteRenderer.color = Color.blue;
                yield return new WaitForSeconds(interval / 2f);
            }

            // update timer
            elapsed += interval;
            float t = Mathf.Clamp01(elapsed / lifetime); // how far along the missile is

            // decrease interval over time (faster blinking)
            interval = Mathf.Lerp(0.05f, 0.5f, 1f - t);
        }
    }
}