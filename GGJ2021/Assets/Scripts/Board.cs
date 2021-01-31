using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PiecesEvent : UnityEvent<HashSet<Piece>> { }

[System.Serializable]
public class Resource
{
    public Piece piece;
    public EnergyBar bar;
    public float weight;
}

public class Board : MonoBehaviour
{
    public int MinimumMatchCount;
    public BoardSquare squarePrefab;
    public Piece piecePrefab;
    public Resource[] resources;
    public BoardType type;

    public PiecesEvent OnPiecesMatched;

    public BoardSquare[,] squares;

    private GridLayoutGroup grid;
    private Dictionary<PieceType, Resource> pieceTypeToResource = new Dictionary<PieceType, Resource>();
    private Dictionary<PieceType, int> pieceTypeCounter = new Dictionary<PieceType, int>();
    private int size;

    private void Awake()
    {
        OnPiecesMatched = new PiecesEvent();
        grid = GetComponent<GridLayoutGroup>();
        EnergyBar[] bars = FindObjectsOfType<EnergyBar>();
        foreach (var r in resources)
        {
            pieceTypeToResource.Add(r.piece.Type, r);
            foreach (var b in bars)
            {
                var ra = b.GetComponentInChildren<ResourceAmount>();
                if (ra != null && ra.type == r.piece.Type)
                {
                    r.bar = b;
                }
            }
        }
    }

    public void ClearBoard()
    {
        //Debug.Log(type + ": " + GetComponentsInChildren<BoardSquare>().Length);
        foreach (var s in GetComponentsInChildren<BoardSquare>())
        {
            Destroy(s.gameObject);
        }
    }

    public void InitializeBoard()
    {
        size = grid.constraintCount;

        squares = new BoardSquare[size, size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                // instantiate board square
                var square = Instantiate(squarePrefab, this.transform);
                squares[x, y] = square;
                square.name = "Square " + coordStr(x, y);

                // randomize piece type without causing matches (brute force)
                PieceType type;
                do
                {
                    type = rollRandomType();
                } while (GetVerticallyConnectedOfType(type, x, y).Count + 1 >= MinimumMatchCount
                || GetHorizontallyConnectedOfType(type, x, y).Count + 1 >= MinimumMatchCount);

                // create new piece
                GenerateNewPiece(square, type);
            }
        }
    }

    public PieceType rollRandomType()
    {
        float total = 0;
        foreach (var r in resources)
        {
            total += r.weight;
        }
        float roll = Random.Range(0, total);
        total = 0;
        PieceType backupType = resources[0].piece.Type;
        foreach (var r in resources)
        {
            total += r.weight;
            if (roll <= total)
            {
                return r.piece.Type;
            }
        }

        return backupType;
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
        for (int i = y + 1; i < size; i++)
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
        for (int i = x + 1; i < size; i++)
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

        //Debug.Log(type + ", size: " + size);

        int x = 0;
        int y = 0;
        bool go = true;
        for (; x < size && go; x++)
        {
            for (; y < size && go; y++)
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
        if (x < size - 1)
        {
            adjacent.Add(squares[x + 1, y]);
        }
        if (y < size - 1)
        {
            adjacent.Add(squares[x, y + 1]);
        }

        return adjacent;
    }

    public bool FindAndNotifyMatches()
    {
        //find all of the matched pieces
        var matched = new HashSet<BoardSquare>();
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
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
            //Debug.Log("Matched " + square.Piece.name + " on " + square.gameObject.name.Replace("Square", ""));
            var piece = RemovePieceFromSquare(square);
            Destroy(piece.gameObject);
        }

        //pull down all non-pieces
        for (int x = 0; x < size; x++)
        {
            for (int y = size - 1; y >= 0; y--)
            {
                if (squares[x, y].Piece == null)
                {
                    BoardSquare aboveSquareWithPiece = null;
                    // find a piece above to pull down
                    for (int j = y - 1; aboveSquareWithPiece == null && j >= 0; j--)
                    {
                        //Debug.Log("Looking at " + coordStr(x, j));
                        if (squares[x, j].Piece != null)
                        {
                            //Debug.Log("Moving " + squares[x, j].Piece.name + coordStr(x, j) + " -> " + coordStr(x, y));
                            MovePieceBetweenSquares(squares[x, j], squares[x, y]);
                            break;
                        }
                    }
                }
            }
        }

        //fill in empty spots
        for (int x = 0; x < size; x++)
        {
            for (int y = size - 1; y >= 0; y--)
            {
                if (squares[x, y].Piece == null)
                {
                    var piece = GenerateNewPiece(squares[x, y]);
                    //Debug.Log("Placed new piece " + piece.name + " on " + coordStr(x, y));
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
        return GenerateNewPiece(onSquare, rollRandomType());
    }

    private Piece GenerateNewPiece(BoardSquare onSquare, PieceType type)
    {

        var piece = Instantiate(pieceTypeToResource[type].piece, onSquare.transform);

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

    public static string coordStr(int x, int y)
    {
        return "(" + x + ", " + y + ")";
    }
}
