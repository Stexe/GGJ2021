﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare : MonoBehaviour
{
    public Piece Piece
    {
        get { return GetComponentInChildren<Piece>(); }
    }
}
