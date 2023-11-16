using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Debugger : MonoBehaviour
{
    public Node testNode;
    //public Node testNode2;
    public bool printBoard;
    Grid<Tile> _grid = new Grid<Tile>(6, 6, 1f, new Vector3(-3f, 0f, -3f), (int x, int y) => new Tile(x, y));

    List<Node> test;
    Dictionary<int, Color> boh = new Dictionary<int, Color>();

    private void Awake()
    {
        testNode.UpdateBoard();
        //testNode2.UpdateBoard();
        //test.Add(testNode);
        //List<Vector2Int> test = Solver.GetAvailablesPositions(testNode, testNode.MainPawn);
        //foreach (Vector2Int i in test)
        //    Debug.Log(i);

        //test = Solver.GetAllNeighbors(testNode);
        //Debug.Log(test.Count);
        //testNode.UpdateBoard();

        //Debug.Log(testNode.Equals(testNode2));
        //Debug.Log(test.Contains(testNode2));
        //Profiler.BeginSample("FindPath");
        //test = Solver.FindPath(testNode);
        //Profiler.EndSample();

        //Debug.Log(test.Count);
        //test = Solver.GetAllNeighbors(testNode);
        foreach (var i in testNode.Pawns)
            boh.Add(i.ID, GetRandomColor());

    }
    public int pippo;
    private void Update()
    {
        Profiler.BeginSample("MY SAMPLE");
        for(int i = 0; i < pippo; i++)
            test = Solver.GetAllNeighbors(testNode);
        Profiler.EndSample();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (printBoard)
                PrintBoard(testNode/*.Board*/);
            else
                PrintBoard(test[i]/*.Board*/);
        }
    }

    [Range(0, 20)]
    public int i;

    public void PrintBoard(bool[,] boardToPrint)
    {
        if (boardToPrint == null) return;

        for (int row = 0; row < 6; row++)
            for (int column = 0; column < 6; column++)
                if (boardToPrint[row, column])
                    Gizmos.DrawSphere(_grid.GetWorldPosition(column, row), 0.1f);
    }

    public void PrintBoard(Node nodeToPrint)
    {
        for(int j = 0; j < nodeToPrint.Pawns.Length; j++)
        {
            Gizmos.color = boh[nodeToPrint.Pawns[j].ID];
            if(nodeToPrint.Pawns[j].Orientation == Orientation.Horizontal)
            {
                for(int i = nodeToPrint.Pawns[j].Position.y; i < nodeToPrint.Pawns[j].Position.y + nodeToPrint.Pawns[j].Length; i++)
                {
                    Gizmos.DrawSphere(_grid.GetWorldPosition(i, nodeToPrint.Pawns[j].Position.x), 0.1f);
                }
            }
            else
            {
                for (int i = nodeToPrint.Pawns[j].Position.x; i < nodeToPrint.Pawns[j].Position.x + nodeToPrint.Pawns[j].Length; i++)
                {
                    Gizmos.DrawSphere(_grid.GetWorldPosition(nodeToPrint.Pawns[j].Position.y, i), 0.1f);
                }
            }
        }

        Gizmos.DrawSphere(_grid.GetWorldPosition(nodeToPrint.MainPawn.Position.y, nodeToPrint.MainPawn.Position.x), 0.1f);
        Gizmos.DrawSphere(_grid.GetWorldPosition(nodeToPrint.MainPawn.Position.y + 1, nodeToPrint.MainPawn.Position.x), 0.1f);
    }

    private Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
    }
}
