using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceAmount : MonoBehaviour
{
    public PieceType type;
    [HideInInspector]
    public int amount;

    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = 0.ToString();
    }

    public void IncreaseAmount(int byAmount)
    {
        amount += byAmount;
        GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }

    public void DecreaseAmount(int byAmount)
    {
        amount -= byAmount;
        GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }
}
