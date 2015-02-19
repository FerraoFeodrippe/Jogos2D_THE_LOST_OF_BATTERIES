using System.Collections;
using UnityEngine;

public class StartLevelActions : BaseLevelActions
{
    public Player Valdemir;
    private CharacterController2D _controllerValdemir;
    public Collider2D TimeMachine;
    public NPC_Speak LeoNPC;
    private StartLevelActions _instance;

    private static bool SpeakedWithLeo;

    public void Start()
    {
        BallonText.Show(Scripts_Speakers.GetTextAt(5), "LabelBallonSpeak",
            new SpeakerBallon(Camera.main, Valdemir.transform, Scripts_Speakers.GetTextAt(2).Length * 0.3f));
        _controllerValdemir = Valdemir.GetComponent<CharacterController2D>();
        Valdemir.CanInput = false;
        TimeMachine.enabled = false;
        SpeakedWithLeo = false;
        _instance = (StartLevelActions)Instance;
        StartCoroutine(Wait(8));
    }

    public IEnumerator Wait(int sec)
    {
        yield return new WaitForSeconds(sec);
        Valdemir.CanInput = true;
    }

    public override void DoAction(string nome)
    {
        StartCoroutine(nome);
    }


    public override void OnTriggerSpeaked()
    {
        TimeMachine.enabled = true;
        SpeakedWithLeo = true;
    }

    public override void OnTriggerInteracted()
    {
        Valdemir.CanInput = true;
        GUI.enabled = false;
        Application.LoadLevel("ComputerScreen");
    }

    public override void SetInputPlayer(bool pode)
    {
        Valdemir.CanInput = pode;
        if (!pode)
            _controllerValdemir.SetHorizontalForce(-1);
    }

    public IEnumerator Time_Machine()
    {
        Valdemir.CanInput = false;
        yield break;
    }

}
