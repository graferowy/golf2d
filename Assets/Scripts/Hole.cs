using UnityEngine;

public class Hole : MonoBehaviour
{
    public delegate void WinHole();
    public static event WinHole OnWin;
    
    [SerializeField]
    private float _maxAllowedSpeed;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!col.CompareTag("Player") || GameManager.GameStatus != GameManager.Status.RUNNING)
        {
            return;
        }

        if (!col.TryGetComponent(out Ball ball) || !col.TryGetComponent(out Rigidbody2D rb))
        {
            throw new MissingComponentException();
        }

        if (rb.velocity.y > 0 || Mathf.Abs(rb.velocity.x) > _maxAllowedSpeed)
        {
            return;
        }
        
        ball.GoInTheHole();
        OnWin?.Invoke();
    } 
}
