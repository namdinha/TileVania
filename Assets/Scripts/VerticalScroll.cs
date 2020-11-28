﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScroll : MonoBehaviour {

    [Tooltip("Game Units per Second")]
    [SerializeField] float scrollRate = 0.2f;

    void Update() { 
        transform.Translate(new Vector3(0f, scrollRate * Time.deltaTime));
    }
}
