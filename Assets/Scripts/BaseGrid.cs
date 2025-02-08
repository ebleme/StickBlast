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
        private BaseLine linePrefab;

        [SerializeField]
        private Transform linesContent;

        private List<BaseLine> lines;
        private List<BaseTileOccupation> occupiedTiles;

        private void Start()
        {
            occupiedTiles = new List<BaseTileOccupation>();
            DrawLines();
        }

        private void DrawLines()
        {
            lines = new List<BaseLine>();
            for (int i = gridManager.Tiles.Count - 1; i >= 0; i--)
            {
                var tile = gridManager.Tiles[i];

                var right = tile.GetNeighbour(Direction.Right);
                if (right)
                    DrawLine((BaseTile)tile, (BaseTile)right, LineDirection.Horizontal);

                var up = tile.GetNeighbour(Direction.Up);
                if (up)
                    DrawLine((BaseTile)tile, (BaseTile)up, LineDirection.Vertical);
            }
        }

        private void DrawLine(BaseTile tileA, BaseTile tileB, LineDirection lineDirection)
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

            line.Set(tileA.coordinate, lineDirection, tileA, tileB);

            lines.Add(line);
        }

        #region Check grid fullness

        public void CheckGrid()
        {
            HashSet<int> fullRowHorizontalIndexes = new HashSet<int>();
            HashSet<int> fullRowVerticalIndexes = new HashSet<int>();
            
            HashSet<int> fullColumnVerticalIndexes = new HashSet<int>();
            HashSet<int> fullColumnHorizontalIndexes = new HashSet<int>();


            CheckHorizontal();
            return;

            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.y; i++)
            {
                if (IsFullRowHorizontal(i)) // Row full both horizontal
                {
                    Debug.Log($"Row {i} is full");
                    fullRowHorizontalIndexes.Add(i);
                }
                
                if (IsFullRowVertical(i)) // Row full both vertical
                {
                    Debug.Log($"Row {i} is full");
                    fullRowVerticalIndexes.Add(i);
                }
            }

            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.x; i++)
            {
                if (IsFullVerticalColumn(i))
                {
                    Debug.Log($"Column {i} is full");
                    fullColumnVerticalIndexes.Add(i);
                }
                
                if (IsFullHorizontalColumn(i))
                {
                    Debug.Log($"Column {i} is full");
                    fullColumnHorizontalIndexes.Add(i);
                }
            }

            // Benzersiz silinecek indeksleri belirle
            HashSet<int> willDestroyRowIndexes = new HashSet<int>();
            HashSet<int> willDestroyColumnIndexes = new HashSet<int>();


            // Yatay
            int prevIndex = -10;

            foreach (var index in fullRowHorizontalIndexes)
            {
                if (prevIndex + 1 == index)
                {
                    if (fullRowVerticalIndexes.Contains(prevIndex))
                    {
                        willDestroyRowIndexes.Add(prevIndex);
                        willDestroyRowIndexes.Add(index);
                        willDestroyColumnIndexes.Add(prevIndex);
                    }
                }
                else
                {
                    prevIndex = index;
                }
            }
            
            // for (int i = 0; i < fullRowHorizontalIndexes.Count; i++)
            // {
            //     if (prevIndex + 1 == i)
            //     {
            //         if (fullColumnHorizontalIndexes.Contains(prevIndex))
            //         {
            //             willDestroyRowIndexes.Add(prevIndex);
            //             willDestroyRowIndexes.Add(i);
            //             willDestroyColumnIndexes.Add(prevIndex);
            //         }
            //     }
            //     else
            //     {
            //         prevIndex = fullRowHorizontalIndexes[i];
            //     }
            // }

            // Dikey
            prevIndex = -10;
            
            foreach (var index in fullColumnVerticalIndexes)
            {
                if (prevIndex + 1 == index)
                {
                    if (fullColumnHorizontalIndexes.Contains(prevIndex))
                    {
                        willDestroyColumnIndexes.Add(prevIndex);
                        willDestroyColumnIndexes.Add(index);
                        willDestroyRowIndexes.Add(prevIndex);
                    }
                }
                else
                {
                    prevIndex = index;
                }
            }


            // Satırları temizle
            foreach (var rowIndex in willDestroyRowIndexes)
            {
                foreach (var colIndex in willDestroyColumnIndexes)
                {
                    var tile = (BaseTile)gridManager.GetTile(new Vector2Int(colIndex, rowIndex));
                    tile.ReColor(ColorTypes.Passive);

                    var line = GetLine(colIndex, rowIndex, LineDirection.Horizontal);
                    if (line)
                        line.RemoveOccupied();
                }
            }

            // Sütunları temizle
            foreach (var colIndex in willDestroyColumnIndexes)
            {
                foreach (var rowIndex in willDestroyRowIndexes)
                {
                    var tile = (BaseTile)gridManager.GetTile(new Vector2Int(colIndex, rowIndex));
                    tile.ReColor(ColorTypes.Passive);

                    var line = GetLine(colIndex, rowIndex, LineDirection.Vertical);
                    if (line)
                        line.RemoveOccupied();
                }
            }
        }


        private void CheckHorizontal()
        {
            HashSet<BaseLine> linesToRemove = new HashSet<BaseLine>();
            HashSet<BaseTile> tilesToRemove = new HashSet<BaseTile>();
            
            for (int row = 0; row < GameConfigs.Instance.BaseGridSize.y; row++)
            {
                if (IsFullRowHorizontal(row) && IsFullRowVertical(row)) // Row full both horizontal
                {
                    if(row+1 < GameConfigs.Instance.BaseGridSize.y)
                        if (IsFullRowHorizontal(row+1)) // Row full both horizontal
                        {
                            Debug.Log($"Row {row} to row {row+1} is full");

                            var horizontalLines = GetHorizontalLines(row);
                            var verticalLines = GetVerticalLines(row);
                            var horizontalLines2 = GetHorizontalLines(row+1);

                            foreach (var line in horizontalLines)
                                linesToRemove.Add(line);
                            
                            foreach (var line in verticalLines)
                                linesToRemove.Add(line);
                            
                            foreach (var line in horizontalLines2)
                                linesToRemove.Add(line);

                            var tiles1 = GetHorizontalTiles(row);
                            var tiles2 = GetHorizontalTiles(row +1);

                            foreach (var tile in tiles1)
                                tilesToRemove.Add(tile); 
                            
                            foreach (var tile in tiles2)
                                tilesToRemove.Add(tile);
                        }
                }
            }

            foreach (var line in linesToRemove)
                line.RemoveOccupied();
            
            foreach (var tile in tilesToRemove)
                tile.ReColor(ColorTypes.Passive);
        }
        
        private bool IsFullRowHorizontal(int row)
        {
            // Yatay
            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.x - 1; i++)
            {
                var line = GetLine(i, row, LineDirection.Horizontal);
                if (line == null || !line.IsOccupied) return false;
            }
            

            return true;
        }
        
        
        private HashSet<BaseLine> GetHorizontalLines(int row)
        {
            // Yatay
            HashSet<BaseLine> lines = new HashSet<BaseLine>();
            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.x - 1; i++)
            {
                var line = GetLine(i, row, LineDirection.Horizontal);
                lines.Add(line);
            }

            return lines;
        } 
        
        private HashSet<BaseLine> GetVerticalLines(int row)
        {
            // Yatay
            HashSet<BaseLine> lines = new HashSet<BaseLine>();
            
            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.x; i++)
            {
                var line = GetLine(i, row, LineDirection.Vertical);
                lines.Add(line);
            }

            return lines;
        }

        
        private HashSet<BaseTile> GetHorizontalTiles(int row)
        {
            // Yatay
            HashSet<BaseTile> tiles = new HashSet<BaseTile>();
            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.x; i++)
            {
                var tile = GetTile(i, row);
                tiles.Add(tile);
            }

            return tiles;
        }
        
        private bool IsFullRowVertical(int row)
        {
            // Dikey
            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.x; i++)
            {
                var line = GetLine(i, row, LineDirection.Vertical);
                if (line == null || !line.IsOccupied) return false;
            }
            return true;
        }
        
        private bool IsFullVerticalColumn(int column)
        {
            // Dikey
            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.y - 1; i++)
            {
                var line = GetLine(column, i, LineDirection.Vertical);
                if (line == null || !line.IsOccupied) return false;
            }
            
            return true;
        }
        
        private bool IsFullHorizontalColumn(int column)
        {
            // Yatay
            for (int i = 0; i < GameConfigs.Instance.BaseGridSize.y; i++)
            {
                var line = GetLine(column, i, LineDirection.Horizontal);
                if (line == null || !line.IsOccupied) return false;
            }
            return true;
        }
        
        private BaseLine GetLine(int x, int y, LineDirection direction)
        {
            return lines.SingleOrDefault(p => p.coordinate == new Vector2Int(x, y) && p.lineDirection == direction);
        }

        private BaseTile GetTile(int x, int y)
        {
            return (BaseTile)gridManager.Tiles.SingleOrDefault(p => p.coordinate == new Vector2Int(x, y));
        }

        #endregion

        public void PutItemToGrid(List<BaseTile> baseTilesToHit, List<ItemTile> itemTiles)
        {
            // AddOccupiedTiles(baseTilesToHit);

            for (int i = 0; i < baseTilesToHit.Count; i++)
            {
                baseTilesToHit[i].ReColor(ColorTypes.Active);

                // var allNeighbours = itemTiles[i].Neighbours;

                // foreach (var neighbour in allNeighbours)
                // {
                //     var tileB = (BaseTile)baseTilesToHit[i].GetNeighbour(neighbour.direction);
                //
                //     var line = lines.SingleOrDefault(p => p.Compare(baseTilesToHit[i], tileB));
                //
                //     if (line)
                //     {
                //         line.ReColor(ColorTypes.Active);
                //     }
                // }
            }
        }

        // public void AddOccupiedTiles(List<BaseTile> tiles)
        // {
        //     foreach (var tile in tiles)
        //     {
        //         AddOccupiedTile(tile);
        //     }
        // }
        //
        // public void AddOccupiedTile(BaseTile tile)
        // {
        //     if (!IsOccupationExist(tile))
        //     {
        //         occupiedTiles.Add(new BaseTileOccupation(tile, tile.Neighbours.Count));
        //     }
        //     else
        //     {
        //         var occupation = GetOccupation(tile);
        //         occupation.ConnectionCount++;
        //     }
        // }
        //
        // public bool IsOccupationExist(BaseTile tile)
        // {
        //     var occupiedTile = GetOccupation(tile);
        //
        //     return occupiedTile != null;
        // }
        //
        // public bool IsOccupied(BaseTile tile)
        // {
        //     var occupiedTile = GetOccupation(tile);
        //
        //     if (occupiedTile == null || occupiedTile.ConnectionCount < GameConfigs.Instance.BaseGridMaxOccupation)
        //         return false;
        //
        //     return true;
        // }
        //
        // public int GetOccupatableCount(BaseTile tile)
        // {
        //     var occupiedTile = GetOccupation(tile);
        //
        //     if (occupiedTile == null)
        //         return GameConfigs.Instance.BaseGridMaxOccupation;
        //
        //     return GameConfigs.Instance.BaseGridMaxOccupation - occupiedTile.ConnectionCount;
        // }
        //
        // private BaseTileOccupation GetOccupation(BaseTile tile)
        // {
        //     return occupiedTiles.SingleOrDefault(p => p.BaseTile == tile);
        // }
        //
        // public void RemoveOccupied(BaseTile tile)
        // {
        //     var occupiedTile = GetOccupation(tile);
        //
        //     if (occupiedTile != null)
        //         occupiedTiles.Remove(occupiedTile);
        // }
    }
}