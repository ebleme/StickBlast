// maebleme2

using StickBlast.Grid;
using UnityEngine;

namespace StickBlast
{
    public class ItemTile:TileController
    {
        public Moveable Moveable { get; private set; }

       
        // public MyTile OnMyTile;

        public Item Item { get; private set; }
       
        private Collider2D collider2d;

        private Vector3 startPosition;
       
        private void Awake()
        {
            startPosition = transform.position;
           
            Moveable = GetComponent<Moveable>();
            collider2d = GetComponent<Collider2D>();
        }

        public void SetActiveCollider(bool active)
        {
            collider2d.enabled = active;
        }

        public void SetItem(Item item)
        {
            Item = item;
        }
       
        public void BackToStartPosition()
        {
            transform.position = startPosition;
        }

        public void SetPositionToHit()
        {
            var hit = Moveable.Hit();
           
            var baseTile = hit.transform.GetComponent<BaseTile>();
            baseTile.SetOnTile();
           
            // baseTile.OnMyTile = this;
            var target = hit.transform.position;
            target.z = 0f;
            transform.position = target;
           
            SetActiveCollider(false);
        }
    }
}