using UnityEngine;
using System.Collections;
using System.Linq;

public class PackHealth : MonoBehaviour{

    public GameObject Effect;
    public AudioClip SoundEffect;

    public void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player == null)
            return;

        if (!player.Itens.Add(tag))
            return;

        if (SoundEffect != null)
            AudioSource.PlayClipAtPoint(SoundEffect, Vector3.zero);

        if (Effect != null)
         Instantiate(Effect, transform.position, transform.rotation);

        gameObject.SetActive(false);

        FloatingText.Show("Vida, uuhuuu!!", "PointObjectText",
            new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
    }

}
