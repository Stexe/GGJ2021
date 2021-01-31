using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDragging : MonoBehaviour
{
    private Board board
    {
        get
        {
            foreach (var b in FindObjectsOfType<Board>())
            {
                if (b.isActiveAndEnabled)
                {
                    return b;
                }
            }
            return null;
        }
    }

    public Piece heldPiece;
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
        if (Input.GetKeyUp(KeyCode.Mouse0) && heldPiece != null)
        {
            DropPiece(true);
        }
        previousMousePosition = currentMousePosition;

        // swap pieces while you're holding and moving the piece around
        var hoveredSquare = GetHoveredSquare();
        if (heldPiece != null
            && hoveredSquare != null
            && hoveredSquare != currentHoldingSquare
            //checks if it's a legal swap (to an adjacent square)
            && board.GetAdjacentSquares(currentHoldingSquare).Find(square => square.name == hoveredSquare.name) != null)
        {
            SwapHeldPieceWithOtherPiece(currentHoldingSquare, hoveredSquare);
        }
    }

    private void OnSquareClicked(BoardSquare square)
    {
        currentHoldingSquare = square;
        heldPiece = square.Piece;
    }

    public void DropPiece(bool doMatching)
    {
        if (heldPiece == null)
        {
            return;
        }

        //stop floating above other pieces
        heldPiece.transform.position = new Vector3(currentHoldingSquare.transform.position.x, currentHoldingSquare.transform.position.y, 0);

        heldPiece = null;

        if (doMatching)
        {
            while (board.FindAndNotifyMatches()) { }
        }
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

    private void SwapHeldPieceWithOtherPiece(BoardSquare squareWithHeldPiece, BoardSquare squareWithNotHeldPiece)
    {
        // remove not held piece from current square
        var toSquareWithHeld = board.RemovePieceFromSquare(squareWithNotHeldPiece);

        // move held piece to new square
        var piece = squareWithHeldPiece.Piece;
        var pos = piece.transform.position;
        piece.transform.SetParent(squareWithNotHeldPiece.transform, false);
        piece.transform.position = pos;

        //add not held piece to new square
        board.PlacePieceOnSquare(toSquareWithHeld, squareWithHeldPiece);

        //update held square state
        currentHoldingSquare = squareWithNotHeldPiece;
    }
}
