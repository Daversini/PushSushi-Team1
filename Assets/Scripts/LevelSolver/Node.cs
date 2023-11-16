using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node 
{
    public bool[,] Board;

    public Pawn MainPawn;
    public Pawn[] Pawns;

    public int gCost;
    /// <summary>
    /// number of squares the target block is away from the right side of the board + number of moves to move the blocks blocking the target block
    /// </summary>
    public int hCost;
    public int fCoast;

    public Node CameFrom;

    public Node(Pawn mainPawn, Pawn[] pawns, Node cameFrom, int pawnToMoveID, Vector2Int newPosition)
    {
        MainPawn = mainPawn;
        Pawns = pawns;
        CameFrom = cameFrom;

        if (MainPawn.ID == pawnToMoveID)
            MainPawn.MoveTo(newPosition);
        else
        {
            foreach (Pawn pawn in Pawns)
            {
                if (pawn.ID == pawnToMoveID)
                {
                    pawn.MoveTo(newPosition);
                    break;
                }
            }
        }
        
        UpdateBoard();
    }

    /// <summary>
    /// Initializes the Board based on Pawns and MainPawn positions
    /// </summary>
    public void UpdateBoard()
    {
        Board = new bool[6,6];
        SetBoardCell(MainPawn, true);
        foreach (Pawn pawn in Pawns)
            SetBoardCell(pawn, true);
    }

    /// <summary>
    /// Sets the cell(row, column) of the board equal to value
    /// </summary>
    private void SetBoardCell(int row, int column, bool value) => Board[row, column] = value;

    /// <summary>
    /// Sets the cells occupied by the pawn equal to value
    /// </summary>
    private void SetBoardCell(Pawn pawn, bool value)
    {
        SetBoardCell(pawn.Position.x, pawn.Position.y, value);
        if (pawn.Orientation == Orientation.Horizontal)
        {
            for (int i = 1; i < pawn.Length; i++)
                SetBoardCell(pawn.Position.x, pawn.Position.y + i, value);
        }
        else
        {
            for (int i = 1; i < pawn.Length; i++)
                SetBoardCell(pawn.Position.x + i, pawn.Position.y, value);
        }      
    }

}
