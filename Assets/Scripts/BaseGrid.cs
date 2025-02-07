// maebleme2

using System;
using System.Collections.Generic;
using System.Linq;
using Ebleme;
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
        private List<BaseTile> occupiedTiles;

        private void Start()
        {
            occupiedTiles = new List<BaseTile>();
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
                    DrawLine((BaseTile)tile, (BaseTile)right);
                }

                var up = tile.GetNeighbour(Direction.Up);

                if (up)
                {
                    DrawLine((BaseTile)tile, (BaseTile)up);
                }
            }
        }

        private void DrawLine(BaseTile tileA, BaseTile tileB)
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

            line.SetConnectedTiles(tileA, tileB);

            lines.Add(line);
        }

        public void CheckGrid()
        {
            List<int> fullRowIndexes = new List<int>();
            List<int> fullColumnIndexes = new List<int>();


            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.y; i++)
            {
                if (IsFullRow(i))
                {
                    Debug.Log($"Row {i} is full");
                    fullRowIndexes.Add(i);
                }
            }

            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.x; i++)
            {
                if (IsFullColumn(i))
                {
                    Debug.Log($"Column {i} is full");
                    fullColumnIndexes.Add(i);
                }
            }

            List<int> willDestrorRowIndexes = new List<int>();
            List<int> willDestroyColumnIndexes = new List<int>();


            int prevIndex = -10;
            foreach (var rowIndex in fullRowIndexes)
            {
                if (prevIndex + 1 == rowIndex)
                {
                    willDestrorRowIndexes.Add(prevIndex);
                    willDestrorRowIndexes.Add(rowIndex);
                }
                else
                {
                    prevIndex = rowIndex;
                }
            }

            prevIndex = -10;
            foreach (var colIndex in fullColumnIndexes)
            {
                if (prevIndex + 1 == colIndex)
                {
                    willDestroyColumnIndexes.Add(prevIndex);
                    willDestroyColumnIndexes.Add(colIndex);
                }
                else
                {
                    prevIndex = colIndex;
                }
            }

            foreach (var rowIndex in willDestrorRowIndexes)
            {
                for (int colIndex = 0; colIndex < GameConfigs.Instance.BaseGridSize.x; colIndex++)
                {
                    var tile = (BaseTile)gridManager.GetTile(new Vector2Int(colIndex, rowIndex));
                    RemoveOccupied(tile);
                }
            }

            foreach (var colIndex in willDestroyColumnIndexes)
            {
                for (int rowIndex = 0; rowIndex < GameConfigs.Instance.BaseGridSize.y; rowIndex++)
                {
                    var tile = (BaseTile)gridManager.GetTile(new Vector2Int(colIndex, rowIndex));
                    RemoveOccupied(tile);
                }
            }
        }

        private bool IsFullRow(int row)
        {
            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.x; i++)
            {
                var tile = (BaseTile)gridManager.GetTile(new Vector2Int(i, row));

                if (!IsOccupied(tile))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsFullColumn(int column)
        {
            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.y; i++)
            {
                var tile = (BaseTile)gridManager.GetTile(new Vector2Int(column, i));

                if (!IsOccupied(tile))
                {
                    return false;
                }
            }

            return true;
        }

        public void PutItemToGrid(List<BaseTile> baseTilesToHit, List<ItemTile> itemTiles)
        {
            AddOccupiedTiles(baseTilesToHit);

            for (int i = 0; i < baseTilesToHit.Count; i++)
            {
                baseTilesToHit[i].ReColor(ColorTypes.Active);

                var allNeighbours = itemTiles[i].Neighbours;

                foreach (var neighbour in allNeighbours)
                {
                    var tileB = (BaseTile)baseTilesToHit[i].GetNeighbour(neighbour.direction);

                    var line = lines.SingleOrDefault(p => p.Compare(baseTilesToHit[i], tileB));

                    if (line)
                    {
                        line.ReColor(ColorTypes.Active);
                    }
                }
            }
        }

        public void AddOccupiedTiles(List<BaseTile> tiles)
        {
            foreach (var tile in tiles)
            {
                AddOccupiedTile(tile);
            }
        }

        public void AddOccupiedTile(BaseTile tile)
        {
            if (!occupiedTiles.Contains(tile))
            {
                occupiedTiles.Add(tile);
            }
        }

        public bool IsOccupied(BaseTile tile)
        {
            return occupiedTiles.Contains(tile);
        }

        public void RemoveOccupied(BaseTile tile)
        {
            if (IsOccupied(tile))
                occupiedTiles.Remove(tile);
        }
    }
}