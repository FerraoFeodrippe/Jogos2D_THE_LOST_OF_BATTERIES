using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    private bool _IsFacingRight;
    private CharacterController2D _controller;
    private float _normalizeHorizontalSpeed;

    public float MaxSpeed = 8f;
    public float SpeedAccelerationOnGrond = 10f;
    public float SpeedAccelerationInAir = 5f;

    public bool IsDead { get; private set; }

    public void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _IsFacingRight = transform.localScale.x > 0;

    }

    public void Update()
    {
        if (!IsDead)
            HandleInput();

        var movementFactor = _controller.State.NoChao ? SpeedAccelerationOnGrond : SpeedAccelerationInAir;
        _controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocidade.x, _normalizeHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));

    }

    public void Kill()
    {
        _controller.HandleCollisions = false;
        collider2D.enabled = false;

        IsDead = true;
    }

    public void RespawnAt(Transform spawnPoint)
    {
        if (!_IsFacingRight)
            Flip();

        IsDead = false;
        collider2D.enabled = true;
        _controller.HandleCollisions = true;
        transform.position = spawnPoint.position;
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.L))
        {
            _normalizeHorizontalSpeed = 1;
            if (!_IsFacingRight)
                Flip();
        }
        else if (Input.GetKey(KeyCode.J))
        {
            _normalizeHorizontalSpeed = -1;
            if (_IsFacingRight)
                Flip();
        }
        else
        {
            _normalizeHorizontalSpeed = 0;
        }

        if (_controller.PodePular && Input.GetKeyDown(KeyCode.Z))
        {
            _controller.Jump();
        }

    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        _IsFacingRight = transform.localScale.x > 0;
    }

}
