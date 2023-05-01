using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineCart : MonoBehaviour
{
    private List<GridTile> _path;
    private int _currentPathIndex;
    private float _t;
    private MineCartSettings _settings;
    private GridTile _currentTile;
    private GridTile _prevTile;
    private bool _reachedHalfWay;
    public ResourceType CurrentRessource { get; private set; }
    public void Initialize(List<GridTile> path, ResourceType ressource)
    {
        _settings = SettingsManager.MineCartSettings;
        _path = path;
        transform.position = _path.First().transform.position;
        _t = 0;
        _currentPathIndex = 1;
        _currentTile = _path[_currentPathIndex];
        _prevTile = _path[0];
        _reachedHalfWay = false;
        CurrentRessource = ressource;
    }
    private void FixedUpdate()
    {
        _t += Time.fixedDeltaTime * _settings.Speed;
        if (_t > 1f)
        {
            if (_currentPathIndex == _path.Count - 1)
            {
                Destroy(gameObject);
            }
            transform.LookAt(_path[_currentPathIndex + 1].transform.position, Vector3.up);
            if (_currentPathIndex < _path.Count - 2)
            {
                if (!(_path[_currentPathIndex + 1].Placeable is RailEntity rail))
                {
                    ExplodeWithDestruction();
                    return;
                }
                if (rail.OccupyingMineCart == null)
                {
                    _prevTile = _currentTile;
                    _currentPathIndex++;
                    _currentTile = _path[_currentPathIndex];
                    _t = 0;
                    RailEntity railEntity = _currentTile.Placeable as RailEntity;
                    railEntity.OccupyingMineCart = this;
                    railEntity.ConnectTiles(_path[_currentPathIndex - 1], _path[_currentPathIndex + 1]);
                }
            }
            else
            {
                ReceiverStation receiver = _path[_currentPathIndex + 1].Placeable as ReceiverStation;
                if (receiver.TryToStoreRessource(CurrentRessource))
                {
                    _prevTile = _currentTile;
                    _currentPathIndex++;
                    _currentTile = _path[_currentPathIndex];
                    _t = 0;
                }
            }
        }
        else
        {
            if (!_reachedHalfWay && _t > 0.5f)
            {
                _reachedHalfWay = true;
                if (_prevTile != null && _prevTile.Placeable is RailEntity)
                    (_prevTile.Placeable as RailEntity).OccupyingMineCart = null;
            }
            if (_currentPathIndex == 1)
            {
                transform.localScale = Vector3.one * _settings.ScaleCurve.Evaluate(_t);
            }
            else if (_currentPathIndex == _path.Count - 1)
            {
                transform.localScale = Vector3.one * _settings.ScaleCurve.Evaluate(1 - _t);
            }
            transform.position = Vector3.Lerp(_prevTile.transform.position, _currentTile.transform.position, _t);
        }
    }


    public void ExplodeWithDestruction()
    {
        // TODO: Play particle effect
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_currentTile != null)
            (_currentTile.Placeable as RailEntity).OccupyingMineCart = null;
        if (_prevTile != null)
            (_prevTile.Placeable as RailEntity).OccupyingMineCart = null;
    }
}