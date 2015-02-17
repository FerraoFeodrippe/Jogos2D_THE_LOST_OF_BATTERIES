using UnityEngine;
using System.Collections;

public class SpeakerBallon : IBallonTextPositioner {

    private readonly Camera _camera;
    private readonly Vector3 _worldPosition;
    private float _timeToLive;
    private float _yOffset;
    private Transform _gameObject;
    //private Vector2 _tempScroll;

    public SpeakerBallon(Camera camera, Transform gameObject, float timeToLive)
    {
        _camera = camera;
        _worldPosition = gameObject.position;
        _timeToLive = timeToLive;
        _gameObject = gameObject;
        _yOffset =Screen.height/10;
        //_tempScroll = new Vector2(0, -Mathf.Infinity);

    }


    public bool GetPosition(ref Vector2 position, ref Vector2 posScroll, Vector2 size)
    {
        if ((_timeToLive -= Time.deltaTime) <= 0)
        {
            return false;
        }

        var screenPosition = _camera.WorldToScreenPoint(_gameObject.position);
        position.x = screenPosition.x - (size.x/2);
        position.y = Screen.height - screenPosition.y - (size.y) - _yOffset;

        return true;
    }
}
