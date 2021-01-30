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
    public int count;
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

        Board board = FindObjectOfType<Board>();
        board.OnPiecesMatched.AddListener(IncreaseResourcesForMatchedPieces);

        board.InitializeBoard();
    }

    private void IncreaseResourcesForMatchedPieces(HashSet<Piece> matched)
    {
        foreach (Piece p in matched)
        {
            Resource resource = typeToResource[p.Type];
            resource.bar.valueCurrent++;
            while (resource.bar.valueMax <= resource.bar.valueCurrent)
            {
                resource.bar.valueCurrent -= resource.bar.valueMax;
                resource.bar.GetComponentInChildren<ResourceAmount>().IncreaseAmount(1);
            }
        }
    }
}
