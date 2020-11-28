﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int valueOfCoin = 100;

    void OnTriggerEnter2D(Collider2D collision) {
        FindObjectOfType<GameSession>().AddToScore(valueOfCoin);
        AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
        Destroy(gameObject);
    }
}
