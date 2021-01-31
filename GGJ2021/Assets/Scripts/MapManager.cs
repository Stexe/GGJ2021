using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject buildingPanel;
    public Color selectedColor = Color.red;
    public Color notSelectedColor = Color.clear;

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

    public void OnBuildButtonClicked(BuildingType type)
    {
        Debug.Log("selected " + type);
        foreach (var button in buildingPanel.GetComponentsInChildren<BuildingButtonType>())
        {
            if (button.type != type)
            {
                button.GetComponent<Image>().color = notSelectedColor;
            }
            else
            {
                // found clicked button
                if (typeToBuild.HasValue && typeToBuild.Value == type)
                {
                    Debug.Log("deselected " + type);
                    typeToBuild = null;

                    button.GetComponent<Image>().color = notSelectedColor;
                }
                else if (CanAfford(type))
                {
                    Debug.Log("Build Mode: " + type);
                    typeToBuild = type;
                    inExcavateMode = false;

                    button.GetComponent<Image>().color = selectedColor;
                }
            }
        }
    }

    private void OnExcavateButtonClicked(ExcavationType type)
    {
        Debug.Log("selected " + type);
        if (inExcavateMode)
        {
            inExcavateMode = false;
        }
        else if (resourceManagement.HasResourcesToExcavate(type))
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

    public void QuarryButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Quarry);
    }

    public void ForgeButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Forge);
    }

    public void MiningFacilityButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.MiningFacility);
    }

    public void MarketButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Market);
    }

    public void DrugHugButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.DrugHug);
    }

    public void NuclearButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Nuclear);
    }

    public void StatueButtonClicked()
    {
        OnBuildButtonClicked(BuildingType.Statue);
    }

    public void ExcavateNormalButtonClicked()
    {
        OnExcavateButtonClicked(ExcavationType.Normal);
    }

    public void ExcavateMediumButtonClicked()
    {
        OnExcavateButtonClicked(ExcavationType.Medium);
    }

    public void ExcavateHardButtonClicked()
    {
        OnExcavateButtonClicked(ExcavationType.Hard);
    }
}
