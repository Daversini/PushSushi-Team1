using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver 
{
    

    public static List<Node> FindPath(Node startNode)
    { 
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        startNode.gCost = 0;
        startNode.hCost = GetHeuristic(startNode);
        startNode.fCoast = startNode.gCost + startNode.hCost;

        Node currentNode;

        while(openList.Count > 0)
        {
            currentNode = GetMinimumNode(openList);

            //Checks if the end node was reached
            if (currentNode.MainPawn.Position == Constants.TARGET_POSITION)
                return BuildPath(startNode, currentNode);

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<Node> neighbors = GetAllNeighbors(currentNode);
            //for each neighbor of currentNode 
            for (int i = 0; i < neighbors.Count; i++)
            {
                //ignores already explored nodes
                if (closedList.Contains(neighbors[i]))
                    continue;

                int newGcost = currentNode.gCost + neighbors[i].PartialGCost;
                if (!openList.Contains(neighbors[i]) || newGcost < neighbors[i].gCost)
                {
                    neighbors[i].gCost = newGcost;
                    neighbors[i].hCost = GetHeuristic(neighbors[i]);
                    neighbors[i].fCoast = neighbors[i].gCost + neighbors[i].hCost;

                    if (!openList.Contains(neighbors[i]))
                        openList.Add(neighbors[i]);
                }
            }
        }
        //if openList is empty no path was found, return null
        Debug.Log("Path Not Found");
        return null;
    }

    /// <summary>
    /// return the H cost ( H = number of squares the target block is away from the right side of the board + number of moves to move the blocks blocking the target block)
    /// </summary>
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


    public static List<Node> GetAllNeighbors(Node rootNode)
    {
        List<Node> neighbors = new List<Node>();

        AddNeighbors(neighbors, rootNode, rootNode.MainPawn);

        foreach (Pawn pawn in rootNode.Pawns)
            AddNeighbors(neighbors, rootNode, pawn);


        return neighbors;
    }

    private static void AddNeighbors(List<Node> neighbors, Node rootNode, Pawn pawn)
    {
        List<Vector2Int> availablesPosition = GetAvailablesPositions(rootNode, pawn);
        int partialGCost;
        foreach(Vector2Int position in availablesPosition)
        {
            partialGCost = (int)Vector2Int.Distance(pawn.Position, position);
            neighbors.Add(new Node(rootNode.Board, rootNode.MainPawn, rootNode.Pawns, partialGCost, rootNode, pawn.ID, position));
        }
    }



    private static List<Vector2Int> GetAvailablesPositions(Node rootNode, Pawn pawn)
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        if (pawn.Orientation == Orientation.Horizontal)
            AddAvailablesPositionsHorizontal(availablePositions, rootNode, pawn);
        else
            AddAvailablesPositionsVertical(availablePositions, rootNode, pawn);
        
        return availablePositions;
    }

    private static void AddAvailablesPositionsHorizontal(List<Vector2Int> availablePositions, Node rootNode, Pawn pawn)
    {
        int firstOccupiedColumnRight = GetFirstOccupiedColumnRight(rootNode, pawn);
        int firstOccupiedColumnLeft = GetFirstOccupiedColumnLeft(rootNode, pawn);

        for (int i = firstOccupiedColumnRight + 1; i < pawn.Position.y; i++)
            availablePositions.Add(new Vector2Int(pawn.Position.x, i));
        for (int i = pawn.Position.y + 1; i <= firstOccupiedColumnLeft - pawn.Length; i++)
            availablePositions.Add(new Vector2Int(pawn.Position.x, i));
    }

    private static void AddAvailablesPositionsVertical(List<Vector2Int> availablePositions, Node rootNode, Pawn pawn)
    {
        int firstOccupiedRowDown = GetFirstOccupiedRowDown(rootNode, pawn);
        int firstOccupiedRowUp = GetFirstOccupiedRowUp(rootNode, pawn);

        for (int i = firstOccupiedRowDown + 1; i < pawn.Position.x; i++)
            availablePositions.Add(new Vector2Int(i, pawn.Position.y));
        for (int i = pawn.Position.x + 1; i <= firstOccupiedRowUp - pawn.Length; i++)
            availablePositions.Add(new Vector2Int(i, pawn.Position.y));
    }

    private static int GetFirstOccupiedColumnRight(Node rootNode, Pawn pawn)
    {
        for (int i = pawn.Position.y - 1; i >= 0; i--)
        {
            if (rootNode.Board[pawn.Position.x, i])
                return i;
        }
        return -1;
    }

    private static int GetFirstOccupiedColumnLeft(Node rootNode, Pawn pawn)
    {
        for (int i = pawn.Position.y + pawn.Length; i < Constants.BOARD_COLUMN; i++)
        {
            if (rootNode.Board[pawn.Position.x, i])
                return i;
        }
        return Constants.BOARD_COLUMN;
    }

    private static int GetFirstOccupiedRowDown(Node rootNode, Pawn pawn)
    {
        for (int i = pawn.Position.x - 1; i >= 0; i--)
        {
            if (rootNode.Board[i, pawn.Position.y])
                return i;
        }
        return -1;
    }

    private static int GetFirstOccupiedRowUp(Node rootNode, Pawn pawn)
    {
        for (int i = pawn.Position.x + pawn.Length; i < Constants.BOARD_ROW; i++)
        {
            if (rootNode.Board[i, pawn.Position.y])
                return i;
        }
        return Constants.BOARD_ROW;
    }

}
