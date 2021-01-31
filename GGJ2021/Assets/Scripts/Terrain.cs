using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TerrainType
{
    Normal, UnexploredBasic, UnexploredAdvanced, Excavating
}

public class Terrain : MonoBehaviour
{
    public TerrainType type;

    public Building building { get { return GetComponentInChildren<Building>(); } }
}
