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
        Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (heldPiece != null)
        {
            heldPiece.transform.position = new Vector3(
                heldPiece.transform.position.x + currentMousePosition.x - previousMousePosition.x,
                heldPiece.transform.position.y + currentMousePosition.y - previousMousePosition.y,
                // float above other pieces
                -1);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //stop floating above other pieces
            heldPiece.transform.position = new Vector3(currentHoldingSquare.transform.position.x, currentHoldingSquare.transform.position.y, 0);
            
            heldPiece = null;
        }
        previousMousePosition = currentMousePosition;

        // swap pieces
        var hoveredSquare = GetHoveredSquare();
        if (heldPiece != null && hoveredSquare != null && hoveredSquare.name != currentHoldingSquare.name)
        {
            Debug.Log("SWAPPING " + hoveredSquare.name + ", " + currentHoldingSquare.name);
            SwapPieces(currentHoldingSquare, hoveredSquare);
        }
    }

    private void OnSquareClicked(BoardSquare square)
    {
        currentHoldingSquare = square;
        heldPiece = square.Piece;
    }

    private BoardSquare GetHoveredSquare()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider == null)
        {
            return null;
        }

        return hit.collider.GetComponent<BoardSquare>();
    }

    private void SwapPieces(BoardSquare squareWithHeldPiece, BoardSquare squareWithNotHeldPiece)
    {
        //move ownership of not-held-piece
        var notHeldPiece = squareWithNotHeldPiece.Piece;
        notHeldPiece.transform.position = squareWithHeldPiece.transform.position;
        squareWithHeldPiece.Piece = notHeldPiece;

        //move ownership of held piece
        squareWithNotHeldPiece.Piece = heldPiece;
        currentHoldingSquare = squareWithNotHeldPiece;
    }
}
