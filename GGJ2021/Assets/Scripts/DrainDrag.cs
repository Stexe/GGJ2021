using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrainDrag : MonoBehaviour
{
    public PieceDragging pieceDragging;
    private EnergyBar bar;
    public int timeToDropMS;
    int elapsedTime;
    bool wasHeld;

    private void Start()
    {
        pieceDragging = pieceDragging.GetComponent<PieceDragging>();
        bar = this.GetComponent<EnergyBar>();
        bar.valueMax = timeToDropMS;
    }

    private void Update()
    {
        if (pieceDragging.heldPiece != null)
        {
            if (!wasHeld)
            {
                elapsedTime = timeToDropMS;
            }
            wasHeld = true;
            elapsedTime -= (int)(Time.deltaTime * 1000);
            bar.valueCurrent = elapsedTime;
            if (bar.valueCurrent <= 0)
            {
                pieceDragging.DropPiece(true);
            }
        }

        if (pieceDragging.heldPiece == null)
        {
            wasHeld = false;
            bar.valueCurrent = bar.valueMax;
        }
        
        
    }
}
