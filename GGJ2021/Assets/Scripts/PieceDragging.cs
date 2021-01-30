using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDragging : MonoBehaviour
{
    private Piece heldPiece;
    private BoardSquare currentHoldingSquare;
    private Vector2 previousMousePosition;

    void Start()
    {
        FindObjectOfType<ClickDetection>().onBoardSquareDown.AddListener(OnSquareClicked);
    }

    private void Update()
    {
        Vector2 currentMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (heldPiece != null)
        {
            heldPiece.transform.position = new Vector3(
                heldPiece.transform.position.x + currentMouse.x - previousMousePosition.x,
                heldPiece.transform.position.y + currentMouse.y - previousMousePosition.y,
                // float above other pieces
                -1);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //stop floating above other pieces
            heldPiece.transform.position = new Vector3(heldPiece.transform.position.x, heldPiece.transform.position.y, 0);
            
            heldPiece = null;
        }
        previousMousePosition = currentMouse;
    }

    void OnSquareClicked(BoardSquare square)
    {
        currentHoldingSquare = square;
        heldPiece = square.Piece;
    }
}
