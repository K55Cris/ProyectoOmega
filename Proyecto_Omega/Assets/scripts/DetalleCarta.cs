﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetalleCarta : MonoBehaviour {
    public Image carta;

    public void Cargar(Sprite image)
    {
        carta.sprite = image;
    }
}