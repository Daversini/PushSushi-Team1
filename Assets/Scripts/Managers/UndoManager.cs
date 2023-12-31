using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoManager : MonoBehaviour
{
    //private MovesCounterManager movesCounterManager;
    public static event Action OnMoveStored;
    public static event Action OnMoveCanceled;
    
    private Grid<Tile> _grid;
    
    private List<Vector2Int> _storedGridPositions;
    private List<UndoComponent> _storedMovedSushi;
    
    private Vector3 _targetPosition;
    private UndoComponent _targetSushi;
    
    private bool _moveSushi;
    
    public float UndoMovementSpeed = 10f;
    
    private void Awake()
    {
        //movesCounterManager = FindObjectOfType<MovesCounterManager>();
        _grid = new Grid<Tile>(6, 6, 1f, new Vector3(-3f, 0f, -3f), (int x, int y) => new Tile(x, y));
        _storedGridPositions = new List<Vector2Int>();
        _storedMovedSushi = new List<UndoComponent>();
    }

    private void OnEnable()
    {
        UndoComponent.OnStoreMove += StoreMove;
        LevelManager.OnPerformUndo += PerformUndo;
    }

    private void OnDisable()
    {
        UndoComponent.OnStoreMove -= StoreMove;
        LevelManager.OnPerformUndo -= PerformUndo;
    }

    private void Update()
    {
        if (_moveSushi)
            MoveSushi();
    }
    
    private void StoreMove(Vector2Int position, UndoComponent movedSushi)
    {
        //Debug.Log("Move stored");
        _storedGridPositions.Add(position);
        _storedMovedSushi.Add(movedSushi);

        //movesCounterManager.IncreaseMovesCount();
        OnMoveStored?.Invoke();
    }
    
    public void PerformUndo()
    {
        if (_storedGridPositions.Count == 0 || _storedMovedSushi.Count == 0 || _moveSushi) return;

        SoundManager.ButtonSound?.Invoke();

        Vector2Int lastGridPosition = _storedGridPositions[_storedGridPositions.Count - 1];
        
        _targetPosition = _grid.GetWorldPosition(lastGridPosition.x, lastGridPosition.y);
        _targetSushi = _storedMovedSushi[_storedMovedSushi.Count - 1];

        _storedGridPositions.RemoveAt(_storedGridPositions.Count - 1);
        _storedMovedSushi.RemoveAt(_storedMovedSushi.Count - 1);
        
        //movesCounterManager.DecreaseMovesCount();
        OnMoveCanceled?.Invoke();
        _moveSushi = true;
    }
    
    private void MoveSushi()
    {
        if (Vector3.Distance(_targetSushi.transform.position, _targetPosition) > 0.1f)
            _targetSushi.transform.position = Vector3.MoveTowards(_targetSushi.transform.position, _targetPosition, Time.deltaTime * UndoMovementSpeed);
        else
        {
            _targetSushi.transform.position = _targetPosition;
            _moveSushi = false;
        }
    }
}
