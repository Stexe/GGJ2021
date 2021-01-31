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

    private void OnExcavateNormalButtonClicked()
    {
        if (inExcavateMode)
        {
            inExcavateMode = false;
        }
        else if (resourceManagement.HasResourcesToExcavateNormal())
        {
            inExcavateMode = true;
            typeToBuild = null;
        }
    }

    private void OnExcavateMediumButtonClicked()
    {
        if (inExcavateMode)
        {
            inExcavateMode = false;
        }
        else if (resourceManagement.HasResourcesToExcavateMedium())
        {
            inExcavateMode = true;
            typeToBuild = null;
        }
    }

    private void OnExcavateHardButtonClicked()
    {
        if (inExcavateMode)
        {
            inExcavateMode = false;
        }
        else if (resourceManagement.HasResourcesToExcavateHard())
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
        OnBuildButtonClicked(BuildingType.Quarry);
    }

    public void MetalworksButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Forge);
    }

    public void MiningFacilityButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.MiningFacility);
    }

    public void NuclearReactorButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Market);
    }

    public void DrugAndHugButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.DrugHug);
    }

    public void MarketButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Nuclear);
    }

    public void MonolithButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Statue);
    }

    public void ExcavateNormalButtonClicked()
    {
        OnExcavateNormalButtonClicked();
    }

    public void ExcavateMediumButtonClicked()
    {
        OnExcavateMediumButtonClicked();
    }

    public void ExcavateHardButtonClicked()
    {
        OnExcavateHardButtonClicked();
    }
}
