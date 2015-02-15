﻿using UnityEngine;
using System.Collections;

public class Instakill : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player == null)
            return;

        LevelManager.Instance.KillPlayer(player);
    }
}
