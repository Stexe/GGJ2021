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
        var image = GetComponent<SpriteRenderer>();

        switch (Type)
        {
            case PieceType.Black:
                image.color = Color.black;
                break;
            case PieceType.Blue:
                image.color = Color.blue;
                break;
            case PieceType.Green:
                image.color = Color.green;
                break;
            case PieceType.Red:
                image.color = Color.red;
                break;
            case PieceType.White:
                image.color = Color.white;
                break;
            case PieceType.Yellow:
                image.color = Color.yellow;
                break;
            default:
                throw new Exception("missing piece type: " + Type);
        }
    }
}
