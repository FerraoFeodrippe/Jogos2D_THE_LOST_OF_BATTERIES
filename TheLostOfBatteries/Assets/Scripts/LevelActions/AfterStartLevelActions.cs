using System.Collections;
using UnityEngine;

public class AfterStartLevelActions : BaseLevelActions
{
    public Player Valdemir;
    private CharacterController2D _controllerValdemir;
    private CharacterController2D _controllerLeoNPC;
    public NPC_Speak LeoNPC;
    public AudioClip PunchAudio;

    private volatile bool PunchedLeo;
    private volatile bool PlayerCollided;

    public void Start()
    {
        Valdemir.CanInput = false;
        BallonText.Show(Scripts_Speakers.GetTextAt(2), "LabelBallonSpeak",
            new SpeakerBallon(Camera.main, LeoNPC.transform, Scripts_Speakers.GetTextAt(2).Length * 0.1f));
        _controllerValdemir = Valdemir.GetComponent<CharacterController2D>();
        _controllerLeoNPC = LeoNPC.GetComponent<CharacterController2D>();
        StartCoroutine(Bot());

    }

    public IEnumerator Bot()
    {
        yield return new WaitForSeconds(2.5f);
        LeoNPC.Animator.SetFloat("Speed", 40);
        _controllerLeoNPC.SetHorizontalForce(-25);

        while (!PunchedLeo)
        {
            yield return new WaitForSeconds(0.1f);
        }

        BallonText.Show(Scripts_Speakers.GetTextAt(3), "LabelBallonSpeak",
    new SpeakerBallon(Camera.main, LeoNPC.transform, Scripts_Speakers.GetTextAt(3).Length * 0.2f));
        Valdemir.Animator.SetBool("IsDown", true);
        yield return new WaitForSeconds(4);
        

        BallonText.Show(Scripts_Speakers.GetTextAt(4), "LabelBallonSpeak",
new SpeakerBallon(Camera.main, LeoNPC.transform, Scripts_Speakers.GetTextAt(4).Length * 0.1f));

        yield return new WaitForSeconds(3);

        while (!PlayerCollided)
        {
            _controllerValdemir.SetHorizontalForce(18);
            yield return new WaitForSeconds(0.05f);
        }

        BallonText.Show("AGORA COMEÇA A PUTARIA, AIAI PAPAI.", "LabelBallonSpeak",
new SpeakerBallon(Camera.main, LeoNPC.transform, 5f));

        yield break;
    }

    public override void PlayerOnCollider(Collider2D other)
    {
        if (PlayerCollided || !PunchedLeo)
            return;
        Debug.Log("Entrou");
        _controllerValdemir.SetHorizontalForce(-10);
        PlayerCollided = true;

    }

    public override void NPCOnCollider(Collider2D other)
    {
        if (PunchedLeo)
            return;

        LeoNPC.Animator.SetFloat("Speed", 0);
        _controllerLeoNPC.SetHorizontalForce(0);
        AudioSource.PlayClipAtPoint(PunchAudio, Vector3.zero);
        LeoNPC.Animator.SetTrigger("Punch");
        Valdemir.Animator.SetTrigger("Dead");
        Valdemir.Animator.SetFloat("Speed", 100);
        _controllerValdemir.SetHorizontalForce(-100);

        LeoNPC.Animator.SetBool("TaBebendo", false);
        PunchedLeo = true;
        //_controllerLeoNPC
    }



}
