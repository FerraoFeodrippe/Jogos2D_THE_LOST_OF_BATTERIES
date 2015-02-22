using UnityEngine;

public class BoundSlot : MonoBehaviour
{
    public Transform Start;
    public Transform End;

    public void OnDrawGizmos()
    {
        if (Start == null || End == null)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Start.position, new Vector3(End.position.x, Start.position.y, 0));
        Gizmos.DrawLine(new Vector3(End.position.x, Start.position.y, 0), End.position);
        Gizmos.DrawLine(End.position, new Vector3(Start.position.x, End.position.y, 0));
        Gizmos.DrawLine(new Vector3(Start.position.x, End.position.y, 0), Start.position);
    }
}
