﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class Player : MonoBehaviour, ITakeDamage
{

    private bool _IsFacingRight;
    private CharacterController2D _controller;
    private float _normalizeHorizontalSpeed;

    public float MaxSpeed = 8f;
    public float SpeedAccelerationOnGrond = 10f;
    public float SpeedAccelerationInAir = 5f;
    public int MaxHealth = 3;
    public GameObject OuchEffect;
    public Projectile Projectile;
    public float FireRate;
    public Transform ProjectileFireLocation;
    public AudioClip PlayerPunchSound;

    public Animator Animator { get; set; }
    private Animator AnimatorInteractive;
    public bool Focused;
    public int PosPlayerSelect;

    public bool IsNearToNpc { get; set; }
    public bool IsNearToInteractiveObject { get; set; }
    public int Health { get; private set; }
    public bool IsDead { get; private set; }
    public bool CanInput { get; set; }

    private float _canFireIn;

    public void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        Animator = GetComponentsInChildren<Animator>().Where(e=>  e.name == "Animação").FirstOrDefault();
        AnimatorInteractive = GetComponentsInChildren<Animator>().Where(e => e.name == "Interrogation").FirstOrDefault();
        _IsFacingRight = transform.localScale.x > 0;
        Health = MaxHealth;

    }

    public void Update()
    {
        _canFireIn -= Time.deltaTime;

        if (!Focused || !CanInput)
        {
            int fator = _IsFacingRight ? 1 : -1;
            _normalizeHorizontalSpeed = fator * _normalizeHorizontalSpeed > 0 ? _normalizeHorizontalSpeed - Time.deltaTime * fator : 0;
        }
        else
        {
            if (!IsDead)
                HandleInput();
            else
            {
                int fator = _IsFacingRight ? 1 : -1;
                _normalizeHorizontalSpeed = fator * _normalizeHorizontalSpeed > 0 ? _normalizeHorizontalSpeed - Time.deltaTime * fator : 0;
            }
        }

        var movementFactor = _controller.State.NoChao ? SpeedAccelerationOnGrond : SpeedAccelerationInAir;
        _controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocidade.x, _normalizeHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));

        Animator.SetBool("IsGround", _controller.State.NoChao);
        Animator.SetBool("IsDead", IsDead);
        Animator.SetFloat("Speed", Mathf.Abs(_controller.Velocidade.x) / MaxSpeed);
        AnimatorInteractive.GetComponent<SpriteRenderer>().enabled = IsNearToInteractiveObject;
        AnimatorInteractive.SetBool("IsInterrogation", IsNearToInteractiveObject);
    }

    public void Kill()
    {
        //_controller.HandleCollisions = false;
        collider2D.enabled = false;
        IsDead = true;
        Health = 0;
        _controller.SetForce(new Vector2(0, 5));

    }

    public void RespawnAt(Transform spawnPoint)
    {
        if (!_IsFacingRight)
            Flip();

        IsDead = false;
        collider2D.enabled = true;
        _controller.HandleCollisions = true;
        Health = MaxHealth;

        transform.position = spawnPoint.position;
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        AudioSource.PlayClipAtPoint(PlayerPunchSound, transform.position);
        Instantiate(OuchEffect, transform.position, transform.rotation);
        Health -= damage;

        if (Health <= 0)
            LevelManager.Instance.KillPlayer(this);

    }

    private void HandleInput()
    {
        if (!CanInput)
            return;

        if (Input.GetKeyDown(KeyCode.A) && Application.loadedLevelName != "start")
        {
            _normalizeHorizontalSpeed = 0;
            LevelManager.Instance.NextPLayer();
            return;
        }

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

        if (Input.GetKeyDown(KeyCode.X) && Application.loadedLevelName != "start")
        {
            if (!IsNearToNpc)
                FireProjectile();
        }

    }

    private void FireProjectile()
    {
        if (_canFireIn > 0)
            return;
        AudioSource.PlayClipAtPoint(PlayerPunchSound, transform.position);
        Animator.SetTrigger("Punch");
        var direction = _IsFacingRight ? Vector2.right : -Vector2.right;

        var projectile = (Projectile)Instantiate(Projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
        projectile.Initialize(gameObject, direction, _controller.Velocidade);



        _canFireIn = FireRate;
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        AnimatorInteractive.transform.localScale = new Vector3(-AnimatorInteractive.transform.localScale.x, 
            AnimatorInteractive.transform.localScale.y, AnimatorInteractive.transform.localScale.z);
        _IsFacingRight = transform.localScale.x > 0;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        BaseLevelActions.Instance.PlayerOnCollider(other);

    }

}
