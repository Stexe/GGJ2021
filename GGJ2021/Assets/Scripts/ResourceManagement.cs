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

public enum ExcavationType
{
    Normal, Medium, Hard
}

public class ResourceManagement : MonoBehaviour
{
    public Dictionary<PieceType, Resource> typeToResource;
    public Cost[] costToExcavateNormal, costToExcavateMedium, costToExcavateHard;

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
        if (building.hasEfficiencyType)
        {
            typeToResource[building.efficiencyType].bar.GetComponentInChildren<ResourceEfficiency>().DecreaseAmount(1);
        }
        foreach (var cost in building.costs)
        {
            typeToResource[cost.type].bar.GetComponentInChildren<ResourceAmount>().DecreaseAmount(cost.amount);
        }
    }

    public bool HasResourcesToBuild(Building building)
    {
        return !building.costs.Any(cost => typeToResource[cost.type].bar.GetComponentInChildren<ResourceAmount>().amount < cost.amount);
    }

    public bool HasResourcesToExcavate(ExcavationType type)
    {
        switch (type)
        {
            case ExcavationType.Normal:
                return !costToExcavateNormal.Any(cost => typeToResource[cost.type].bar.valueCurrent < cost.amount);
            case ExcavationType.Medium:
                return !costToExcavateMedium.Any(cost => typeToResource[cost.type].bar.valueCurrent < cost.amount);
            case ExcavationType.Hard:
                return !costToExcavateHard.Any(cost => typeToResource[cost.type].bar.valueCurrent < cost.amount);
        }

        throw new Exception("Cost undefined for " + type);
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
