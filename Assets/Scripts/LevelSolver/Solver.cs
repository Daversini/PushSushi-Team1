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

        Node currentNode = null;

        while(openList.Count > 0)
        {
            currentNode = GetMinimumNode(openList);

            if (currentNode.MainPawn.Position == Constants.TARGET_POSITION)
                return BuildPath(startNode, currentNode);

            openList.Remove(currentNode);
            closedList.Add(currentNode);
            

        }














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

    private static Node GetMinimumNode(List<Node> openList)
    {
        Node minimumNode = openList[0];

        int f = minimumNode.fCoast;

        foreach(Node node in openList)
        {
            if (node.fCoast < f)
                minimumNode = node;
        }

        return minimumNode;
    }

    private static List<Node> BuildPath(Node startNode, Node lastNode)
    {
        List<Node> path = new List<Node>();
        path.Add(lastNode);

        while(path[path.Count - 1] != startNode)
            path.Add(path[path.Count - 1].CameFrom);
        
        path.Reverse();
        return path;
    }


    private static List<Node> GetNeighbors(Node rootNode)
    {
        List<Node> neighbors = null;

        for(int i = 0; i <= rootNode.Pawns.Length; i++)
        {
            if (i == rootNode.Pawns.Length)
            {
                // analyze the main pawn of rootNode here
            }


        }

        return neighbors;
    }

    private static List<Node> GetNeighborsFromPawn(Pawn pawn)
    {
        List<Node> neighbors = null;

        int availablesMovesCount = 0;

        return neighbors;
    }

    private static List<Vector2Int> GetAvailablesPositions(Node rootNode, Pawn pawn)
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        if (pawn.Orientation == Orientation.Horizontal)
        {
            for(int i = 0; i < pawn.Position.y; i++)
            {
                Vector2Int position = new Vector2Int(pawn.Position.x, i);
                //if position is free (no obstacles on it)
                if (!rootNode.Board[position.x, position.y])
                    availablePositions.Add(position);
            }
            for (int i = pawn.Position.y + pawn.Length; i < Constants.BOARD_COLUMN; i++)
            {
                Vector2Int position = new Vector2Int(pawn.Position.x, i);
                //if position is free (no obstacles on it)
                if (!rootNode.Board[position.x, position.y])
                    availablePositions.Add(position);
            }

        }
        else
        {
            for (int i = 0; i < pawn.Position.x; i++)
            {
                Vector2Int position = new Vector2Int(i, pawn.Position.y);
                if (!rootNode.Board[position.x, position.y])
                    availablePositions.Add(position);
            }
            for (int i = pawn.Position.x + pawn.Length; i < Constants.BOARD_ROW; i++)
            {
                Vector2Int position = new Vector2Int(i, pawn.Position.y);
                if (!rootNode.Board[position.x, position.y])
                    availablePositions.Add(position);
            }
        }

        return availablePositions;
    }


}
