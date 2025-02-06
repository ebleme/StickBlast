// maebleme2

using System.Collections.Generic;
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
        private Vector2Int gridSize;

        
        public void CheckGrid()
        {
            List<int> willDestroyRowIndexes = new List<int>();
            List<int> willDestroyColumnIndexes = new List<int>();

            for (int i = 0; i < gridSize.x; i++)
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
                for (int colIndex = 0; colIndex < gridSize.x; colIndex++)
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
                for (int rowIndex = 0; rowIndex < gridSize.y; rowIndex++)
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
            for (int i = 0; i < gridSize.y; i++)
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
            for (int i = 0; i < gridSize.x; i++)
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