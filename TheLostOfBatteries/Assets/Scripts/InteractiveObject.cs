using UnityEngine;
using System.Linq;

public class InteractiveObject : MonoBehaviour
{
    public bool IsInteracting { get; set; }
    public bool IsNearToNpc { get; set; }

    private volatile Player _playerOwn;
    public void LateUpdate()
    {
        if (IsInteracting || _playerOwn == null || !IsNearToNpc || !_playerOwn.Focused)
            return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            IsInteracting = true;
          
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

}

