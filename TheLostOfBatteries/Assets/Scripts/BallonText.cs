using System.Collections;
using UnityEngine;
using System.Linq;

public class BallonText : MonoBehaviour
{

    private static readonly GUISkin Skin = Resources.Load<GUISkin>("Game Skin");
    public static BallonText Show(string text, string style, IBallonTextPositioner positioner, NPC_Speak npcSpeaker= null)
    {
        var go = new GameObject("Ballon Text");
        var ballonText = go.AddComponent<BallonText>();
        ballonText.Style = Skin.GetStyle(style);
        ballonText.StyleArea = Skin.GetStyle("BallonSpeak");
        ballonText._positioner = positioner;
        ballonText._content = new GUIContent(text);
        ballonText._npc_speaker = npcSpeaker;
        return ballonText;
    }

    private GUIContent _content;
    private IBallonTextPositioner _positioner;
    private Vector2 scroll;
    private NPC_Speak _npc_speaker;

    public string Text { get { return _content.text; } set { _content.text = value; } }
    public GUIStyle Style { get; set; }
    public GUIStyle StyleArea { get; set; }
    
    private float y = -30;

    public void OnGUI()
    {
        var position = new Vector2();
        var contentSize = Style.CalcSize(_content);
        if (!_positioner.GetPosition(ref position, ref scroll, contentSize))
        {
            Destroy(gameObject);

            if (_npc_speaker != null)
                _npc_speaker.IsSpeaking = false;

            return;
        }

        GUILayout.BeginArea(new Rect(position.x, position.y, contentSize.x, contentSize.y), StyleArea);
        {
            GUI.Label(new Rect(0, -y, contentSize.x, contentSize.y), _content, Style);
        }
        GUILayout.EndArea();

        y += Time.deltaTime * 4;
    }



}
