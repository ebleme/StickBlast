using System;
using StickBlast.Grid;
using UnityEngine;

namespace StickBlast
{
    public class MyTile : TileController
    {
       public Moveable Moveable { get; private set; }
       public MyTile OnMyTile;

       public Item Item { get; private set; }
       
       private Collider2D collider;

       private Vector3 startPosition;
       
       private void Awake()
       {
           startPosition = transform.position;
           
           Moveable = GetComponent<Moveable>();
           collider = GetComponent<Collider2D>();
       }

       public void SetActiveCollider(bool active)
       {
           collider.enabled = active;
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
           var baseTile = hit.transform.GetComponent<MyTile>();
           baseTile.OnMyTile = this;
           var target = hit.transform.position;
           target.z = 0f;
           transform.position = target;
           
           SetActiveCollider(false);
       }
    }
}