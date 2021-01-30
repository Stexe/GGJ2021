using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public Building[] buildingTypes;
    public Terrain[] terrainTypes;

    public Terrain[,] tiles;

    private Dictionary<TerrainType, Terrain> typeToTerrain = new Dictionary<TerrainType, Terrain>();
    private Dictionary<BuildingType, Building> typeToBuilding = new Dictionary<BuildingType, Building>();
    private Dictionary<BuildingType, int> buildingCount = new Dictionary<BuildingType, int>();

    private void Start()
    {
        foreach (var terrain in terrainTypes)
        {
            typeToTerrain.Add(terrain.type, terrain);
        }

        foreach (var b in buildingTypes)
        {
            typeToBuilding.Add(b.type, b);
        }

        var grid = GetComponent<GridLayoutGroup>();
        for (int x = 0; x < grid.constraintCount; x++)
        {
            for (int y = 0; y < grid.constraintCount; y++)
            {
                if ((x == 3 && y == 4)
                    || (x == 4 && y == 5)
                    || (x == 4 && y == 3)
                    || (x == 5 && y == 4))
                {
                    GenerateTerrainTile(TerrainType.Normal, x, y);
                }
                else if (x == 4 && y == 4)
                {
                    var terrain = GenerateTerrainTile(TerrainType.Normal, x, y);
                    GenerateBuildingOnTerrainTile(BuildingType.HQ, terrain);
                }
                else
                {
                    GenerateTerrainTile(TerrainType.UnexploredBasic, x, y);
                }
            }
        }
    }

    private Terrain GenerateTerrainTile(TerrainType type, int x, int y)
    {
        var terrain = Instantiate(typeToTerrain[type], this.transform);
        terrain.gameObject.name = type + " " + Board.coordStr(x, y);
        return terrain;
    }

    private Building GenerateBuildingOnTerrainTile(BuildingType type, Terrain tile)
    {
        var building = Instantiate(typeToBuilding[type], tile.transform);
        if (!buildingCount.ContainsKey(type))
        {
            buildingCount.Add(type, 1);
        }
        building.gameObject.name = type + " " + buildingCount[type]++;

        return building;
    }
}
