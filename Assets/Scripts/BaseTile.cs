// maebleme2

using StickBlast.Grid;
using UnityEngine;

namespace StickBlast
{
    public class BaseTile:TileController
    {
        public int Count { get; set; }

        public ItemTile OnMyTile { get; set; }

        private Collider2D collider2d;

        private void Awake()
        {
            collider2d = GetComponent<Collider2D>();
        }

        public void SetOnTile()
        {
            Count++;
            
        }
    }
}