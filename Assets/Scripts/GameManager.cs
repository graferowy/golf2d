using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Status GameStatus;
    
    public enum Status
    {
        RUNNING,
        FINISHED,
        PAUSED
    }
    
    [SerializeField]
    private AudioClip _holeSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        GameStatus = Status.PAUSED;
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameStatus = Status.RUNNING;
        Hole.OnWin += OnWin;
    }

    private void OnWin()
    {
        GameStatus = Status.FINISHED;
        _audioSource.PlayOneShot(_holeSound);
    }

    private void OnDisable()
    {
        Hole.OnWin -= OnWin;
    }
}
