﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Paddle : MonoBehaviour
{
    protected Rigidbody2D rb;

    public float speed = 8f;
    [Tooltip("Changes how the ball bounces off the paddle depending on where it hits the paddle. The further from the center of the paddle, the steeper the bounce angle.")]
    public bool useDynamicBounce = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ResetPosition()
    {
        rb.velocity = Vector2.zero;
        rb.position = new Vector2(0f, rb.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (useDynamicBounce && collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ball = collision.rigidbody;
            Collider2D paddle = collision.otherCollider;


            Vector2 ballDirection = ball.velocity.normalized;
            Vector2 contactDistance = ball.transform.position - paddle.bounds.center;
            Vector2 surfaceNormal = collision.GetContact(0).normal;
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, surfaceNormal);

            float maxBounceAngle = 75f;
            float bounceAngle = contactDistance.x / paddle.bounds.size.x * maxBounceAngle;
            ballDirection = Quaternion.AngleAxis(bounceAngle, rotationAxis) * ballDirection;

            ball.velocity = ballDirection * ball.velocity.magnitude;
        }
    }
}