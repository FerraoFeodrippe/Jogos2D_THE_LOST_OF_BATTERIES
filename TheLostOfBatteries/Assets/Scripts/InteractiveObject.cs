using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class InteractiveObject : MonoBehaviour
{

    public AudioClip Sound;

    public bool IsInteracting { get; set; }
    public bool IsNearToNpc { get; set; }

    protected volatile Player _playerOwn;
    protected volatile IList<Player> players;

    public void Awake()
    {
        players = new List<Player>();
    }

    public void LateUpdate()
    {
        if (IsInteracting || _playerOwn == null || !IsNearToNpc || !_playerOwn.Focused)
            return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            BaseLevelActions.Instance.DoAction(gameObject.name);
            //   BaseLevelActions.Instance.SetInputPlayer(false);
            IsInteracting = true;
            if (Sound != null)
                AudioSource.PlayClipAtPoint(Sound, Vector3.zero);
            StartCoroutine(OnAudioEnd());

        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var playerTemp = other.GetComponent<Player>();
        if (playerTemp == null)
            return;
        if (!players.Any(e => e.Equals(playerTemp)))
            players.Add(playerTemp);
        if (_playerOwn == null)
        {
            _playerOwn = playerTemp;
        }
        else
        {
            _playerOwn.IsNearToInteractiveObject = false;
            IsNearToNpc = false;
            _playerOwn = playerTemp;
        }

        if (!playerTemp.Equals(_playerOwn) || IsNearToNpc)
            return;

        players.Add(playerTemp);

        _playerOwn.IsNearToInteractiveObject = true;
        IsNearToNpc = true;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        var playerTemp = other.GetComponent<Player>();
        if (playerTemp == null)
            return;

        players.Remove(playerTemp);

        if (_playerOwn == null || !playerTemp.Equals(_playerOwn) || !IsNearToNpc)
            return;

        _playerOwn.IsNearToInteractiveObject = false;
        IsNearToNpc = false;

        if (players.Count <= 0)
        {
            _playerOwn = null;
        }
        else
        {
            _playerOwn = players.FirstOrDefault();
            _playerOwn.IsNearToInteractiveObject = true;
            IsNearToNpc = true;
        }


    }

    public IEnumerator OnAudioEnd()
    {
        if (Sound != null)
            yield return new WaitForSeconds(Sound.length);
        BaseLevelActions.Instance.OnTriggerInteracted();
        IsInteracting = false;
    }
}

