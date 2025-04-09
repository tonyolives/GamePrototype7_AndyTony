using UnityEngine;

public class MissileTurretScript : MonoBehaviour
{
    public GameObject redMissilePrefab; // reference to red missile prefab
    public GameObject blueMissilePrefab; // reference to blue missile prefab
    public float fireRate = 2f; // time between shots
    public Transform firePoint; // where the missile spawns from

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("FireMissile", 1f, fireRate); // start firing repeatedly
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
        Instantiate(prefab, firePoint.position, firePoint.rotation);
    }
}