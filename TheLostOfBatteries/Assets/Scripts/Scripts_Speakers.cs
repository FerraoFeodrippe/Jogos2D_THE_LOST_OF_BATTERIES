using System;


public class Scripts_Speakers
{
    private static readonly string[] LeoTexts = 
    {
     /*0*/    "Valdermir não toque em nada. Estou trabalhando. Jajá eu falo com você.", 
     /*1*/    "Eu já disse que estou ocupado, oras,agora dê o fora daqui!!!" 
    };

    public static string GetLeoTextAt(int pos)
    {
        return LeoTexts[pos];
    }
}
