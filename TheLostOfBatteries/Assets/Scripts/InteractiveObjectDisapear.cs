using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class InteractiveObjectDisapear : InteractiveObject
{
    public GameObject ObjectToDisappear;
    
    public void LateUpdate()
    {
        if (IsInteracting || _playerOwn == null || !IsNearToNpc || !_playerOwn.Focused)
            return;

        if (Input.GetKeyDown(KeyCode.X))
        {
                        //   BaseLevelActions.Instance.SetInputPlayer(false);
			transform.Rotate(0, Input.GetAxis("Horizontal")*1*Time.deltaTime, 0,Space.World);
            IsInteracting = true;
            if (Sound != null)
                AudioSource.PlayClipAtPoint(Sound, Vector3.zero);
            StartCoroutine(OnDisappear());

        }
    }

    public IEnumerator OnDisappear()
    {
        if (Sound != null)
            yield return new WaitForSeconds(Sound.length);
        BaseLevelActions.Instance.OnTriggerInteracted();
        IsInteracting = false;
        IsNearToNpc = false;
        _playerOwn.IsNearToInteractiveObject = false;

        ObjectToDisappear.gameObject.SetActive(false);
    }

}

