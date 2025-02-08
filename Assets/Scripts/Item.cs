// maebleme2

using System;
using System.Collections.Generic;
using System.Linq;
using StickBlast.Grid;
using UnityEngine;

namespace StickBlast
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private List<ItemLine> lines;

        [SerializeField]
        private ItemLine linePrefab;

        [SerializeField]
        private Transform linesContent;

        [SerializeField]
        private Vector3 movingScale = Vector3.one;

        private List<ItemTile> tiles = new List<ItemTile>();


        private Vector3 startScale;
        private Vector3 startPosition;

        private void Start()
        {
            startScale = transform.localScale;
            startPosition = transform.position;

            SetTilesList();
            DrawLines();
            Recolor(ColorTypes.ItemStill);
        }


        private void Recolor(ColorTypes colorType)
        {
            RecolorItems(colorType);
            RecolorLines(colorType);
        }


        private void RecolorItems(ColorTypes colorType)
        {
            foreach (var tile in tiles)
            {
                tile.ReColor(colorType);
            }
        }

        private void RecolorLines(ColorTypes colorType)
        {
            foreach (var line in lines)
            {
                line.ReColor(colorType);
            }
        }

        private void DrawLines()
        {
            foreach (Transform c in linesContent)
                Destroy(c.gameObject);

            lines = new List<ItemLine>();
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

            line.SetConnectedTiles(tileA, tileB);


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

        public void AssingItemTilesToGridTiles()
        {
            List<BaseTile> baseTilesToHit = tiles.Select(tile => tile.HitBaseTile).ToList();

            BaseGrid.Instance.PutItemToGrid(baseTilesToHit, tiles);

            Destroy(gameObject);
        }

        // private void ReColorLines(ColorTypes lineStat)
        // {
        //     foreach (var line in lines)
        //         line.ReColor(lineStat);
        // }

        public bool AllowSetToGrid()
        {
            var allowSetToGrid = true;

            // int hitCount = 0;


            var lineHits = GetBaseLineHits();
            foreach (var baseLine in lineHits)
            {
                if (baseLine.IsOccupied)
                {
                    allowSetToGrid = false;
                    break;
                }
            }

            // foreach (var tile in tiles)
            // {
            //     var hit = tile.Moveable.Hit();
            //     if (!hit)
            //     {
            //         allowSetToGrid = false;
            //         break;
            //     }
            //
            //     // base tile ın üstünde herhangi birşey var mı kontrolü
            //     var baseTile = hit.transform.GetComponent<BaseTile>();
            //     if (BaseGrid.Instance.GetOccupatableCount(baseTile) <= 0)
            //     {
            //         allowSetToGrid = false;
            //         break;
            //     }
            //
            //     hitCount++;
            // }

            if (allowSetToGrid)
            {
                Recolor(ColorTypes.Hover);

                foreach (var baseLine in lineHits)
                {
                    baseLine.SetOccupied();
                }
            }
            else
                Recolor(ColorTypes.ItemStill);

            return allowSetToGrid;
        }

        private List<BaseLine> GetBaseLineHits()
        {
            var hits = lines.Select(p => p.Hit()).ToList();

            var list = new List<BaseLine>();
            foreach (var hit in hits)
            {
                if (hit && hit.transform != null)
                {
                    list.Add(hit.transform.GetComponent<BaseLine>());
                }
            }

            return list;
        }
    }
}