using System.Collections;
using UnityEngine;

public class StartLevelActions : BaseLevelActions
{
    public Player Valdemir;
    public Collider2D TimeMachine;
    public NPC_Speak LeoNPC;
    private StartLevelActions _instance;

    private static bool SpeakedWithLeo;

    public void Start()
    {
        Valdemir.CanInput = false;
        TimeMachine.enabled = false;
        SpeakedWithLeo = false;
        _instance = (StartLevelActions)Instance;
        StartCoroutine(Wait(12));
    }

    public IEnumerator Wait(int sec)
    {
        yield return new WaitForSeconds(sec);
        Valdemir.CanInput = true;
    }


    public override void OnTriggerSpeaked()
    {
        TimeMachine.enabled = true;
        SpeakedWithLeo = true;
    }

}
