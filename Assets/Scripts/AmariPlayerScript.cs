using UnityEngine;

public class AmariPlayerScript : MonoBehaviour
{
    private float horizontal;
    private float jumpingPower = 32f;
    bool canJump = true;

    [SerializeField] private Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {
            
        Debug.Log("Space Pressed!");

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            
            rb.AddForce(Vector2.up * jumpingPower);
        }

        // if (Input.GetKeyUp(KeyCode.Space))
        // {
        //     rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        // }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Ground"))
            {
                canJump = true;
            }
        }
        
    }
}