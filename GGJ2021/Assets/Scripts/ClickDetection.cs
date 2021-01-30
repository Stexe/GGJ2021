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

    void Awake()
    {
        onBoardSquareDown = new BoardSquareDownEvent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == null)
            {
                return;
            }
            var maybeSquare = hit.collider.GetComponent<BoardSquare>();
            
            if (maybeSquare != null)
            {
                onBoardSquareDown.Invoke(maybeSquare);
            }
        }
    }
}
