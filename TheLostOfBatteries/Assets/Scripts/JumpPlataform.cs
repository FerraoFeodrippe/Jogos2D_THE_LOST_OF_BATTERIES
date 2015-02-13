using UnityEngine;
using System.Collections;

public class JumpPlataform : MonoBehaviour {
    public float JumpMagnitude = 5;

    public void ControllerEnter2D(CharacterController2D controller)
    {
        controller.Setverticalforce(JumpMagnitude);
        

    }
}
