using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Pawn
{
    public int ID;
    public int Length;
    public Orientation Orientation;
    /// <summary>
    /// x = row, y = column
    /// </summary>
    public Vector2Int Position;

    public Pawn(int id, int length, Orientation orientation, Vector2Int position)
    {
        ID = id;
        Length = length;
        Orientation = orientation;
        Position = position;
    }

    public void MoveTo(Vector2Int newPosition) => Position = newPosition;
}


