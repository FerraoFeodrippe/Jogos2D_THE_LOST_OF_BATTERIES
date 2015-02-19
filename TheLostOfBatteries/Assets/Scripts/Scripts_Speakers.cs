using System;


public class Scripts_Speakers
{
    private static readonly string[] Texts = 
    {

#region LeoTexts
     /*0*/    "Valdermir não toque em nada. Estou trabalhando. Jajá eu falo com você.", 
     /*1*/    "Eu já disse que estou ocupado, oras,agora dê o fora daqui!!!",
     /*2*/    "PUTA MERDA, O QUE VOCÊ FEZ? SAIA DAQUI AGORA!!!!",
     /*3*/    "Como é que esta merda foi acontecer? Pqp.",
     /*4*/    "EU DISSE PRA SAIR AGORA, NÃO VENHA ME AJUDAR, APENAS CORRA!!!",
#endregion

#region ValdemirTexts
     /*5*/   @"Ola, meu nome é valdemir, quer ser meu amigo? Sou sobrinho de Leo, 
lo phodão.Gostaria de lembrar a todos que amo Bruna e que já dei para túlio. Ele adorou, por sinal.", 
#endregion
     
    };

    public static string GetTextAt(int pos)
    {
        return Texts[pos];
    }
}
