using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ControllerParameters2D {

    public enum PuloBehavior
    {
       OnlyOnGround,
       Anywhere,
       Impossible
    }

    public Vector2 VelocidadeMax = new Vector2(float.MaxValue, float.MaxValue);

    [Range(0,90)]
    public float AlguloLimite = 30;
    public float Gravidade = -25;

    public PuloBehavior RestricoesPulo;
    public float FrequenciaPulo = .25f;

    public float JumpMagnitude = 12;


}
