using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {
    private const float SkinWidth = 0.02f;
    private const int LimitesHorizontais = 8;
    private const int LimitesVerticais = 4;

    private static readonly float LimiteTangenteInclinacao = Mathf.Tan(75f * Mathf.Deg2Rad);

    public LayerMask PLataformaMark;
    public ControllerParameters2D ValoresPadrao;

    public ControllerState2D State { get; private set; }
    public Vector2 Velocidade { get { return _velocidade; } }
    public bool PodePular 
    { 
        get 
        {
            if (Parameters.RestricoesPulo == ControllerParameters2D.PuloBehavior.Anywhere)
                return _jumpIn < 0;

            if (Parameters.RestricoesPulo == ControllerParameters2D.PuloBehavior.OnlyOnGround)
                return State.NoChao;

            return false;
        } 
    }
    public bool HandleCollisions { get; set; }
    public ControllerParameters2D Parameters { get { return _overrrideParameters ?? ValoresPadrao; } }
    public GameObject StandingOn { get; private set; }
    public Vector3 PlataformVelocity { get; private set; }

    private Vector2 _velocidade;
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _boxCollider;
    private ControllerParameters2D _overrrideParameters;
    private float _jumpIn;
    private GameObject _lastStandingOn;

    private Vector3
        _activeGlobalPlataformPoint,
        _activeLocalPlataformPoint;

    private float
        _verticalDistanceBetweenRays,
        _horizontalDistanceBetweenRays;

    private Vector3
        _raycastTopLeft,
        _raycastBottomRight,
        _raycastBottomLeft;

    public void Awake()
    {
        HandleCollisions = true;
        State = new ControllerState2D();
        _transform = transform;
        _localScale = transform.localScale;
        _boxCollider = GetComponent<BoxCollider2D>();

        var collidionWidth = _boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinWidth);
        _horizontalDistanceBetweenRays = collidionWidth / (LimitesVerticais - 1);

        var collidionHeigth = _boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
        _verticalDistanceBetweenRays = collidionHeigth / (LimitesHorizontais - 1);
    }

    public void Addforce(Vector2 force)
    {
        _velocidade += force;
    }

    public void SetForce(Vector2 force)
    {
        _velocidade = force;
    }

    public void SetHorizontalForce (float x)
    {
        _velocidade.x = x;
    }
    public void Setverticalforce (float y)
    {
        _velocidade.y = y;
    }

    public void Jump()
    {
        Addforce(new Vector2(0, Parameters.JumpMagnitude));
        _jumpIn = Parameters.FrequenciaPulo;

    }

    public void LateUpdate()
    {
        _jumpIn = Time.deltaTime;
        _velocidade.y += Parameters.Gravidade * Time.deltaTime;
        Move(Velocidade * Time.deltaTime);
    }

    private void Move(Vector2 deltaMoviment)
    {
        var wasGrounded = State.ColidindoBaixo;
        State.Reset();
        
        if (HandleCollisions)
        {
            HandlePlataforms();
            CalculateRaysOrigins();

            if (deltaMoviment.y < 0 && wasGrounded)
                HandleInclinacaoVertical(ref deltaMoviment);

            if (Mathf.Abs(deltaMoviment.x) > 0.001f)
                MoverHorizontal(ref deltaMoviment);

            MoverVertical(ref deltaMoviment);

            CorrectHorizontalPlacement(ref deltaMoviment, true);
            CorrectHorizontalPlacement(ref deltaMoviment, false);
        }

        _transform.Translate(deltaMoviment, Space.World);

        if (Time.deltaTime > 0)
            _velocidade = deltaMoviment / Time.deltaTime;

        _velocidade.x = Mathf.Min(_velocidade.x, Parameters.VelocidadeMax.x);
        _velocidade.y = Mathf.Min(_velocidade.y, Parameters.VelocidadeMax.y);

        if (State.MovendoInclinadoCima)
            _velocidade.y = 0;

        if (StandingOn != null)
        {
            _activeGlobalPlataformPoint = transform.position;
            _activeLocalPlataformPoint = StandingOn.transform.InverseTransformPoint(transform.position);

            Debug.DrawLine(transform.position, _activeGlobalPlataformPoint);
            Debug.DrawLine(transform.position, _activeLocalPlataformPoint);

            if (_lastStandingOn != StandingOn)
            {
                if (_lastStandingOn != null)
                {
                    _lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
                }

                StandingOn.SendMessage("ControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
                _lastStandingOn = StandingOn;
            }
            else if (StandingOn != null)
                StandingOn.SendMessage("ControllerStay2D", this, SendMessageOptions.DontRequireReceiver);


        }
        else if (_lastStandingOn != null)
        {
            _lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
            _lastStandingOn = null;
        }
    }

    private void HandlePlataforms()
    {
        if (StandingOn != null)
        {
            var newGlobalPlataformPoint = StandingOn.transform.TransformPoint(_activeLocalPlataformPoint);
            var moveDistance = newGlobalPlataformPoint - _activeGlobalPlataformPoint;

            if (moveDistance != Vector3.zero)
                transform.Translate(moveDistance, Space.World);

            PlataformVelocity = (newGlobalPlataformPoint - _activeGlobalPlataformPoint) / Time.deltaTime;
        }
        else
            PlataformVelocity = Vector3.zero;

        StandingOn = null;
    }

    private void CorrectHorizontalPlacement(ref Vector2 deltaMoviment, bool isRight)
    {
        var halfWidth = (_boxCollider.size.x * _localScale.x) / 2f;
        var rayOrigin = isRight ? _raycastBottomRight : _raycastBottomLeft;

        if (isRight)
            rayOrigin.x -= (halfWidth - SkinWidth);
        else
            rayOrigin.x += (halfWidth - SkinWidth);

        var rayDirection = isRight ? Vector2.right : -Vector2.right;
        var offset = 0f;

        for (var i=1; i< LimitesHorizontais -1; i++)
        {
            var rayVector = new Vector2(deltaMoviment.x +  rayOrigin.x, deltaMoviment.y + rayOrigin.y + (i * _verticalDistanceBetweenRays));
            //Debug.DrawRay(rayVector, rayDirection * halfWidth, isRight ? Color.cyan : Color.magenta);
            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, halfWidth, PLataformaMark);
            if (!raycastHit)
                continue;

            offset = isRight ? ((raycastHit.point.x - _transform.position.x) - halfWidth) : (halfWidth - (_transform.position.x - raycastHit.point.x));
        }
        deltaMoviment.x += offset;
    }

    private void CalculateRaysOrigins()
    {
        var size = new Vector2(_boxCollider.size.x * Mathf.Abs(_localScale.x), _boxCollider.size.y * Mathf.Abs(_localScale.y)) / 2 ;
        var center = new Vector2(_boxCollider.center.x * _localScale.x, _boxCollider.center.y * _localScale.y) ;

        _raycastTopLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y + size.y - SkinWidth);
        _raycastBottomRight = _transform.position + new Vector3(center.x + size.x - SkinWidth, center.y - size.y + SkinWidth);
        _raycastBottomLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y - size.y + SkinWidth);
    }

    private void MoverHorizontal(ref Vector2 deltaMoviment)
    {
        var isGoingRight = deltaMoviment.x > 0;
        var rayDistance = Mathf.Abs(deltaMoviment.x) + SkinWidth;
        var raydirection = isGoingRight ? Vector2.right : -Vector2.right;
        var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;


        for (var i = 0; i < LimitesHorizontais; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * _verticalDistanceBetweenRays));
            Debug.DrawRay(rayVector, raydirection * rayDistance, Color.red);

            var rayCastHit = Physics2D.Raycast(rayVector, raydirection, rayDistance, PLataformaMark);
            if (!rayCastHit)
                continue;

            if (i == 0 && HandleInclinacaoHorizontal(ref deltaMoviment, Vector2.Angle(rayCastHit.normal, Vector2.up), isGoingRight))
                break;

            deltaMoviment.x = rayCastHit.point.x - rayVector.x;
            rayDistance = Mathf.Abs(deltaMoviment.x);

            if (isGoingRight)
            {
                deltaMoviment.x -= SkinWidth;
                State.ColidindoDir = true;
            }
            else
            {
                deltaMoviment.x += SkinWidth;
                State.ColidindoEsq =  true;
            }

            if (rayDistance < SkinWidth + 0.001f)
                break;
        }
    }

    private void MoverVertical(ref Vector2 deltaMoviment)
    {
        var isGoingUp = deltaMoviment.y > 0;
        var rayDistance = Mathf.Abs(deltaMoviment.y) + SkinWidth;
        var raydirection = isGoingUp ? Vector2.up : -Vector2.up;
        var rayOrigin = isGoingUp ? _raycastTopLeft : _raycastBottomLeft;

        rayOrigin.x += deltaMoviment.x;

        var standingOnDistance = float.MaxValue;

        for (var i = 0; i < LimitesVerticais; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + (i * _horizontalDistanceBetweenRays), rayOrigin.y);
            Debug.DrawRay(rayVector, raydirection * rayDistance, Color.red);

            var rayCastHit = Physics2D.Raycast(rayVector, raydirection, rayDistance, PLataformaMark);
            if (!rayCastHit)
                continue;

            deltaMoviment.y = rayCastHit.point.y - rayVector.y;
            rayDistance = Mathf.Abs(deltaMoviment.y);

            if (!isGoingUp)
            {
                var verticalDistanceToHit = _transform.position.y - rayCastHit.point.y;
                if (verticalDistanceToHit < standingOnDistance)
                {
                    standingOnDistance = verticalDistanceToHit;
                    StandingOn = rayCastHit.collider.gameObject; 
                }
                
            }

            deltaMoviment.y = rayCastHit.point.y - rayVector.y;
            rayDistance = Mathf.Abs(deltaMoviment.y);

            if (isGoingUp)
            {
                deltaMoviment.y -= SkinWidth;
                State.ColidindoCima = true;
            }
            else
            {
                deltaMoviment.y += SkinWidth;
                State.ColidindoBaixo = true;
            }

            if (!isGoingUp && deltaMoviment.y > .0001f)
                State.MovendoInclinadoCima = true;

            if (rayDistance < SkinWidth + .0001f)
                break;
        }
        
    }

    private void HandleInclinacaoVertical(ref Vector2 deltaMoviment)
    {
        var center = (_raycastBottomLeft.x + _raycastBottomRight.x) / 2;
        var direction = -Vector2.up;

        var slopeDistance = LimiteTangenteInclinacao * (_raycastBottomRight.x - center);
        var slopeRayVector = new Vector2(center, _raycastBottomLeft.y);

        Debug.DrawRay(slopeRayVector, direction * slopeDistance, Color.yellow);
        var rayCastHit = Physics2D.Raycast(slopeRayVector, direction, slopeDistance, PLataformaMark);
        if (!rayCastHit)
            return;
        
        var isMovingDownSlope = Mathf.Sign(rayCastHit.normal.x) == Mathf.Sign(deltaMoviment.x);
        if (!isMovingDownSlope)
            return;

        var angle = Vector2.Angle(rayCastHit.normal, Vector2.up);
        if (Mathf.Abs(angle) < 0.001)
            return;

        State.MovendoInclinadoBaixo = true;
        State.AnguloInclinacao = angle;
        deltaMoviment.y = rayCastHit.point.y - slopeRayVector.y;
    }

    private bool HandleInclinacaoHorizontal(ref Vector2 deltaMoviment, float angle, bool isGoingRight)
    {
        if (Mathf.RoundToInt(angle) == 90)
            return false;  

        if (angle > Parameters.AlguloLimite)
        {
            deltaMoviment.x = 0;
            return true;
        }

        if (deltaMoviment.y > .07f)
            return true;

        deltaMoviment.x += isGoingRight ? -SkinWidth : SkinWidth;
        deltaMoviment.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMoviment.x);
        State.MovendoInclinadoCima= true;
        State.ColidindoBaixo = true;
        return true;    
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var parameters = other.gameObject.GetComponent<ControllerPhsyicsVolume2D>();
        if (parameters == null)
            return;

        _overrrideParameters = parameters.Parameters;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        var parameters = other.gameObject.GetComponent<ControllerPhsyicsVolume2D>();
        if (parameters == null)
            return;

        _overrrideParameters = null;
    }
}