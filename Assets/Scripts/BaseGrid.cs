// maebleme2

using System;
using System.Collections.Generic;
using Ebleme.ColowSwapMaddness;
using Ebleme.Utility;
using StickBlast.Grid;
using UnityEngine;

namespace StickBlast
{
    public class BaseGrid : Singleton<BaseGrid>
    {
        [SerializeField]
        private GridManager gridManager;

        [SerializeField]
        private Line linePrefab;

        [SerializeField]
        private Transform linesContent;

        private List<Line> lines;

        private void Start()
        {
            DrawLines();
        }

        private void DrawLines()
        {
            lines = new List<Line>();
            for (int i = gridManager.Tiles.Count - 1; i >= 0; i--)
            {
                var tile = gridManager.Tiles[i];
                // Your code here
                var right = tile.GetNeighbour(Direction.Right);

                if (right)
                {
                    DrawLine((MyTile)tile, (MyTile)right);
                }

                var up = tile.GetNeighbour(Direction.Up);

                if (up)
                {
                    DrawLine((MyTile)tile, (MyTile)up);
                }
            }
        }

        private void DrawLine(MyTile tileA, MyTile tileB)
        {
            if (tileA == null || tileB == null) return;
            
            Transform pointA = tileA.transform, pointB = tileB.transform;

            float distance = Vector2.Distance(pointA.position, pointB.position);

            var line = Instantiate(linePrefab, linesContent);
            line.transform.position = (pointA.position + pointB.position) / 2;

            Vector2 direction = pointB.position - pointA.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            line.transform.rotation = Quaternion.Euler(0, 0, angle);

            Vector3 scale = line.transform.localScale;
            scale.x = distance / line.GetComponent<SpriteRenderer>().bounds.size.x;
            line.transform.localScale = scale;
            
            lines.Add(line);
        }

        public void CheckGrid()
        {
            List<int> willDestroyRowIndexes = new List<int>();
            List<int> willDestroyColumnIndexes = new List<int>();

            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.x; i++)
            {
                if (IsFullRow(i))
                {
                    Debug.Log($"Row {i} is full");
                    willDestroyRowIndexes.Add(i);
                }

                if (IsFullColumn(i))
                {
                    Debug.Log($"Column {i} is full");
                    willDestroyColumnIndexes.Add(i);
                }
            }

            foreach (var rowIndex in willDestroyRowIndexes)
            {
                for (int colIndex = 0; colIndex < GameConfigs.Instance.BaseGridSize.x; colIndex++)
                {
                    var tile = (MyTile)gridManager.GetTile(new Vector2Int(colIndex, rowIndex));
                    if (tile.OnMyTile)
                    {
                        Destroy(tile.OnMyTile.gameObject);
                    }
                }
            }

            foreach (var colIndex in willDestroyColumnIndexes)
            {
                for (int rowIndex = 0; rowIndex < GameConfigs.Instance.BaseGridSize.y; rowIndex++)
                {
                    var tile = (MyTile) gridManager.GetTile(new Vector2Int(colIndex, rowIndex));
                    if (tile && tile.OnMyTile)
                    {
                        Destroy(tile.OnMyTile.gameObject);
                    }
                }
            }
        }
        
        private bool IsFullRow(int row)
        {
            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.y; i++)
            {
                var tile = (MyTile)gridManager.GetTile(new Vector2Int(i, row));

                if (!tile.OnMyTile)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsFullColumn(int column)
        {
            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.x; i++)
            {
                var tile = (MyTile)gridManager.GetTile(new Vector2Int(column, i));

                if (!tile.OnMyTile)
                {
                    return false;
                }
            }

            return true;
        }
    }
}