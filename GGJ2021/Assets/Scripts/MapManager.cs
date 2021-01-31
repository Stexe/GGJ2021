using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    Map map;

    private void Start()
    {
        map = FindObjectOfType<Map>();
        map.InitializeMap();

        foreach (var t in map.tiles)
        {
            t.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnMapTileClicked(t); });
        }
    }

    public void OnMapTileClicked(Terrain tile)
    {
        switch (tile.type)
        {
            case TerrainType.Normal:
                if (tile.building == null)
                {
                    map.GenerateBuildingOnTerrainTile(BuildingType.Farm, tile);
                }
                break;
            case TerrainType.UnexploredBasic:
                map.BeginTileExcavation(tile);
                break;
        }
    }
}
