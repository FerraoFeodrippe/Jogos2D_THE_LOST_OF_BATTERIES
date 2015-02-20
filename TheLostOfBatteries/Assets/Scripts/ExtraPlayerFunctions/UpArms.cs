using UnityEngine;

public class UpArms:MonoBehaviour
{
    public Player Player;
    public Animator PlayerAnimator;
    public GameObject UpArm;

    private bool _upArms;

    public void Update()
    {
        if (!Player.Focused)
            return;

        if (Input.GetKeyDown(KeyCode.C))
            _upArms = !_upArms;
       //if (_upArms)
       //    Player.gameObject.layer = 14;
       //else
       //    Player.gameObject.layer = 11;
       UpArm.SetActive(_upArms);
        PlayerAnimator.SetBool("LevantaBraços", _upArms);
    }
}

