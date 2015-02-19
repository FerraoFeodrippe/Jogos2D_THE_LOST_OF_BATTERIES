using System.Collections;
using UnityEngine;

public class ComputerScreen : MonoBehaviour
{
    public GUISkin Skin;
    public AudioClip TypingAudio;
    public AudioClip ComputerAudio;

    private string Add = "Initiating . . .";

    private int pos;
    private int safeArray;
    private bool iniciar;
    private bool cursor;
    

    public void Awake()
    {
        Time.timeScale = 1;
        iniciar = false;
        cursor = false;
        StartCoroutine(Loading(0.1f));
        StartCoroutine(WaitIni(4));
        AudioSource.PlayClipAtPoint(ComputerAudio, Vector3.zero);
    }

    public void OnGUI()
    {
        //if (pos < Scripts_Speakers.ScreenText.Length)
          //  Add += Scripts_Speakers.ScreenText[pos++];

        var xS = Screen.width / 25;
        var yS = Screen.height / 25;
        var skin = Skin.GetStyle("ComputerScreen");
        skin.fontSize = yS / 2;
        if (Skin != null)
        {
            GUILayout.BeginArea(new Rect(xS, yS, 17 * xS, 16 * yS));
            {
                if (!iniciar)
                {
                    GUI.Label(new Rect(0, 0, 17 * xS, 16 * yS), Add.Substring(0, Add.Length - (5 - pos)), skin);   

                }
                else if (Add.Length < 1500 && !cursor)
                {
                    
                    Add = Add + Random.Range(0, 2).ToString();
                    GUI.Label(new Rect(0, 0, 17 * xS, 16 * yS), Add, skin);
                }
                else  
                {
                    if (!cursor)
                    {
                        cursor = true;
                        StartCoroutine(Cursor(0.5f));
                        StartCoroutine(WaitForCommand());
                        Add += "\n\n Type your command: \n\n_";
                    }
                      
                    GUI.Label(new Rect(0, 0, 17 * xS, 16 * yS), Add.Substring(0, Add.Length - pos), skin); 
                }
                
            }
            GUILayout.EndArea();
        }
    }

    public IEnumerator WaitIni(float time)
    {
        yield return new WaitForSeconds(time);
        Add = string.Empty;
        iniciar = true;

        yield break;

    }

    public IEnumerator WaitForCommand()
    {
        string _COMMAND =  "DATE COMMAND";
        yield return new WaitForSeconds(5);
        int i = 0;
        while (i< _COMMAND.Length)
        {
            yield return new WaitForSeconds(0.1f);
            AudioSource.PlayClipAtPoint(TypingAudio, Vector3.zero);
            Add = Add.Substring(0, Add.Length - 1) + _COMMAND[i] + "_";
            i++;
        }
        yield return new WaitForSeconds(2);
        Add = "Select the date of... err4r.\n\n Date selected. Have a nice travel Doctor. Travel will start in a few seconds... \n\n _";
        yield return new WaitForSeconds(3);

        Application.LoadLevel("AfterComputerScreen");
    }

    public IEnumerator Cursor(float time)
    {
        while (cursor)
        {
            yield return new WaitForSeconds(time);
            pos = (pos + 1) % 2;
        }

        yield break;

    }

    public IEnumerator Loading(float time)
    {
       while (!iniciar)
       {
           yield return new WaitForSeconds(time);
          pos = ( pos+1) % 6;
       }
       

       yield break;

    }

}
