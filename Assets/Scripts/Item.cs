// maebleme2

using System;
using System.Collections.Generic;
using System.Linq;
using StickBlast.Grid;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StickBlast
{
    public class Item : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField]
        private ItemTypes itemType;
        
        [SerializeField]
        private Vector3 movingScale = Vector3.one;

        [Header("Line")]
        [SerializeField]
        private ItemLine linePrefab;

        [SerializeField]
        private Transform linesContent;
        
        [SerializeField]
        private List<ItemLine> lines;


        private Vector3 startScale;
        private Vector3 startPosition;
        private Vector2 offset;

        private List<ItemTile> tiles = new List<ItemTile>();
        private List<BaseLine> baseLinesHit;

        private bool canPlaced;

        private void Start()
        {
            startScale = transform.localScale;
            startPosition = transform.position;

            SetTilesList();
            DrawLines();
            Recolor(ColorTypes.ItemStill);
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

        #region Color

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

        #endregion

        #region Lines

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
                    DrawLine((ItemTile)tile, (ItemTile)right, LineDirection.Horizontal);
                }

                var up = tile.GetNeighbour(Direction.Up);

                if (up && up.gameObject.activeSelf)
                {
                    DrawLine((ItemTile)tile, (ItemTile)up, LineDirection.Vertical);
                }
            }
        }

        private void DrawLine(ItemTile tileA, ItemTile tileB, LineDirection lineDirection)
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

        #endregion

        private void SetMovingScale()
        {
            transform.localScale = movingScale;
        }

        private void SetStartScale()
        {
            transform.localScale = startScale;
        }
        
        /// <summary>
        /// Set all tiles position to their start position
        /// </summary>
        private void ReleaseAll()
        {
            SetStartScale();

            transform.position = startPosition;

            // foreach (var tile in tiles)
            // {
            //     tile.BackToStartPosition();
            // }
            //
            // foreach (var line in lines)
            // {
            //     line.BackToStartPosition();
            // }
        }

        // private void ReColorLines(ColorTypes lineStat)
        // {
        //     foreach (var line in lines)
        //         line.ReColor(lineStat);
        // }

        public void AllowSetToGrid()
        {
            canPlaced = true;

            var newHitLines = GetBaseLineHits();
            // var newHitTiles = GetBaseTileHits();

            // if (newHitTiles != baseTilesHit || newHitLines != baseLinesHit)
            // {
            //     BaseGrid.Instance.DeHover(baseTilesHit, baseLinesHit);
            // } 

            
            // if (baseLinesHit != null && newHitLines != null && !newHitLines.SequenceEqual(baseLinesHit))
            if (newHitLines != baseLinesHit)
            {
                BaseGrid.Instance.DeHover(baseLinesHit);
            }

            baseLinesHit = newHitLines;
            // baseTilesHit = newHitTiles;

            // if (baseLinesHit.Count == lines.Count && baseTilesHit.Count == tiles.Count)
            if (baseLinesHit.Count == lines.Count)
            {
                foreach (var baseLine in baseLinesHit)
                {
                    if (baseLine.IsOccupied)
                    {
                        canPlaced = false;
                        break;
                    }
                }
            }
            else
            {
                canPlaced = false;
            }

            if (canPlaced)
            {
                BaseGrid.Instance.Hover(baseLinesHit);
            }
            // else
            //     BaseGrid.Instance.DeHover(baseLinesHit);
        }

        private List<BaseLine> GetBaseLineHits()
        {
            var hits = lines.Select(p => p.Hit()).ToList();

            var list = new List<BaseLine>();

            foreach (var line in lines)
            {
                var hit = line.Hit();

                if (hit && hit.transform != null)
                {
                    var baseLine = hit.transform.GetComponent<BaseLine>();
                    if (baseLine.lineDirection == line.lineDirection)
                        list.Add(baseLine);
                }
            }

            return list;
        }

        private List<BaseTile> GetBaseTileHits()
        {
            var hits = tiles.Select(p => p.Hit()).ToList();

            var list = new List<BaseTile>();
            foreach (var hit in hits)
            {
                if (hit && hit.transform != null)
                {
                    list.Add(hit.transform.GetComponent<BaseTile>());
                }
            }

            return list;
        }


        #region Pointers

        public void OnPointerDown(PointerEventData eventData)
        {
            var target = Camera.main.ScreenToWorldPoint(eventData.position);
            offset = transform.position - target;

            SetMovingScale();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(eventData.position);
            target += offset;

            transform.position = target;

            AllowSetToGrid();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (canPlaced)
            {
                BaseGrid.Instance.PutItemToGrid(baseLinesHit);

                Destroy(gameObject);                
                BaseGrid.Instance.CheckGrid();
            }
            else
            {
                ReleaseAll();
            }
            
            baseLinesHit = null;
        }

        #endregion
    }
}