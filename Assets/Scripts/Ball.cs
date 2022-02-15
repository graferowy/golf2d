using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Ball : MonoBehaviour
{
    public delegate void BallLost();
    public static event BallLost OnBallLost;
    
    private const float MIN_MOVING_SPEED = 0.1f;
    
    public Rigidbody2D Rigidbody { get; private set; }
    private CircleCollider2D _collider;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }
    
    public bool isMoving()
    {
        Vector2 velocity = Rigidbody.velocity;

        return Mathf.Abs(velocity.y) > MIN_MOVING_SPEED || Mathf.Abs(velocity.x) > MIN_MOVING_SPEED;
    }

    public void GoInTheHole()
    {
        Rigidbody.velocity = Vector2.zero;
        _collider.enabled = false;
        _renderer.sortingOrder = -5;
    }

    private void OnBecameInvisible()
    {
        if (GameManager.GameStatus == GameManager.Status.RUNNING)
        {
            OnBallLost?.Invoke();
        }
    }
}
