using UnityEngine;
using System.Collections;
using System.Linq;

public class ObjectsPointToGet : MonoBehaviour, IPlayerRespawnListener {
    public GameObject Effect;
    public int PointsToAdd = 10;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null)
            return;

        GameManager.Instance.AddPoints(PointsToAdd);
        Instantiate(Effect, transform.position, transform.rotation);

        gameObject.SetActive(false);

    }

    public void OnPlayerRespawnInThisCheckPoint(CheckPoint checkPoint, Player player)
    {
        gameObject.SetActive(true);
    }
}
