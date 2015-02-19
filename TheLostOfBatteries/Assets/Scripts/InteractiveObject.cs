using UnityEngine;
using System.Linq;
using System.Collections;

public class InteractiveObject : MonoBehaviour
{

    public AudioClip Sound;

    public bool IsInteracting { get; set; }
    public bool IsNearToNpc { get; set; }
    

    private volatile Player _playerOwn;
    public void LateUpdate()
    {
        if (IsInteracting || _playerOwn == null || !IsNearToNpc || !_playerOwn.Focused)
            return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            BaseLevelActions.Instance.DoAction(name);
         //   BaseLevelActions.Instance.SetInputPlayer(false);
            IsInteracting = true;
            if (Sound != null)
                AudioSource.PlayClipAtPoint(Sound, Vector3.zero);
            StartCoroutine(OnAudioEnd());
            
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        _playerOwn = other.GetComponent<Player>();
        if (_playerOwn == null || IsNearToNpc)
            return;

        _playerOwn.IsNearToInteractiveObject = true;
        IsNearToNpc = true;

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        _playerOwn = other.GetComponent<Player>();
        if (_playerOwn == null || !IsNearToNpc)
            return;

        _playerOwn.IsNearToInteractiveObject = false;
        IsNearToNpc = false;

    }

    public IEnumerator OnAudioEnd()
    {
        if (Sound != null)
            yield return new WaitForSeconds(Sound.length);
        BaseLevelActions.Instance.OnTriggerInteracted();
        IsInteracting = false;
    }

}

