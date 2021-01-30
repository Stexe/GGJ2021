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
    HQ, Farm, Lumberyard, ConcreteFactory, Metalworks, MiningFacility, NuclearReactor, DrugAndHug, Market, Monolith
}

public class Building : MonoBehaviour
{
    public BuildingType type;
    public Cost[] costs;
}
