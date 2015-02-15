using UnityEngine;

public class FloatingText : MonoBehaviour {

    private static readonly GUISkin Skin = Resources.Load<GUISkin>("Game Skin");

    public static FloatingText Show(string text, string style, IFloatingTextPositioner positioner)
    {
        var go = new GameObject("Floating Text");
        var floatingText = go.AddComponent<FloatingText>();
        floatingText.Style = Skin.GetStyle(style);
        floatingText._positioner = positioner;
        floatingText._content = new GUIContent(text);
        return floatingText;
    }

    private GUIContent _content;
    private IFloatingTextPositioner _positioner;

    public string Text { get { return _content.text; } set { _content.text = value; } }
    public GUIStyle Style { get; set; }

    public void OnGUI()
    {
        var position = new Vector2();
        var contentSize = Style.CalcSize(_content);
        if (!_positioner.GetPosition(ref position, _content, contentSize))
        {
            Destroy(gameObject);
            return;
        }

        GUI.Label(new Rect(position.x, position.y, contentSize.x, contentSize.y), _content, Style);

    }

}
