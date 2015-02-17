using UnityEngine;

public class FollowObject : MonoBehaviour {

    public Vector2 Offset;
    public Transform Following;

    public void Update()
    {
        transform.position = new Vector3(Following.position.x, Following.position.y, 0 )
        + (Vector3)Offset;
    }
}
