using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform Player;
    public Transform[] ObjectsToMove;
    public float ObjectsOffsetX;
    public float ObjectsOffsetY;

    public Vector2 Margin, Smoothing;

    public BoxCollider2D Bounds;

    private Vector3 _min, _max;

    public bool IsFollowing { get; set; }

    public void Start()
    {
        _min = Bounds.bounds.min;
        _max = Bounds.bounds.max;
        IsFollowing = true;
    }

    public void Update()
    {
        var x = transform.position.x;
        var y = transform.position.y;

        if (IsFollowing)
        {
            if (Mathf.Abs(x - Player.position.x) > Margin.x)
                x = Mathf.Lerp(x, Player.position.x, Smoothing.x * Time.deltaTime);

            if (Mathf.Abs(y - Player.position.y) > Margin.y)
                y = Mathf.Lerp(y, Player.position.y, Smoothing.y * Time.deltaTime);
        }

        var cameraHalfWidth = camera.orthographicSize * ((float)Screen.width / Screen.height);
        x = Mathf.Clamp(x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
        y = Mathf.Clamp(y, _min.y + camera.orthographicSize, _max.y - camera.orthographicSize);


        for (int i = 0; i < ObjectsToMove.Length; i++ )
        {
            var transformObj = ObjectsToMove[i];
            transformObj.transform.position = new Vector3(x + ObjectsOffsetX, y + ObjectsOffsetY,0);
        }

        transform.position = new Vector3(x, y, transform.position.z);
    }

    
}
