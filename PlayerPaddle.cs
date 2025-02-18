using UnityEngine;

public class PlayerPaddle : Paddle
{
    private bool isDragging = false;
    private Vector2 lastTouchPosition;
    private float screenHalfY;
    private float paddleWidth;

    [SerializeField] private float smoothingSpeed = 10f;

    private void Start()
    {
        screenHalfY = Screen.height / 2f;
        paddleWidth = GetComponent<BoxCollider2D>().bounds.size.x / 2f;
    }

    private void Update()
    {
        HandleTouchInput();
        HandleMouseInput();
    }

    private void HandleTouchInput()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && touch.position.y < screenHalfY)
            {
                isDragging = true;
                lastTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 targetPosition = Camera.main.ScreenToWorldPoint(touch.position);
                MovePaddle(targetPosition.x);
                lastTouchPosition = targetPosition;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y < screenHalfY)
        {
            isDragging = true;
            lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MovePaddle(targetPosition.x);
            lastTouchPosition = targetPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private void MovePaddle(float targetX)
    {
        float leftBoundary = -7f + paddleWidth;
        float rightBoundary = 7f - paddleWidth;

        targetX = Mathf.Clamp(targetX, leftBoundary, rightBoundary);

        Vector2 newPosition = new Vector2(targetX, rb.position.y);
        rb.position = Vector2.Lerp(rb.position, newPosition, Time.deltaTime * smoothingSpeed);
    }
}