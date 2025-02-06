// maebleme2

using System.Collections.Generic;
using MyGrid.Code;
using UnityEngine;

namespace StickBlast
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private Vector3 movingScale = Vector3.one;
        
        private List<MyTile> tiles = new List<MyTile>();
        private GridManager gridManager;

        public GridManager GridManager => gridManager;

        private Vector3 startScale;
        
        private void Start()
        {
            gridManager = GetComponent<GridManager>();
            startScale = transform.localScale;
            
            SetTilesList();
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