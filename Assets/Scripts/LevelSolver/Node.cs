using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : IEquatable<Node> //Defines a generalized method that a value type or class implements to create a type-specific method for determining equality of instances. (from C# documentation)
{
    public bool[,] Board;

    public Pawn MainPawn;
    public Pawn[] Pawns;

    public int gCost;
    public int hCost;
    public int fCoast;

    public int PartialGCost;

    public Node CameFrom;

    public Node(bool[,] board, Pawn mainPawn, Pawn[] pawns, int partialGCost, Node cameFrom, int pawnToMoveID, Vector2Int newPosition)
    {
        Board = board;
        MainPawn = mainPawn;
        Pawns = GetPawns(pawns);
        PartialGCost = partialGCost;
        CameFrom = cameFrom;

        //gCost = 999999;
        //fCoast = gCost + hCost;

        if (MainPawn.ID == pawnToMoveID)
        {
            MainPawn.MoveTo(newPosition);
        }
        else
        {
            for(int i = 0; i < Pawns.Length; i++)
            {
                if (Pawns[i].ID == pawnToMoveID)
                {
                    Pawns[i].MoveTo(newPosition);
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
        {
            SetBoardCell(pawn, true);
        }
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

    /// <summary>
    /// return a copy of the passed array original
    /// </summary>
    private Pawn[] GetPawns(Pawn[] original)
    {
        Pawn[] copy = new Pawn[original.Length];
        for (int i = 0; i < original.Length; i++)
            copy[i] = original[i];
        return copy;
    }




    public bool Equals(Node other) => this == other;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
    public static bool operator ==(Node node1, Node node2)
    {
        for (int i = 0; i < Constants.BOARD_ROW; i++)
        {
            for (int j = 0; j < Constants.BOARD_COLUMN; j++)
            {
                if (node1.Board[i, j] != node2.Board[i, j])
                    return false;
            }
        }
        return true;
    }
    public static bool operator !=(Node node1, Node node2)
    {
        for (int i = 0; i < Constants.BOARD_ROW; i++)
        {
            for (int j = 0; j < Constants.BOARD_COLUMN; j++)
            {
                if (node1.Board[i, j] != node2.Board[i, j])
                    return true;
            }
        }
        return false;
    }

}
