using UnityEngine;
using System.Collections;

public class SpeakerBallon : IBallonTextPositioner {

    private readonly Camera _camera;
    private readonly Vector3 _worldPosition;
    private float _timeToLive;
    private float _yOffset;
    private Transform _gameObject;
    private Vector2 _tempScroll;

    public SpeakerBallon(Camera camera, Transform gameObject, float timeToLive)
    {
        _camera = camera;
        _worldPosition = gameObject.position;
        _timeToLive = timeToLive;
        _gameObject = gameObject;
        _yOffset =50;
        _tempScroll = new Vector2(0, -Mathf.Infinity);

    }


    public bool GetPosition(ref Vector2 position, ref Vector2 posScroll, Vector2 size)
    {
        if ( Mathf.Abs(posScroll.y - _tempScroll.y) <= 0.055f ) 
            return false;

        _tempScroll = new Vector2(posScroll.x, posScroll.y); 
          posScroll +=  4* Vector2.up * Time.deltaTime;

        var screenPosition = _camera.WorldToScreenPoint(_gameObject.position);
        position.x = screenPosition.x - (size.x/2);
        position.y = Screen.height - screenPosition.y - (size.y) - _yOffset;

        return true;
    }
}
