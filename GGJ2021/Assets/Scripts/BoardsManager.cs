using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoardType
{
    Main, UnexploredTerrain, MediumUnexplored, HardUnexploredTerrain
}

public class BoardsManager : MonoBehaviour
{
    private Board mainBoard;
    private Board[] boards;
    private PieceDragging pieceDragging;

    void Start()
    {
        pieceDragging = FindObjectOfType<PieceDragging>();
        boards = FindObjectsOfType<Board>();
        foreach (var b in boards)
        {
            if (b.type != BoardType.Main)
            {
                b.gameObject.SetActive(false);
            }
        }

        mainBoard = FindMainBoard();
        if (mainBoard == null)
        {
            throw new System.Exception("Could not find Main Board");
        }
        mainBoard.InitializeBoard();
    }

    public static Board FindMainBoard()
    {
        foreach (var b in FindObjectsOfType<Board>())
        {
            if (b.type == BoardType.Main)
            {
                return b;
            }
        }

        throw new System.Exception("Could not find Main Board");
    }

    public void SwitchToBoard(BoardType type)
    {
        pieceDragging.DropPiece(false);
        foreach (var b in boards)
        {
            if (b.type == type)
            {
                Debug.Log("Setting Active " + b.type);
                b.gameObject.SetActive(true);
                if (type != BoardType.Main)
                {
                    b.InitializeBoard();
                }
            }
            else
            {
                if (b.type != BoardType.Main)
                {
                    b.ClearBoard();
                }
                //Debug.Log("Setting Inactive " + b.type);
                b.gameObject.SetActive(false);
            }
        }
    }

    public void SwitchToMain()
    {
        SwitchToBoard(BoardType.Main);
    }

    public void SwitchToUnexplore()
    {
        SwitchToBoard(BoardType.UnexploredTerrain);
    }

    public void SwitchToMediumUnexplore()
    {
        SwitchToBoard(BoardType.MediumUnexplored);
    }

    public void SwitchToHardUnexplore()
    {
        SwitchToBoard(BoardType.HardUnexploredTerrain);
    }
}
