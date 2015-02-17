using System.Collections;
using UnityEngine;

public class BallonText : MonoBehaviour
{

    private static readonly GUISkin Skin = Resources.Load<GUISkin>("Game Skin");
    public static BallonText Show(string text, string style, IBallonTextPositioner positioner)
    {
        var go = new GameObject("Ballon Text");
        var ballonText = go.AddComponent<BallonText>();
        ballonText.Style = Skin.GetStyle(style);
        ballonText._positioner = positioner;
        ballonText._content = new GUIContent(text+"\n\n\n\n");
        
        return ballonText;
    }

    private GUIContent _content;
    private IBallonTextPositioner _positioner;

    public string Text { get { return _content.text; } set { _content.text = value; } }
    public GUIStyle Style { get; set; }
    private Vector2 scroll;
    private Vector2 position;
    private Vector2 contentSize;

    public void DrawBallon(Vector2 position, Vector2 contentSize)
    {
        GUILayout.BeginArea(new Rect(position.x, position.y, contentSize.x, contentSize.y), Skin.GetStyle("BallonSpeak"));
        scroll = GUILayout.BeginScrollView(scroll, GUIStyle.none, GUIStyle.none, GUILayout.Width(contentSize.x), GUILayout.Height(contentSize.y));

        {

             GUILayout.Label(_content);

        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    public void OnGUI()
    {
        contentSize = Style.CalcSize(_content);
        if (!_positioner.GetPosition(ref position, ref scroll, contentSize))
        {
            Destroy(gameObject);
        }
        DrawBallon(position, contentSize);
    }



}
