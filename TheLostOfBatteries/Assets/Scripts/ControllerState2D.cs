using UnityEngine;
using System.Collections;

public class ControllerState2D{
    public bool ColidindoEsq { get; set; }
    public bool ColidindoDir { get; set; }
    public bool ColidindoCima { get; set; }
    public bool ColidindoBaixo { get; set; }
    public bool MovendoInclinadoBaixo { get; set; }
    public bool MovendoInclinadoCima { get; set; }
    public bool NoChao { get { return ColidindoBaixo; } }
    public float AnguloInclinacao { get; set; }

    public bool Colidindo { get { return ColidindoEsq || ColidindoBaixo || ColidindoDir || ColidindoCima; } }

    public void Reset()
    {
        ColidindoBaixo = 
            ColidindoCima = ColidindoDir = ColidindoEsq = 
            MovendoInclinadoBaixo = MovendoInclinadoCima = false;
        AnguloInclinacao = 0;
    }

    public override string ToString()
    {
        return string.Format("(controle: d:{0} e:{1} c:{2} b:{3} inclinadoBaixo:{4}  inclinadoCima:{5} angulo:{6} )",
            ColidindoDir, ColidindoEsq, ColidindoCima, 
            ColidindoBaixo, MovendoInclinadoBaixo, MovendoInclinadoCima, 
            AnguloInclinacao);
    }

}
