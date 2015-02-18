using System;


public class Scripts_Speakers
{
    private static readonly string[] Texts = 
    {
#region LeoTexts
     /*0*/    "Valdermir não toque em nada. Estou trabalhando. Jajá eu falo com você.", 
     /*1*/    "Eu já disse que estou ocupado, oras,agora dê o fora daqui!!!"
#endregion
     
    };

    public static string GetTextAt(int pos)
    {
        return Texts[pos];
    }
}
