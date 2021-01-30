using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    Blue, Green, Red, Black, Yellow, White, Magenta, Grey, Cyan
}

public class PieceMapping
{
    public static Dictionary<PieceType, Color> typeToColor = new Dictionary<PieceType, Color>();

    static PieceMapping()
    {
        typeToColor.Add(PieceType.Black, Color.black);
        typeToColor.Add(PieceType.Blue, Color.blue);
        typeToColor.Add(PieceType.Green, Color.green);
        typeToColor.Add(PieceType.Red, Color.red);
        typeToColor.Add(PieceType.White, Color.white);
        typeToColor.Add(PieceType.Yellow, Color.yellow);
        typeToColor.Add(PieceType.Magenta, Color.magenta);
        typeToColor.Add(PieceType.Grey, Color.grey);
        typeToColor.Add(PieceType.Cyan, Color.cyan);
    }
}
