using UnityEngine;
using System.Linq;

public class NPC_Speak : MonoBehaviour
{
    public int []NumberSpeakSequence;
    public bool IsSpeaking { get; set; }
    public bool IsNearToNpc { get; set; }
    public bool HaveSpeaked { get; set; }
    public Animator Animator { get; set; }
 
    public void Awake()
    {
        Animator = GetComponent<Animator>();
    }


    private int _sequenceActual;
    private volatile Player _playerOwn;
    public void LateUpdate()
    {
        if (IsSpeaking || _playerOwn == null || !IsNearToNpc || !_playerOwn.Focused)
            return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            IsSpeaking= true;
            BaseLevelActions.Instance.DoAction(gameObject.name);
            var fala = Scripts_Speakers.GetTextAt(NumberSpeakSequence[_sequenceActual]);
            if (_sequenceActual +1 < NumberSpeakSequence.Count())
                _sequenceActual++;
            BallonText.Show(fala, "LabelBallonSpeak",
                new SpeakerBallon(Camera.main, transform, fala.Length * 0.2f), this);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        BaseLevelActions.Instance.NPCOnCollider(other);
            
        _playerOwn = other.GetComponent<Player>();
        if (_playerOwn == null || IsNearToNpc)
            return;
        _playerOwn.IsNearToNpc = true;
     

        IsNearToNpc = true;

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        _playerOwn = other.GetComponent<Player>();
        if (_playerOwn == null || !IsNearToNpc)
            return;
        _playerOwn.IsNearToNpc = false ;
        IsNearToNpc = false;

    }

}

