using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject buildingPanel;
    public GameObject excavationButtons;
    public Color selectedColor = Color.red;
    public Color notSelectedColor = Color.clear;

    public float hardExcavationTimeSec = 5;
    public float mediumExcavationTimeSec = 5;
    public float normalExcavationTimeSec = 5;

    private ResourceManagement resourceManagement;
    private Map map;
    private BoardsManager boardsManager;
    private BuildingType? typeToBuild;
    private ExcavationType? excavationMode;
    private float remainingExcavationTime;
    private Terrain excavatingTile;

    private void Start()
    {
        Assert.IsNotNull(buildingPanel);
        Assert.IsNotNull(excavationButtons);

        map = FindObjectOfType<Map>();
        map.InitializeMap();

        resourceManagement = FindObjectOfType<ResourceManagement>();
        boardsManager = FindObjectOfType<BoardsManager>();

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
            case TerrainType.UnexcavatedNormal:
                if (excavationMode.HasValue && CanAfford(ExcavationType.Normal))
                {
                    StartExcavation(tile, ExcavationType.Normal);
                    boardsManager.SwitchToBoard(BoardType.UnexploredTerrain);
                }
                break;
            case TerrainType.UnexcavatedMedium:
                if (excavationMode.HasValue && CanAfford(ExcavationType.Medium))
                {
                    StartExcavation(tile, ExcavationType.Medium);
                    boardsManager.SwitchToBoard(BoardType.MediumUnexplored);
                }
                break;
            case TerrainType.UnexcavatedHard:
                if (excavationMode.HasValue && CanAfford(ExcavationType.Hard))
                {
                    StartExcavation(tile, ExcavationType.Hard);
                    boardsManager.SwitchToBoard(BoardType.HardUnexploredTerrain);
                }
                break;
            default:
                throw new System.Exception("Unexcpected case for " + tile.type);
        }
    }

    private void StartExcavation(Terrain tile, ExcavationType type)
    {
        switch (type)
        {
            case ExcavationType.Hard:
                remainingExcavationTime = hardExcavationTimeSec;
                break;
            case ExcavationType.Medium:
                remainingExcavationTime = mediumExcavationTimeSec;
                break;
            case ExcavationType.Normal:
                remainingExcavationTime = normalExcavationTimeSec;
                break;
        }
        excavatingTile = tile;
        map.BeginTileExcavation(tile);
        StartCoroutine("EndExcavationTimer");
    }

    IEnumerator EndExcavationTimer()
    {
        Debug.Log("Starting to End");
        while (remainingExcavationTime > 0)
        {
            var oldTime = remainingExcavationTime;
            remainingExcavationTime -= Time.deltaTime;

            if((int) oldTime != (int)remainingExcavationTime)
            {
                Debug.Log("Remaining: " + oldTime + " seconds");
            }

            yield return null;
        }
        map.FinishTileExcavation(excavatingTile);
        boardsManager.SwitchToBoard(BoardType.Main);
    }

    private bool CanAfford(BuildingType type)
    {
        return resourceManagement.HasResourcesToBuild(map.typeToBuilding[type]);
    }

    private bool CanAfford(ExcavationType type)
    {
        return resourceManagement.HasResourcesToExcavate(type);
    }

    public void OnBuildButtonClicked(BuildingType type)
    {
        var buttons = buildingPanel.GetComponentsInChildren<BuildingButtonType>();

        //ensure all other buttons are unselected
        DeselectBuildingButtons(buildingPanel.GetComponentsInChildren<BuildingButtonType>()
            .Where(b => b.type == type)
            .ToArray());

        //handle clicked button
        var button = buttons.Where(b => b.type == type).First();
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
            excavationMode = null;

            button.GetComponent<Image>().color = selectedColor;
        }
    }

    private void DeselectBuildingButtons(BuildingButtonType[] buttons)
    {
        foreach (var b in buttons)
        {
            b.GetComponent<Image>().color = notSelectedColor;
        }
    }

    private void OnExcavateButtonClicked(ExcavationType type)
    {
        DeselectBuildingButtons(buildingPanel.GetComponentsInChildren<BuildingButtonType>());

        if (excavationMode.HasValue && excavationMode.Value == type)
        {
            Debug.Log("deselected " + type);
            excavationMode = null;
        }
        else if (resourceManagement.HasResourcesToExcavate(type))
        {
            excavationMode = type;
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
