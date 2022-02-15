using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Controls : MonoBehaviour
{
    [SerializeField]
    private AudioClip _puttSound;
    [SerializeField]
    private TMP_Text _hitsText;
    [SerializeField]
    private float _maxForce;
    [SerializeField]
    private float _additionalForce;
    private LineRenderer _lineRenderer;
    private Camera _camera;
    private AudioSource _audioSource;
    private Ball _ball;
    private Vector3 _hitStartPos;
    private Vector3 _lastPosition;
    private int _hits;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _camera = Camera.main;
        _hits = 0;
    }

    private void Start()
    {
        _ball = FindObjectOfType<Ball>();
        _lastPosition = _ball.transform.position;
    }

    private void OnEnable()
    {
        Ball.OnBallLost += ResetBallPosition;
    }

    private void Update()
    {
        HandleControls();
    }

    private void HandleControls()
    {
        if (_ball.isMoving())
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            OnHitStart();
        }

        if (Input.GetMouseButton(0))
        {
            OnHitDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnHitRelease();
        }
    }

    private void OnHitStart()
    {
        _hitStartPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        _hitStartPos.z = 0;
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, _ball.transform.position);
    }

    private void OnHitDrag()
    {
        Vector3 hitDragPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        hitDragPos.z = 0;
        Vector3 lineEndPos = hitDragPos - _hitStartPos;
        float distance = Mathf.Clamp(Vector3.Distance(_hitStartPos, hitDragPos), 0, _maxForce);
        lineEndPos = lineEndPos.normalized * distance;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(1, _ball.transform.position + lineEndPos);
    }

    private void OnHitRelease()
    {
        Vector3 hitReleasePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        hitReleasePos.z = 0f;

        Vector3 force = _hitStartPos - hitReleasePos;
        force = Vector3.ClampMagnitude(force, _maxForce) * _additionalForce;

        _lastPosition = _ball.transform.position;
        
        _ball.Rigidbody.AddForce(force, ForceMode2D.Impulse);
        
        _lineRenderer.positionCount = 0;
        _hits++;
        _hitsText.text = $"{_hits} Hits";
        _audioSource.PlayOneShot(_puttSound);
    }

    private void ResetBallPosition()
    {
        _ball.transform.position = _lastPosition;
        _ball.Rigidbody.velocity = Vector2.zero;
    }

    private void OnDisable()
    {
        Ball.OnBallLost -= ResetBallPosition;
    }
}
