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
        StartCoroutine(PlayerHitcheckPointCo(LevelManager.Instance.CurrentTimebonus));
    }

    public IEnumerator PlayerHitcheckPointCo(int bonus)
    {
        FloatingText.Show("Checkpoint!", "CheckPointText", new CenteredTextPosition(.5f));
        yield return new WaitForSeconds(.5f);
        FloatingText.Show(string.Format("+{0} time bonus!", bonus), "CheckPointText", new CenteredTextPosition(.5f));
    }

    public void PlayerLeftCheckPoint()
    {

    }

    public void SpawnPlayer(Player player, float offSet = 0)
    {
        player.RespawnAt(transform, offSet);

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

