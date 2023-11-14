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

    /// <summary>
    /// Initializes the Board based on Pawns and MainPawn positions
    /// </summary>
    public void InitBoard()
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
        for (int i = 1; i < pawn.Length; i++)
        {
            if (pawn.Orientation == Orientation.Horizontal)
                SetBoardCell(pawn.Position.x, pawn.Position.y + i, value);
            else
                SetBoardCell(pawn.Position.x + i, pawn.Position.y, value);
        }       
    }

    //public int GetHeuristic()
    //{
    //    int heuristic = 0;
    //    int targetColumn = MainPawn.Position.y + MainPawn.Length;

    //    foreach(Pawn pawn in Pawns)
    //    {
    //        //only vartical pawn can block the main pawn
    //        if (pawn.Orientation == Orientation.Horizontal) continue;

    //        int bottom = pawn.Position.x;
    //        int top = pawn.Position.x + pawn.Length - 1;

    //        //check if the pawn is blocking the main pawn
    //        if (top >= Constants.TARGET_ROW && bottom <= Constants.TARGET_ROW && pawn.Position.y >= targetColumn)
    //        {
    //            int movesUp = Constants.TARGET_ROW - bottom + 1;
    //            int movesDown = top - Constants.TARGET_ROW + 1;

    //            if (Constants.BOARD_ROW - (top + 1) < movesUp)
    //                heuristic += movesDown;
    //            else if (bottom < movesDown)
    //                heuristic += movesUp;
    //            else
    //                heuristic += Mathf.Min(movesUp, movesDown);
    //        }
    //    }
    //    heuristic += Constants.BOARD_COLUMN - targetColumn; 
    //    return heuristic;
    //}

}
