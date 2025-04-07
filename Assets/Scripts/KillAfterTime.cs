using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAfterTime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float TimetoDeath = 2.0f;
    
    void Start()
    {
        StartCoroutine(waiter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator waiter()
    {
        //Wait for 2 seconds
        yield return new WaitForSeconds(TimetoDeath);
        Destroy(gameObject);
    }
}
