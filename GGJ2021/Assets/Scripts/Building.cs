using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cost
{
    public PieceType type;
    public int amount;
}
public enum BuildingType
{
    HQ, Farm, Lumberyard, Quarry, Forge, MiningFacility, Market, DrugHug, Nuclear, Statue
}

public class Building : MonoBehaviour
{
    public BuildingType type;
    public bool hasEfficiencyType;
    public PieceType efficiencyType;
    public Cost[] costs;
}
