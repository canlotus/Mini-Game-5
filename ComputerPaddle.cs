using UnityEngine;

public class ComputerPaddle : Paddle
{
    [SerializeField]
    private Rigidbody2D ball;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed; 
    }

    private void FixedUpdate()
    {
        if (ball.position.x < rb.position.x) 
        {
            rb.velocity = Vector2.left * speed;
        }
        else if (ball.position.x > rb.position.x)
        {
            rb.velocity = Vector2.right * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}