using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    public PieceType Type;

    private void Start()
    {
        var color = PieceMapping.typeToColor[Type];

        if (color == null)
        {
            throw new Exception("missing piece type: " + Type);
        }

        GetComponent<SpriteRenderer>().color = color;
    }
}
