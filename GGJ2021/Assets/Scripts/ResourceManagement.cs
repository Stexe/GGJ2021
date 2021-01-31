using EnergyBarToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Resources
{
    public PieceType type;
    public int count;
}

public class ResourceManagement : MonoBehaviour
{
    public Dictionary<PieceType, Resource> typeToResource;
    public Board mainBoard;

    void Start()
    {
        typeToResource = new Dictionary<PieceType, Resource>();
        foreach (var r in BoardsManager.FindMainBoard().resources)
        {
            typeToResource.Add(r.piece.Type, r);
        }

        foreach (var b in FindObjectsOfType<Board>())
        {
            b.OnPiecesMatched.AddListener(IncreaseResourcesForMatchedPieces);
        }
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
