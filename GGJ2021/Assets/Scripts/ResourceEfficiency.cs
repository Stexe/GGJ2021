using TMPro;
using UnityEngine;

public class ResourceEfficiency : MonoBehaviour
{
    public PieceType type;
    public int minimumCost = 5;
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

    public void DecreaseAmount(int byAmount)
    {
        int newValue = Mathf.Max(amount - byAmount, minimumCost);
        amount = newValue;
        energyBar.valueMax = newValue;
        GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }
}
