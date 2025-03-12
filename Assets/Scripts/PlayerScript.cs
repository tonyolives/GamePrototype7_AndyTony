using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;
    public float runSpeed = 20.0f;
    private SpriteRenderer _spriteRenderer;

    void Start ()
    {
        // sprite renderer component to access color - default is RED
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.red;

        // rigid body component to access velocity
        rb = GetComponent<Rigidbody2D>();
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
}
