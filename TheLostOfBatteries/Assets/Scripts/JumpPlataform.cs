using UnityEngine;
using System.Collections;

public class JumpPlataform : MonoBehaviour {
    public float JumpMagnitude = 20;

    public void ControllerEnter2D(CharacterController2D controller)
    {
        controller.Setverticalforce(JumpMagnitude);
    }
}
