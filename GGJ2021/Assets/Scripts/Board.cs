using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public int TypeCount;
    public int MinimumMatchCount;
    public int Rows;
    public int Columns;

    public BoardSquare squarePrefab;
    public Piece piecePrefab;

    public BoardSquare[,] boardSquares;
    GridLayoutGroup grid;

    private void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
        grid.constraintCount = Columns;
        InitializeBoard();
    }

    public void InitializeBoard()
    {
        boardSquares = new BoardSquare[Rows, Columns];
        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                // instantiate board square
                var square = Instantiate(squarePrefab, this.transform);
                boardSquares[x, y] = square;
                square.name = "Square (" + x + ", " + y + ")";

                // randomize piece type without causes matches (brute force)
                PieceType type;
                do
                {
                    type = (PieceType)Random.Range(0, TypeCount);
                } while (GetVerticallyConnectedOfType(this, type, x, y).Count >= MinimumMatchCount - 1
                || GetHorizontallyConnectedOfType(this, type, x, y).Count >= MinimumMatchCount - 1);

                // create piece type
                var piece = Instantiate(piecePrefab, square.transform);
                piece.Type = type;
                square.Piece = piece;
            }
        }
    }

    public static List<BoardSquare> GetVerticallyConnectedOfType(Board board, PieceType type, int x, int y)
    {
        var connected = new List<BoardSquare>();

        // check above
        for (int i = y - 1; i >= 0; i--)
        {
            var square = board.boardSquares[x, i];
            if (square.Piece == null || square.Piece.Type != type)
            {
                break;
            }
            connected.Add(square);
        }

        // check below
        for (int i = y + 1; i < board.Rows; i++)
        {
            var square = board.boardSquares[x, i];
            if (square == null || square.Piece == null || square.Piece.Type != type)
            {
                break;
            }
            connected.Add(square);
        }

        return connected;
    }

    public static List<BoardSquare> GetHorizontallyConnectedOfType(Board board, PieceType type, int x, int y)
    {
        var connected = new List<BoardSquare>();

        // check left
        for (int i = x - 1; i >= 0; i--)
        {
            var square = board.boardSquares[i, y];
            if (square.Piece == null || square.Piece.Type != type)
            {
                break;
            }
            connected.Add(square);
        }

        // check right
        for (int i = x + 1; i < board.Columns; i++)
        {
            var square = board.boardSquares[i, y];
            if (square == null || square.Piece == null || square.Piece.Type != type)
            {
                break;
            }
            connected.Add(square);
        }

        return connected;
    }

    public List<BoardSquare> GetAdjacentSquares(BoardSquare square)
    {
        List<BoardSquare> adjacent = new List<BoardSquare>();

        int x = 0;
        int y = 0;
        bool go = true;
        for (; x < Columns && go; x++)
        {
            for (; y < Rows && go; y++)
            {
                if (square.name == boardSquares[x, y].name)
                {
                    go = false;
                    break;
                }
            }
            if (!go)
            {
                break;
            }
            y = 0;
        }

        if (x > 0)
        {
            adjacent.Add(boardSquares[x - 1, y]);
        }
        if (y > 0)
        {
            adjacent.Add(boardSquares[x, y - 1]);
        }
        if (x < Columns - 1)
        {
            adjacent.Add(boardSquares[x + 1, y]);
        }
        if (y < Rows - 1)
        {
            adjacent.Add(boardSquares[x, y + 1]);
        }

        return adjacent;
    }
}
