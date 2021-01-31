using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public Building[] buildingTypes;
    public Terrain[] terrainTypes;

    public Terrain[,] tiles;

    private Dictionary<TerrainType, Terrain> typeToTerrain = new Dictionary<TerrainType, Terrain>();
    [HideInInspector]
    public Dictionary<BuildingType, Building> typeToBuilding = new Dictionary<BuildingType, Building>();
    private Dictionary<BuildingType, int> buildingCount = new Dictionary<BuildingType, int>();

    public void InitializeMap()
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
        tiles = new Terrain[grid.constraintCount, grid.constraintCount];

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
                    GenerateTerrainTile(TerrainType.UnexcavatedNormal, x, y);
                }
            }
        }
    }

    private Terrain GenerateTerrainTile(TerrainType type, int x, int y)
    {
        var terrain = Instantiate(typeToTerrain[type], this.transform);
        terrain.gameObject.name = type + " " + Board.coordStr(x, y);

        tiles[x, y] = terrain;

        return terrain;
    }

    public Building GenerateBuildingOnTerrainTile(BuildingType type, Terrain tile)
    {
        var building = Instantiate(typeToBuilding[type], tile.transform);
        if (!buildingCount.ContainsKey(type))
        {
            buildingCount.Add(type, 1);
        }
        building.gameObject.name = type + " " + buildingCount[type]++;

        return building;
    }

    public void BeginTileExcavation(Terrain tile)
    {
        Debug.Log("excavating " + tile.name);

        ConvertToTerrainType(tile, TerrainType.Excavating);
    }

    public void FinishTileExcavation(Terrain tile)
    {
        Debug.Log("excavated " + tile.name);

        ConvertToTerrainType(tile, TerrainType.Normal);
    }

    private void ConvertToTerrainType(Terrain toConvert, TerrainType type)
    {
        toConvert.type = type;

        // change graphics
        toConvert.GetComponent<Button>().targetGraphic = typeToTerrain[type].GetComponent<Image>();
        Color newColor = typeToTerrain[type].transform.GetChild(0).GetComponent<Image>().color;
        toConvert.transform.GetChild(0).GetComponent<Image>().color = newColor;
    }
}
