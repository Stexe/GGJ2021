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
    public int match3 = 3;
    public int match4 = 4;
    public int match5 = 5;
    public int match6 = 6;
    public int match7 = 7;
    public int match8 = 8;
    public int match9 = 9;

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
        int toIncrement;
        switch (matched.Count())
        {
            case 3:
                toIncrement = match3;
                break;
            case 4:
                toIncrement = match4;
                break;
            case 5:
                toIncrement = match5;
                break;
            case 6:
                toIncrement = match6;
                break;
            case 7:
                toIncrement = match7;
                break;
            case 8:
                toIncrement = match8;
                break;
            case 9:
                toIncrement = match9;
                break;
            default:
                throw new System.Exception("unexpected match count: " + matched.Count());
        }

        Resource resource = typeToResource[matched.First().Type];
        int total = resource.bar.valueCurrent;
        total += toIncrement;
        while (resource.bar.valueMax <= total)
        {
            total -= resource.bar.valueMax;
            resource.bar.GetComponentInChildren<ResourceAmount>().IncreaseAmount(1);
        }
        resource.bar.valueCurrent = total;
    }
}
