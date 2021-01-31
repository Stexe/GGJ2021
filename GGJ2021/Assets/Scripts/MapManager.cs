using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    private ResourceManagement resourceManagement;
    private Map map;
    private BuildingType? typeToBuild;
    private bool inExcavateMode;

    private void Start()
    {
        map = FindObjectOfType<Map>();
        map.InitializeMap();

        resourceManagement = FindObjectOfType<ResourceManagement>();

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
                if (tile.building == null && typeToBuild != null && CanAfford(typeToBuild.Value))
                {
                    map.GenerateBuildingOnTerrainTile(typeToBuild.Value, tile);
                    resourceManagement.DeductCostAndModifyEfficiency(map.typeToBuilding[typeToBuild.Value]);
                }
                break;
            case TerrainType.UnexploredBasic:
            case TerrainType.UnexploredAdvanced:
                if (inExcavateMode)
                {
                    map.BeginTileExcavation(tile);
                }
                break;
        }
    }

    private bool CanAfford(BuildingType type)
    {
        return resourceManagement.HasResourcesToBuild(map.typeToBuilding[type]);
    }

    private void OnBuildButtonClicked(BuildingType type)
    {
        if (typeToBuild == type)
        {
            typeToBuild = null;
        }
        else if (CanAfford(type))
        {
            Debug.Log("Build Mode: " + type);
            typeToBuild = type;
            inExcavateMode = false;
        }
    }

    private void OnExcavateButtonClicked()
    {
        if (inExcavateMode)
        {
            inExcavateMode = false;
        }
        else if (resourceManagement.HasResourcesToExcavate())
        {
            inExcavateMode = true;
            typeToBuild = null;
        }
    }

    public void FarmButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Farm);
    }

    public void LumberyardButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Lumberyard);
    }

    public void ConcreteFactoryButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.ConcreteFactory);
    }

    public void MetalworksButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Metalworks);
    }

    public void MiningFacilityButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.MiningFacility);
    }

    public void NuclearReactorButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.NuclearReactor);
    }

    public void DrugAndHugButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.DrugAndHug);
    }

    public void MarketButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Market);
    }

    public void MonolithButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Monolith);
    }

    public void ExcavateButtonClicked()
    {
        OnExcavateButtonClicked();
    }
}
