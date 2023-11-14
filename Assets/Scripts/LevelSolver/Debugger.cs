using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public Node testNode;
    public bool printBoard;
    Grid<Tile> _grid = new Grid<Tile>(6, 6, 1f, new Vector3(-3f, 0f, -3f), (int x, int y) => new Tile(x, y));

    private void Awake()
    {
        testNode.InitBoard();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && printBoard)
            PrintBoard(testNode.Board);
    }

    public void PrintBoard(bool[,] boardToPrint)
    {
        if (boardToPrint == null) return;

        for (int row = 0; row < 6; row++)
            for (int column = 0; column < 6; column++)
                if (boardToPrint[row, column])
                    Gizmos.DrawSphere(_grid.GetWorldPosition(column, row), 0.1f);
    }
}
