using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceAmount : MonoBehaviour
{
    private int amount;
    public void IncreaseAmount(int byAmount)
    {
        amount += byAmount;
        GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }
}
