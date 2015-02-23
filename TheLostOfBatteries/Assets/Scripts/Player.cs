using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

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
    public AudioClip PlayerProjectileSound;
    public Itens Itens;

    public Animator Animator { get; set; }
    private Animator AnimatorInteractive;
    public bool Focused;
    public int PosPlayerSelect;


    public bool IsNearToNpc { get; set; }
    public bool IsNearToInteractiveObject { get; set; }
    public int Health { get; private set; }
    public bool IsDead { get; private set; }
    public bool CanInput { get; set; }
    public bool IsArmado { get; set; }

    private float _canFireIn;
    private float _canHeal;
    private volatile bool _giveDamage;
    private volatile Collider2D _currentEnemy;
    private volatile IList<Collider2D> _enemiesToHit;
    private volatile Dictionary<string, IEnumerator> _routines;
    private volatile Dictionary<string, bool> _listGiveDamage;

    public void Awake()
    {
    //    Itens.Add(name + "Arma");
        _enemiesToHit = new List<Collider2D>();
        _routines = new Dictionary<string, IEnumerator>();
        _listGiveDamage = new Dictionary<string, bool>();
        _controller = GetComponent<CharacterController2D>();
        Animator = GetComponentsInChildren<Animator>().Where(e => e.name == "Animação").FirstOrDefault();
        AnimatorInteractive = GetComponentsInChildren<Animator>().Where(e => e.name == "Interrogation").FirstOrDefault();
        _IsFacingRight = transform.localScale.x > 0;
        Health = MaxHealth;
    }

    public void Update()
    {
        _canFireIn -= Time.deltaTime;
        _canHeal -= Time.deltaTime;

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
        Animator.SetBool("IsArmado", IsArmado);
    }

    public void Kill()
    {
        //_controller.HandleCollisions = false;
        collider2D.enabled = false;
        IsDead = true;
        Health = 0;
        _controller.SetForce(new Vector2(0, 5));

    }

    public void RespawnAt(Transform spawnPoint, float offSet)
    {
        if (!_IsFacingRight)
            Flip();

        IsDead = false;
        collider2D.enabled = true;
        _controller.HandleCollisions = true;
        Health = MaxHealth;

        transform.position = new Vector3(spawnPoint.position.x + offSet, spawnPoint.position.y, spawnPoint.position.z);
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

        if (_controller.enabled && _controller.PodePular && Input.GetKeyDown(KeyCode.Z))
        {
            _controller.Jump();
        }

        if (Input.GetKeyDown(KeyCode.X) && Application.loadedLevelName != "start")
        {
            if (!IsNearToNpc)
            {
                var acao = Itens.GetAcao();

                if (acao == Itens.Acao.Fist)
                    Punch();
                else if (acao == Itens.Acao.Projectile)
                    FireProjectile();
                else if (acao == Itens.Acao.LifeRecover)
                    HealLife();
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveItemFocus();
        }

    }

    private void Punch()
    {
        if (_canFireIn > 0)
            return;

        Animator.SetTrigger("Punch");

        _currentEnemy = _enemiesToHit.Where(e => _listGiveDamage[e.name]).OrderBy(e =>
            Mathf.Abs(collider2D.transform.position.x - e.transform.position.x)).FirstOrDefault();

        if (_currentEnemy == null || _enemiesToHit.Count() == 0)
            return;

        //TODO tratar toda lapada, como tirar daqui aqui embaixo

        //_currentEnemy.name = _currentEnemy.name + " X ";
        //Debug.Log("Inimigo apanhou: " + _currentEnemy.name);
        AudioSource.PlayClipAtPoint(PlayerPunchSound, transform.position);
        _currentEnemy.GetComponent<SimpleEnemyAI>().TakeDamage(1, gameObject);
        StopCoroutine(_routines[_currentEnemy.name]);
        _enemiesToHit.Remove(_currentEnemy);
        _routines[_currentEnemy.name] = null;
        _listGiveDamage[_currentEnemy.name] = false;
        _canFireIn = FireRate;

    }

    public void HandleDamageToEnemy(Collider2D other)
    {
        if (!_giveDamage)
            return;

    }

    private void MoveItemFocus()
    {
        if (Itens != null)
           IsArmado = Itens.ChangeItem().Contains("Arma");
        //if (IsArmado)
        //    Animator.SetTrigger("GetWeapon");
    }

    public void HealLife()
    {
        Health++;
        Health = Mathf.Min(Health, MaxHealth);
    }

    private void FireProjectile()
    {
        if (_canFireIn > 0)
            return;
        AudioSource.PlayClipAtPoint(PlayerProjectileSound, transform.position);

        var direction = _IsFacingRight ? Vector2.right : -Vector2.right;

        Animator.SetTrigger("Shoot");

        if (Projectile != null)
        {
            var projectile = (Projectile)Instantiate(Projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
            projectile.Initialize(gameObject, direction, _controller.Velocidade);
        }
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

        if (other.tag == "Enemy")
        {
            var newRoutine = HitEnemy(other);
            _routines[other.name] = newRoutine;
            StartCoroutine(newRoutine);
        }


    }

    public void OnTriggerExit2D(Collider2D other)
    {
        BaseLevelActions.Instance.PlayerOnCollider(other);

        if (other.tag == "Enemy")
        {
            var removeRoutive = _routines[other.name];
            StopCoroutine(removeRoutive);
            _enemiesToHit.Remove(other);
            _routines[other.name] = null;
            _listGiveDamage[other.name] = false;
            //Debug.Log(name + ": COMEÇA");
            //Debug.Log(other.name + " Removido");
            //foreach (var routine in _routines)
            //{
            //    Debug.Log("     REM:" + routine.Value);
            //}
            //Debug.Log(name + ":TERMINA");


            //Debug.Log(name + ": COMEÇA");
            //Debug.Log(other.name + " Removido");
            //foreach(var enemy in _enemiesToHit  )
            //{
            //    Debug.Log("     REM:" + enemy.name);
            //}
            //Debug.Log(name + ":TERMINA");
        }

    }

    public IEnumerator HitEnemy(Collider2D other)
    {
        //if (_enemyToHit != null)
        //    yield break;

        _enemiesToHit.Add(other);


        //Debug.Log(name + ": COMEÇA");
        //Debug.Log(other.name + " Adicionado");
        //foreach (var enemy in _enemiesToHit)
        //{
        //    Debug.Log("     ADD: " + enemy.name);
        //}
        //Debug.Log(name + ": TERMINA");

        while (true)
        {
            var direction = transform.position.x - other.transform.position.x;

            if ((_IsFacingRight && direction < 0) || (!_IsFacingRight && direction > 0))
                _listGiveDamage[other.name] = true;
            else
            {
                var limitePlayer = _IsFacingRight ?
                    collider2D.transform.position.x + collider2D.bounds.size.x / 2 :
                    collider2D.transform.position.x - collider2D.bounds.size.x / 2;
                var limiteEnimy = _IsFacingRight ?
                    (other.transform.position.x + other.bounds.size.x / 2) :
                    (other.transform.position.x - other.bounds.size.x / 2);

                //   Debug.DrawLine(transform.position, other.transform.position, Color.red);
                //   Debug.DrawLine(other.transform.position, new Vector3(limiteEnimy, other.transform.position.y, 0)
                //       , Color.blue);
                //   Debug.DrawLine(transform.position, new Vector3(limitePlayer, transform.position.y, 0)
                //, Color.yellow);


                if ((_IsFacingRight && limitePlayer - limiteEnimy < 0) || (!_IsFacingRight && limitePlayer - limiteEnimy > 0))
                    _listGiveDamage[other.name] = true;
                else
                    _listGiveDamage[other.name] = false;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }



}
