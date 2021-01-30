using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainType
{
    Normal, UnexploredBasic, UnexploredAdvanced, Excavating
}

public class Terrain : MonoBehaviour
{
    public TerrainType type;

    private Building building;
}
