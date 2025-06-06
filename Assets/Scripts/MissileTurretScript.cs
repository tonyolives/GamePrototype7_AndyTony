using UnityEngine;

public class MissileTurretScript : MonoBehaviour
{
    public GameObject redMissilePrefab; // reference to red missile prefab
    public GameObject blueMissilePrefab; // reference to blue missile prefab
    public float fireRate = 2f; // time between shots
    public Transform firePoint; // where the missile spawns from
    private SpriteRenderer _spriteRenderer;

    private Transform player;

    public bool isBlue;
    public bool isRed;
    public bool isWide;
    public bool isGatling;

    public Transform widePointcenter;
    public Transform widePointLeft;
    public Transform widePointright;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (isGatling)
        {
            InvokeRepeating(nameof(FireGatlingBurst), 1f, fireRate);
            return; // so no other modes instantiate
        }
        
        if ((isBlue == false && isRed == false) || (isBlue == true && isRed == true))
        {
            InvokeRepeating("FireMissile", 1f, fireRate); // start firing repeatedly
        }
        if (isBlue == false && isRed == true)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            //_spriteRenderer.color = Color.red;            
            InvokeRepeating("FireRed", 1f, fireRate); // start firing red missiles repeatedly
        }

        if (isBlue == true && isRed == false)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            //_spriteRenderer.color = Color.blue;            
            InvokeRepeating("FireBlue", 1f, fireRate); // start firing red missiles repeatedly
        } 
    }

    void Update()
    {
        if (!player) return;

        // rotate turret to face the player
        Vector2 dir = player.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    void FireMissile()
    {
        // randomly choose a red or blue missile to fire
        GameObject prefab = (Random.value > 0.5f) ? redMissilePrefab : blueMissilePrefab;

        if (isWide)
        {
            Instantiate(prefab, widePointcenter.position, widePointcenter.rotation);
            Instantiate(prefab, widePointLeft.position, widePointLeft.rotation);
            Instantiate(prefab, widePointright.position, widePointright.rotation);
        }
        else
        {
            Instantiate(prefab, firePoint.position, firePoint.rotation);
        }
    }

    void FireRed()
    {
        // Choose red missile and fire
        GameObject prefab = redMissilePrefab;

        if (isWide)
        {
            Instantiate(prefab, widePointcenter.position, widePointcenter.rotation);
            Instantiate(prefab, widePointLeft.position, widePointLeft.rotation);
            Instantiate(prefab, widePointright.position, widePointright.rotation);
        }
        else
        {
            Instantiate(prefab, firePoint.position, firePoint.rotation);
        }
    }

    void FireBlue()
    {
        // Choose blue missile and fire
        GameObject prefab = blueMissilePrefab;
        if (isWide)
        {
            Instantiate(prefab, widePointcenter.position, widePointcenter.rotation);
            Instantiate(prefab, widePointLeft.position, widePointLeft.rotation);
            Instantiate(prefab, widePointright.position, widePointright.rotation);
        }
        else
        {
            Instantiate(prefab, firePoint.position, firePoint.rotation);
        }
    }

    void FireGatlingBurst()
    {
        StartCoroutine(GatlingBurstCoroutine());
    }

    System.Collections.IEnumerator GatlingBurstCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject prefab = (Random.value > 0.5f) ? redMissilePrefab : blueMissilePrefab;
            Instantiate(prefab, firePoint.position, firePoint.rotation);
            yield return new WaitForSeconds(0.1f);
        }
    }
}