using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardSquareDownEvent : UnityEvent<BoardSquare> { }

public class ClickDetection : MonoBehaviour
{
    public BoardSquareDownEvent onBoardSquareDown;
    GraphicRaycaster raycaster;

    void Awake()
    {
        this.raycaster = GetComponent<GraphicRaycaster>();

        onBoardSquareDown = new BoardSquareDownEvent();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            pointerData.position = Input.mousePosition;
            this.raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                var square = result.gameObject.GetComponent<BoardSquare>();
                if (square != null)
                {
                    onBoardSquareDown.Invoke(square);
                }
            }
        }
    }
}
