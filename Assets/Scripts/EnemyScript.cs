using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float chaseSpeed = 3.0f;
    public float repelSpeed = 1.5f;
    public float detectionRange = 5.0f;

    public enum EnemyColorType { Red, Blue }
    public EnemyColorType enemyColorType = EnemyColorType.Red;

    public GameObject explosionPrefab;

    public float pulseMinScale = 0.9f;
    public float pulseMaxScale = 1.1f;
    public float pulseSpeed = 3.0f;

    private Transform player;
    private PlayerScript playerScript;
    private SpriteRenderer playerSprite;
    private SpriteRenderer enemySprite;
    private Rigidbody2D rb;
    private Vector3 originalScale;
    private bool isPulsing = false;
    private Coroutine pulseCoroutine;

    private Color baseColor;

    public AudioClip explosionClip;
    private AudioSource audioSource;


    void Start()
    {

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }


        originalScale = transform.localScale;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            Debug.LogError("Player not found! Make sure it has the 'Player' tag.");
            return;
        }

        player = playerObject.transform;
        playerSprite = player.GetComponent<SpriteRenderer>();
        playerScript = player.GetComponent<PlayerScript>();

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Enemy needs a Rigidbody2D component!");
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError("Enemy needs a Collider2D component!");
            gameObject.AddComponent<BoxCollider2D>();
        }

        enemySprite = GetComponent<SpriteRenderer>();
        if (enemySprite == null)
        {
            Debug.LogError("Enemy needs a SpriteRenderer component!");
        }
        else
        {
            SetEnemyColor();
        }

        if (!gameObject.CompareTag("Enemy"))
        {
            Debug.LogError("This GameObject MUST have the 'Enemy' tag!");
            gameObject.tag = "Enemy";
        }

        if (explosionPrefab == null)
        {
            Debug.LogError("Explosion prefab is not assigned!");
        }
    }

    void OnDisable()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.localScale = originalScale;
        }

        StopPulsing();
    }

    private void SetEnemyColor()
    {
        if (enemyColorType == EnemyColorType.Red)
        {
            baseColor = Color.red;
        }
        else
        {
            baseColor = Color.blue;
        }

        enemySprite.color = baseColor;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        if (playerScript == null)
            playerScript = player.GetComponent<PlayerScript>();

        if (playerScript != null && playerScript.IsInNeutralState())
        {
            StopPulsing();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > detectionRange)
        {
            StopPulsing();
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;

        bool playerIsRed = (playerSprite.color == Color.red);
        bool enemyIsRed = (baseColor == Color.red);

        bool shouldAttract = (playerIsRed && !enemyIsRed) || (!playerIsRed && enemyIsRed);

        if (shouldAttract)
        {
            rb.MovePosition(rb.position + direction * chaseSpeed * Time.fixedDeltaTime);
            if (!isPulsing) StartPulsing();
        }
        else
        {
            rb.MovePosition(rb.position + direction * -1 * repelSpeed * Time.fixedDeltaTime);
            StopPulsing();
        }
    }

    private void StartPulsing()
    {
        if (isPulsing) return;

        isPulsing = true;

        if (pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);

        pulseCoroutine = StartCoroutine(PulseEffect());
    }

    private void StopPulsing()
    {
        if (!isPulsing) return;

        isPulsing = false;

        if (pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);

        transform.localScale = originalScale;
        enemySprite.color = baseColor;
    }

    private IEnumerator PulseEffect()
    {
        float t = 0f;

        while (isPulsing)
        {
            t += Time.deltaTime * pulseSpeed;

            float pulseFactor = Mathf.Lerp(pulseMinScale, pulseMaxScale, (Mathf.Sin(t * Mathf.PI) + 1f) / 2f);
            transform.localScale = originalScale * pulseFactor;

            float colorLerp = (Mathf.Sin(t * Mathf.PI) + 1f) / 2f;
            Color pulsingColor = Color.Lerp(baseColor, Color.white, colorLerp);
            enemySprite.color = pulsingColor;

            yield return null;
        }

        transform.localScale = originalScale;
        enemySprite.color = baseColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 explosionPosition = collision.contacts[0].point;
            if (explosionClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(explosionClip);
            }


            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(
                    explosionPrefab,
                    new Vector3(explosionPosition.x, explosionPosition.y, -1),
                    Quaternion.identity
                );

                explosion.transform.localScale = Vector3.one * 2.0f;
                Destroy(explosion, 2.0f);
            }
            else
            {
                CreateSimpleExplosion(explosionPosition);
            }

            if (playerScript != null)
            {
                playerScript.StartCoroutine("DieWithDelay");
            }
        }
    }

    private void CreateSimpleExplosion(Vector2 position)
    {
        GameObject explosion = new GameObject("SimpleExplosion");
        explosion.transform.position = new Vector3(position.x, position.y, -1);

        SpriteRenderer renderer = explosion.AddComponent<SpriteRenderer>();
        renderer.sprite = CreateCircleSprite();
        renderer.color = Color.yellow;
        renderer.sortingOrder = 10;

        StartCoroutine(GrowAndFadeExplosion(explosion));
    }

    private IEnumerator GrowAndFadeExplosion(GameObject explosion)
    {
        float duration = 0.5f;
        float timer = 0;
        SpriteRenderer renderer = explosion.GetComponent<SpriteRenderer>();

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            explosion.transform.localScale = Vector3.one * Mathf.Lerp(0.1f, 2.0f, progress);

            Color color = renderer.color;
            color.a = Mathf.Lerp(1.0f, 0.0f, progress);
            renderer.color = color;

            yield return null;
        }

        Destroy(explosion);
    }

    private Sprite CreateCircleSprite()
    {
        Texture2D texture = new Texture2D(128, 128);
        Color[] colors = new Color[texture.width * texture.height];

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(texture.width / 2, texture.height / 2));
                colors[y * texture.width + x] = (dist <= texture.width / 2) ? Color.white : Color.clear;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
