using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceEfficiency : MonoBehaviour
{
    public PieceType type;
    private EnergyBar energyBar;
    private int amount;

    private void Awake()
    {
        energyBar = GetComponentInParent<EnergyBar>();
    }

    private void Start()
    {
        amount = energyBar.valueMax;
        GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }

    public void IncreaseAmount(int byAmount)
    {
        amount += byAmount;
        energyBar.valueMax += byAmount;
        GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }

    public void DecreaseAmount(int byAmount)
    {
        amount -= byAmount;
        energyBar.valueMax -= byAmount;
        GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }
}
