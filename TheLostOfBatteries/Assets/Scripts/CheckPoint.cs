using System.Collections;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public void Start()
    {

    }

    public void PlayerHitCheckPoint()
    {

    }

    public IEnumerator PlayerHitcheckPointCo(int bonus)
    {
        yield break;
    }

    public void PlayerLeftCheckPoint()
    {

    }

    public void SpawnPlayer(Player player)
    {
        player.RespawnAt(transform);
    }

    public void AssingObjectToCheckPoint()
    {

    }
}

