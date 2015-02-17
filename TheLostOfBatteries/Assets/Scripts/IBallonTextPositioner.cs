using UnityEngine;

public interface IBallonTextPositioner
{
    bool GetPosition(ref Vector2 position, ref Vector2 posScroll, Vector2 size);
}
