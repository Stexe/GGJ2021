using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnergyBarToolkit;


public class GameStateManager : MonoBehaviour
{

    public GameObject happinessGameObject;
    public GameObject scoreGameObject;
    private EnergyBar happinessBar;
    private float timer = 0.0f;
    private float decimalHappiness;
    public int happinessValue;

    public void Awake()
    {
        happinessBar = happinessGameObject.GetComponent<EnergyBar>();
        happinessValue = happinessBar.valueMax;
    }

    public void Update()
    {
        if (Time.timeScale != 0)
        {
            timer += Time.deltaTime;

            if (timer >= 1)
            {
                DecreaseHappiness();
                CheckLoss();
                timer = timer - 1;
            }
        }
    }

    public void DecreaseHappiness()
    {
        happinessValue--;
        happinessBar.valueCurrent = happinessValue;
    }

    public void CheckLoss()
    {
        if (happinessValue <= 0)
        {
            PlayerLost();
        }
    }

    public void PlayerLost()
    {
        Time.timeScale = 0;
        Debug.Log("You lost! Womp womp.");
    }

}
