using System.Collections;
using UnityEngine;

public class Level2Actions : BaseLevelActions
{
    public Player Valdemir;
    public Player Leo;
    //public GameObject Armor1;
    private CharacterController2D _controllerValdemir;
    private CharacterController2D _controllerLeo;

    private volatile bool PunchedLeo;
    private volatile bool PlayerCollided;

    public void Start()
    {
        Valdemir.CanInput = true;
        Leo.CanInput = true;

    }

    public override void DoAction(string nome)
    {
        StartCoroutine(nome);
    }

    public IEnumerator Armor()
    {

        yield return new WaitForSeconds(2f);

        //Armor1.SetActive(false);

        yield break;
    }

    public override void OnTriggerInteracted()
    {

    }



    public override void PlayerOnCollider(Collider2D other)
    {

    }

    public override void NPCOnCollider(Collider2D other)
    {

    }



}
