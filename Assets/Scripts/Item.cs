// maebleme2

using System.Collections.Generic;
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
        
        private List<MyTile> tiles = new List<MyTile>();
        private List<Line> lines;

        private Vector3 startScale;
        
        private void Start()
        {
            startScale = transform.localScale;
            
            SetTilesList();
            DrawLines();
        }

        private void DrawLines()
        {
            lines = new List<Line>();
            for (int i = tiles.Count - 1; i >= 0; i--)
            {
                var tile = tiles[i];
                // Your code here
                var right = tile.GetNeighbour(Direction.Right);
            
                if (right && right.gameObject.activeSelf)
                {
                    DrawLine((MyTile)tile, (MyTile)right);
                }
                
                var up = tile.GetNeighbour(Direction.Up);
            
                if (up && up.gameObject.activeSelf)
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
                    var tile = c.GetComponent<MyTile>();
                    if (tile == null) continue;
                    
                    tile.SetItem(this);
                    tiles.Add(tile);
                }
            }
        }

        /// <summary>
        /// Set all tiles position to their start position
        /// </summary>
        public void BackToStartPositionAll()
        {
            SetStartScale();

            foreach (var tile in tiles)
            {
                tile.BackToStartPosition();
            }
        }
        
        public void SetPositionAll()
        {
            foreach (var tile in tiles)
            {
                tile.SetPositionToHit();
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
                var baseTile = hit.transform.GetComponent<MyTile>();
                if (baseTile.OnMyTile)
                {
                    allowSetToGrid = false;
                    break;
                }
            }

            return allowSetToGrid;
        }
    }
}