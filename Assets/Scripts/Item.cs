// maebleme2

using System;
using System.Collections.Generic;
using Ebleme.ColowSwapMaddness;
using StickBlast.Grid;
using UnityEngine;

namespace StickBlast
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private Line linePrefab;

        [SerializeField]
        private Transform linesContent;
        
        [SerializeField]
        private Vector3 movingScale = Vector3.one;
        
        private List<ItemTile> tiles = new List<ItemTile>();
        private List<Line> lines;

        private Vector3 startScale;
        private Vector3 startPosition;
        
        private void Start()
        {
            startScale = transform.localScale;
            startPosition = transform.position;
            
            SetTilesList();
            DrawLines();
        }

        private void DrawLines()
        {
            foreach (Transform c in linesContent)
                Destroy(c.gameObject);
            
            lines = new List<Line>();
            for (int i = tiles.Count - 1; i >= 0; i--)
            {
                var tile = tiles[i];
                // Your code here
                var right = tile.GetNeighbour(Direction.Right);
            
                if (right && right.gameObject.activeSelf)
                {
                    DrawLine((ItemTile)tile, (ItemTile)right);
                }
                
                var up = tile.GetNeighbour(Direction.Up);
            
                if (up && up.gameObject.activeSelf)
                {
                    DrawLine((ItemTile)tile, (ItemTile)up);
                }
            }
        }
        
        private void DrawLine(ItemTile tileA, ItemTile tileB)
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

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
        
        public void SetMovingScale()
        {
            transform.localScale = movingScale;
        }
        
        public void SetStartScale()
        {
            transform.localScale = startScale;
        }
        
        private void SetTilesList()
        {
            foreach (Transform c in transform)
            {
                if (c.gameObject.activeSelf)
                {
                    var tile = c.GetComponent<ItemTile>();
                    if (tile == null) continue;
                    
                    tile.SetItem(this);
                    tiles.Add(tile);
                }
                else
                {
                    Destroy(c.gameObject);
                }
            }
        }

        /// <summary>
        /// Set all tiles position to their start position
        /// </summary>
        public void ReleaseAll()
        {
            SetStartScale();

            transform.position = startPosition;
            
            foreach (var tile in tiles)
            {
                tile.BackToStartPosition();
            }

            foreach (var line in lines)
            {
                line.BackToStartPosition();
            }
        }
        
        public void SetPositionAll()
        {
            foreach (var tile in tiles)
            {
                tile.SetPositionToHit();
            }
            
            DrawLines();
            // ReColorLines(LineStatus.Active);
        }

        private void ReColorLines(LineStatus lineStat)
        {
            switch (lineStat)
            {
                case LineStatus.Passive:
                    foreach (var line in lines)
                        line.Passive();
                    break;
                case LineStatus.Hover:
                    foreach (var line in lines)
                        line.Hover();
                    break;
                case LineStatus.Active:
                    foreach (var line in lines)
                        line.Active();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lineStat), lineStat, null);
            }
        }

        public bool AllowSetToGrid()
        {
            var allowSetToGrid = true;

            foreach (var tile in tiles)
            {
                var hit = tile.Moveable.Hit();
                if (!hit)
                {
                    allowSetToGrid = false;
                    break;
                }

                // base tile ın üstünde herhangi birşey var mı kontrolü
                var baseTile = hit.transform.GetComponent<BaseTile>();
                if (baseTile.OnMyTile)
                {
                    allowSetToGrid = false;
                    break;
                }
            }

            if (allowSetToGrid)
                ReColorLines(LineStatus.Hover);
            else
                ReColorLines(LineStatus.Passive);
            
            return allowSetToGrid;
        }
    }
}