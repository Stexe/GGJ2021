using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PiecesEvent : UnityEvent<HashSet<Piece>> { }

public class Board : MonoBehaviour
{
    public int TypeCount;
    public int MinimumMatchCount;
    public int Rows;
    public int Columns;

    public BoardSquare squarePrefab;
    public Piece piecePrefab;

    public PiecesEvent OnPiecesMatched;

    public BoardSquare[,] squares;

    private GridLayoutGroup grid;
    private Dictionary<PieceType, int> pieceTypeCounter = new Dictionary<PieceType, int>();
    private ResourceManagement resourceManagement;

    private void Awake()
    {
        OnPiecesMatched = new PiecesEvent();
    }

    private void Start()
    {
        resourceManagement = FindObjectOfType<ResourceManagement>();
        grid = GetComponent<GridLayoutGroup>();
        grid.constraintCount = Columns;
        InitializeBoard();
    }

    public void InitializeBoard()
    {
        squares = new BoardSquare[Columns, Rows];
        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                // instantiate board square
                var square = Instantiate(squarePrefab, this.transform);
                squares[x, y] = square;
                square.name = "Square (" + x + ", " + y + ")";

                // randomize piece type without causing matches (brute force)
                PieceType type;
                do
                {
                    type = (PieceType)Random.Range(0, TypeCount);
                } while (GetVerticallyConnectedOfType(type, x, y).Count + 1 >= MinimumMatchCount
                || GetHorizontallyConnectedOfType(type, x, y).Count + 1 >= MinimumMatchCount);

                // create new piece
                GenerateNewPiece(square, type);
            }
        }
    }

    public List<BoardSquare> GetVerticallyConnectedOfType(PieceType type, int x, int y)
    {
        var connected = new List<BoardSquare>();

        // check above
        for (int i = y - 1; i >= 0; i--)
        {
            var square = squares[x, i];
            if (square.Piece == null || square.Piece.Type != type)
            {
                break;
            }
            connected.Add(square);
        }

        // check below
        for (int i = y + 1; i < Rows; i++)
        {
            var square = squares[x, i];
            if (square == null || square.Piece == null || square.Piece.Type != type)
            {
                break;
            }
            connected.Add(square);
        }

        return connected;
    }

    public List<BoardSquare> GetHorizontallyConnectedOfType(PieceType type, int x, int y)
    {
        var connected = new List<BoardSquare>();

        // check left
        for (int i = x - 1; i >= 0; i--)
        {
            var square = squares[i, y];
            if (square.Piece == null || square.Piece.Type != type)
            {
                break;
            }
            connected.Add(square);
        }

        // check right
        for (int i = x + 1; i < Columns; i++)
        {
            var square = squares[i, y];
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
                if (square == squares[x, y])
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
            adjacent.Add(squares[x - 1, y]);
        }
        if (y > 0)
        {
            adjacent.Add(squares[x, y - 1]);
        }
        if (x < Columns - 1)
        {
            adjacent.Add(squares[x + 1, y]);
        }
        if (y < Rows - 1)
        {
            adjacent.Add(squares[x, y + 1]);
        }

        return adjacent;
    }

    public bool FindAndNotifyMatches()
    {
        //find all of the matched pieces
        var matched = new HashSet<BoardSquare>();
        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                var connected = GetHorizontallyConnectedOfType(squares[x, y].Piece.Type, x, y);
                if (connected.Count + 1 >= MinimumMatchCount)
                {
                    matched.Add(squares[x, y]);
                    foreach (var c in connected)
                    {
                        matched.Add(c);
                    }

                }
                connected = GetVerticallyConnectedOfType(squares[x, y].Piece.Type, x, y);
                if (connected.Count + 1 >= MinimumMatchCount)
                {
                    matched.Add(squares[x, y]);
                    foreach (var c in connected)
                    {
                        matched.Add(c);
                    }
                }
            }
        }

        OnPiecesMatched.Invoke(new HashSet<Piece>(matched.Select(m => m.Piece)));

        //delete all of the matched pieces
        foreach (var square in matched)
        {
            Debug.Log("Matched " + square.Piece.name + " on " + square.gameObject.name.Replace("Square", ""));
            var piece = RemovePieceFromSquare(square);
            Destroy(piece.gameObject);
        }

        //pull down all non-pieces
        for (int x = 0; x < Columns; x++)
        {
            for (int y = Rows - 1; y >= 0; y--)
            {
                if (squares[x, y].Piece == null)
                {
                    BoardSquare aboveSquareWithPiece = null;
                    // find a piece above to pull down
                    for (int j = y - 1; aboveSquareWithPiece == null && j >= 0; j--)
                    {
                        Debug.Log("Looking at " + coordStr(x, j));
                        if (squares[x, j].Piece != null)
                        {
                            Debug.Log("Moving " + squares[x, j].Piece.name + coordStr(x, j) + " -> " + coordStr(x, y));
                            MovePieceBetweenSquares(squares[x, j], squares[x, y]);
                            break;
                        }
                    }
                }
            }
        }

        //fill in empty spots
        for (int x = 0; x < Columns; x++)
        {
            for (int y = Rows - 1; y >= 0; y--)
            {
                if (squares[x, y].Piece == null)
                {
                    var piece = GenerateNewPiece(squares[x, y]);
                    Debug.Log("Placed new piece " + piece.name + " on " + coordStr(x, y));
                }
            }
        }

        return matched.Count > 0;
    }

    public Piece RemovePieceFromSquare(BoardSquare square)
    {
        var piece = square.GetComponentInChildren<Piece>();
        piece.transform.SetParent(null);
        return piece;
    }

    public void PlacePieceOnSquare(Piece piece, BoardSquare square)
    {
        if (square.GetComponentInChildren<Piece>() != null)
        {
            throw new System.Exception("Square already has a Piece!");
        }

        piece.transform.SetParent(square.transform);
        piece.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void MovePieceBetweenSquares(BoardSquare from, BoardSquare to)
    {
        PlacePieceOnSquare(from.Piece, to);
    }

    private Piece GenerateNewPiece(BoardSquare onSquare)
    {
        return GenerateNewPiece(onSquare, (PieceType)Random.Range(0, TypeCount));
    }

    private Piece GenerateNewPiece(BoardSquare onSquare, PieceType type)
    {
        var piece = Instantiate(piecePrefab, onSquare.transform);

        // give piece a name
        if (!pieceTypeCounter.ContainsKey(type))
        {
            pieceTypeCounter.Add(type, 1);
        }
        int count = pieceTypeCounter[type];
        piece.gameObject.name = type + ", " + count;
        pieceTypeCounter[type] = count + 1;

        return piece;
    }

    private static string coordStr(int x, int y)
    {
        return "(" + x + ", " + y + ")";
    }
}
