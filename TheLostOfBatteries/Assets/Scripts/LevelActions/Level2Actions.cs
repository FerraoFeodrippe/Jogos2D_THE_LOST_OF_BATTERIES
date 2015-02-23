using System.Collections;
using UnityEngine;

public class Level2Actions : BaseLevelActions
{
	public Player Valdemir;
	public Player Leo;
	private CharacterController2D _controllerValdemir;
	private CharacterController2D _controllerLeo;
	
	private volatile bool PunchedLeo;
	private volatile bool PlayerCollided;
	
	public void Start()
	{
		Valdemir.CanInput = true;
		Leo.CanInput = true;
		
	}
	
	
	public override void PlayerOnCollider(Collider2D other)
	{
		
	}
	
	public override void NPCOnCollider(Collider2D other)
	{
		
	}
	
	
	
}
