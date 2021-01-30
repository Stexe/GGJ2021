using EnergyBarToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Resource
{
    public Piece piece;
    public EnergyBar bar;
}

public class ResourceManagement : MonoBehaviour
{   
    public Resource[] resources;

    public Dictionary<PieceType, Resource> typeToResource;

    void Start()
    {
        typeToResource = new Dictionary<PieceType, Resource>();
        foreach (var r in resources)
        {
            typeToResource.Add(r.piece.Type, r);
        }
        
        FindObjectOfType<Board>().OnPiecesMatched.AddListener(IncreaseResourcesForMatchedPieces);

    }

    private void IncreaseResourcesForMatchedPieces(HashSet<Piece> matched)
    {
        foreach (Piece p in matched)
        {
            typeToResource[p.Type].bar.valueCurrent++;
        }
    }
}
