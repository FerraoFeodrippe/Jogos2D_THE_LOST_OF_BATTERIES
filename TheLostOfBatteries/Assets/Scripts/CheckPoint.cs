using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private List<IPlayerRespawnListener> _listeners;



    public void Awake()
    {
        _listeners = new List<IPlayerRespawnListener>();

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

        foreach(var listener in _listeners)
        {
            listener.OnPlayerRespawnInThisCheckPoint(this, player);
        }
    }

    public void AssingObjectToCheckPoint(IPlayerRespawnListener listener)
    {
        _listeners.Add(listener);
    }
}

