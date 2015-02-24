using System.Collections;
using UnityEngine;

public class Level1Actions : BaseLevelActions
{
    public Player Valdemir;
    public Player Leo;
    private CharacterController2D _controllerValdemir;
    private CharacterController2D _controllerLeo;
    private Level1Actions _instance;
    
    private volatile bool PunchedLeo;
    private volatile bool PlayerCollided;

    public void Start()
    {
        Valdemir.CanInput = false;
        Leo.CanInput = false;
        _controllerValdemir = Valdemir.GetComponent<CharacterController2D>();
        _controllerLeo = Leo.GetComponent<CharacterController2D>();
        _instance = (Level1Actions)Instance;
        StartCoroutine(Intro());
    }


    public override void PlayerOnCollider(Collider2D other)
    {
       
    }

    public override void NPCOnCollider(Collider2D other)
    {
        
    }

    public IEnumerator Intro()
    {
        yield return new WaitForSeconds(1);

        _controllerValdemir.SetHorizontalForce(30);

            BallonText.Show(Scripts_Speakers.GetTextLevel1At(0), "LabelBallonSpeak",
                new SpeakerBallon(Camera.main,Valdemir.transform, Scripts_Speakers.GetTextAt(0).Length * 0.1f));
            
            yield return new WaitForSeconds(3.5f);


            BallonText.Show(Scripts_Speakers.GetTextLevel1At(1), "LabelBallonSpeak",
                new SpeakerBallon(Camera.main, Leo.transform, Scripts_Speakers.GetTextAt(1).Length * 0.4f));

            yield return new WaitForSeconds(12f);


            BallonText.Show(Scripts_Speakers.GetTextLevel1At(2), "LabelBallonSpeak",
                new SpeakerBallon(Camera.main, Valdemir.transform, Scripts_Speakers.GetTextAt(2).Length * 0.15f));

            yield return new WaitForSeconds(4);


            BallonText.Show(Scripts_Speakers.GetTextLevel1At(3), "LabelBallonSpeak",
                new SpeakerBallon(Camera.main, Leo.transform, Scripts_Speakers.GetTextAt(3).Length * 0.2f));

            _controllerLeo.SetHorizontalForce(10);
            yield return new WaitForSeconds(4);


        yield return new WaitForSeconds(0.5f);

        Valdemir.CanInput = true;
        Leo.CanInput = true;

        yield break;
    }



}
