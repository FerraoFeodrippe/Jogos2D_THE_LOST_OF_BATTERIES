using UnityEngine;
using System.Linq;
using System.Collections;

public class UpRope : MonoBehaviour
{
    private Player[] Players;
    // public Animator PlayerAnimator;



    public void Awake()
    {
        Players = FindObjectsOfType<Player>();

        foreach (var player in Players)
        {
            StartCoroutine(HandleUp(player));
        }

    }

    //public void Update()
    //{



    //    Player.GetComponent<CharacterController2D>().enabled = false;

    //}

    public void OnTriggerEnter2D(Collider2D other)
    {
        //   _canUp = true;
        //  _upRope = true;
        //_isUping = transform.position.y > other.transform.position.y;

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        //_canUp = false;
        //_upRope = false;
        //Player.GetComponent<CharacterController2D>().enabled = true;
        //PlayerAnimator.SetBool("Up 1", false);
    }

    public IEnumerator HandleUp(Player Player)
    {
        var _upRope = false;
        var _canUp = false;
        var PlayerAnimator = Player.GetComponentInChildren<Animator>();

        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (!Player.Focused)
                continue;

            _canUp = Player.transform.position.x - transform.position.x < 0.1 && Player.transform.position.x - transform.position.x > -0.1;


            if (!_canUp)
                continue;

            PlayerAnimator.SetBool("Up 1", true);

            if (Input.GetKey(KeyCode.I))
            {
                PlayerAnimator.SetTrigger("Up");
                _upRope = Player.transform.position.y - Player.collider2D.bounds.size.y / 2f
                    < transform.position.y + transform.collider2D.bounds.size.y / 2f - 0.2f;
                Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + transform.collider2D.bounds.size.y / 2f, 0), Color.blue);
                Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + Time.deltaTime, 0);

            }

            if (Input.GetKey(KeyCode.K))
            {
                PlayerAnimator.SetTrigger("Up");
                _upRope = Player.transform.position.y - Player.collider2D.bounds.size.y / 2f
                     > transform.position.y - transform.collider2D.bounds.size.y / 2f;
                Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - transform.collider2D.bounds.size.y / 2f, 0), Color.blue);
                Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - Time.deltaTime, 0);
            }

            Player.GetComponent<CharacterController2D>().enabled = !_upRope;
            //if (Input.GetKeyDown(KeyCode.X))
            //    _upRope = false;
            //Player.CanInput = !_upRope;


            PlayerAnimator.SetBool("Up 1", _upRope);
        }

    }

}

