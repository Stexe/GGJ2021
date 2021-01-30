using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{

    public Canvas masterCanvas;
    public PieceDragging pieceDragging;
    private EnergyBarToolkit.FilledRendererUGUI filledRenderer;

    // Start is called before the first frame update
    void Start()
    {
        pieceDragging = pieceDragging.GetComponent<PieceDragging>();
        filledRenderer = filledRenderer.GetComponent<EnergyBarToolkit.FilledRendererUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pieceDragging.heldPiece == null)
        {
            filledRenderer.enabled = false;
        }

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(masterCanvas.transform as RectTransform, Input.mousePosition, masterCanvas.worldCamera, out pos);
        transform.position = masterCanvas.transform.TransformPoint(pos);
    }
}
