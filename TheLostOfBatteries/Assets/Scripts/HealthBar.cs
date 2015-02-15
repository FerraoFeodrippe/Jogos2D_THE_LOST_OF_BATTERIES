using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
    
    public Player Player;
    public Transform ForegroundSprite;
    public SpriteRenderer ForegroundRederender;
    public Color MaxHealthColor = new Color(.6f, .6f, .1f, .7f);
    public Color MinHealthColor = new Color(.6f, .1f, .1f, .7f);
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var healthPercent = Player.Health / (float)Player.MaxHealth;

        ForegroundSprite.localScale = new Vector3(healthPercent, 1, 1);
        ForegroundRederender.color = Color.Lerp(MaxHealthColor, MinHealthColor, healthPercent);
	}
}
