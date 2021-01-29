using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDragging : MonoBehaviour
{
    public Board board;
    public ClickDetection clickDetection;
    private Piece heldPiece;
    Vector2 previousMousePosition;

    void Start()
    {
        clickDetection.onBoardSquareDown.AddListener(OnSquareClicked);
    }

    private void Update()
    {
        if (heldPiece != null)
        {
            heldPiece.transform.position = new Vector3(
                heldPiece.transform.position.x + Input.mousePosition.x - previousMousePosition.x,
                heldPiece.transform.position.y + Input.mousePosition.y - previousMousePosition.y,
                heldPiece.transform.position.z);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            heldPiece = null;
        }
        previousMousePosition = Input.mousePosition;
    }

    void OnSquareClicked(BoardSquare square)
    {
        heldPiece = square.Piece;
    }
}
