using EnergyBarToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Cost[] costToExcavate;

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

    public void DeductCostAndModifyEfficiency(Building building)
    {
        foreach (var cost in building.costs)
        {
            typeToResource[cost.type].bar.GetComponentInChildren<ResourceEfficiency>().DecreaseAmount(1);
            typeToResource[cost.type].bar.GetComponentInChildren<ResourceAmount>().DecreaseAmount(cost.amount);
        }
    }

    public bool HasResourcesToBuild(Building building)
    {
        return !building.costs.Any(cost => typeToResource[cost.type].bar.GetComponentInChildren<ResourceAmount>().amount < cost.amount);
    }

    public bool HasResourcesToExcavate()
    {
        return !costToExcavate.Any(cost => typeToResource[cost.type].bar.valueCurrent < cost.amount);
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
