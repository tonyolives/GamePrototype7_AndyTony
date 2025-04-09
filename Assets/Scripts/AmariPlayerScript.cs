using UnityEngine;
using System.Collections;

public class AmariPlayerScript : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    public float jumpingPower = 20f;
    public float runSpeed = 20.0f;
    public bool canJump = true;
    
    // private Transform redCoin;
    // private Transform blueCoin;
    // public float attractionSpeed = 2.4f; // Speed of attraction/repulsion
    // public float detectionRange = 3.5f; // Distance where the effect is applied

    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Rigidbody2D rb;

    void Start ()
    {
        // redCoin = GameObject.FindGameObjectWithTag("RedCoin").transform;
        // blueCoin = GameObject.FindGameObjectWithTag("BlueCoin").transform;

        rb = GetComponent<Rigidbody2D>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.red;

        
    }

    void FixedUpdate()
    {

            
        

        // float redDistance = Vector2.Distance(transform.position, redCoin.position);
        // if (redDistance > detectionRange) return; // only react if within range
        // //Debug.Log("Red Complete!");
        // float blueDistance = Vector2.Distance(transform.position, blueCoin.position);
        // if (blueDistance > detectionRange) return; // only react if within range
        // Debug.Log("Blue Complete!");

        // bool playerIsRed = (_spriteRenderer.color == Color.red);

        // if (playerIsRed == true)
        // {

        //     Vector2 redDirection = (redCoin.position - transform.position).normalized;
        //     Vector2 blueDirection = (blueCoin.position - transform.position).normalized;
        //     blueDirection *= -1;
        //     if(Input.GetKeyDown(KeyCode.E))
        //     {
                
        //         rb.MovePosition(rb.position + redDirection * attractionSpeed * Time.fixedDeltaTime);
        //         rb.MovePosition(rb.position + blueDirection * attractionSpeed * Time.fixedDeltaTime);
        //     }
        // }
        // else
        // {
        //     Vector2 redDirection = (redCoin.position - transform.position).normalized;
        //     Vector2 blueDirection = (blueCoin.position - transform.position).normalized;
        //     redDirection *= -1;
        //     if(Input.GetKeyDown(KeyCode.E))
        //     {
        //         rb.MovePosition(rb.position + redDirection * attractionSpeed * Time.fixedDeltaTime);
        //         rb.MovePosition(rb.position + blueDirection * attractionSpeed * Time.fixedDeltaTime);
        //     }
        // }
                
        
        
    }

    void Update()
    {
        canJump = true;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.S))
        rb.linearVelocity = new Vector2(horizontal * runSpeed, vertical * jumpingPower);
        

        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            // change color
            if (_spriteRenderer.color == Color.red) _spriteRenderer.color = Color.blue;
            else if (_spriteRenderer.color == Color.blue) _spriteRenderer.color = Color.red;
        }
        
        // if (Input.GetKey(KeyCode.Space) && canJump == true)
        // {
        //     rb.linearVelocity = new Vector2(0, 10);
        //     canJump = false;
        // }


    }
        
    private void OnCollisionEnter2D(Collision2D collision)
    {      
        
        
        if(collision.gameObject.CompareTag("Ground"))
        {
            
        }
    } 
    
}