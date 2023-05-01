using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineCart : MonoBehaviour
{
    private List<GridTile> _path;
    private int _currentPathIndex;
    private float t;
    public void Initialize(List<GridTile> path)
    {
        _path = path;
        transform.position = _path.First().transform.position;
        t = 0.5f;
    }
    private void FixedUpdate()
    {

    }

    // private IEnumerator SpawnAnimation()
    // {
    //     transform.localScale = Vector3.zero;

    // }


    // private void SpawnAnimation


    private void OnDestroy()
    {

    }
}