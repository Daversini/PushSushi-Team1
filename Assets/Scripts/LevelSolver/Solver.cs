using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver 
{
    

    public static List<Node> FindPath(Node startNode)
    {
        List<Node> path = new List<Node>(); 
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        startNode.gCost = 0;
        startNode.hCost = GetHeuristic(startNode);
        startNode.fCoast = startNode.gCost + startNode.hCost;
















        return path;
    }

    private static int GetHeuristic(Node targetNode)
    {
        int heuristic = 0;
        int targetColumn = targetNode.MainPawn.Position.y + targetNode.MainPawn.Length;

        foreach (Pawn pawn in targetNode.Pawns)
        {
            //only vartical pawn can block the main pawn
            if (pawn.Orientation == Orientation.Horizontal) continue;

            int bottom = pawn.Position.x;
            int top = pawn.Position.x + pawn.Length - 1;

            //check if the pawn is blocking the main pawn
            if (top >= Constants.TARGET_ROW && bottom <= Constants.TARGET_ROW && pawn.Position.y >= targetColumn)
            {
                int movesUp = Constants.TARGET_ROW - bottom + 1;
                int movesDown = top - Constants.TARGET_ROW + 1;

                if (Constants.BOARD_ROW - (top + 1) < movesUp)
                    heuristic += movesDown;
                else if (bottom < movesDown)
                    heuristic += movesUp;
                else
                    heuristic += Mathf.Min(movesUp, movesDown);
            }
        }
        heuristic += Constants.BOARD_COLUMN - targetColumn;
        return heuristic;
    }


}
