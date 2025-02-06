using System;
using MyGrid.Code;
using UnityEngine;

namespace StickBlast
{
    public class MyTile : TileController
    {
       public Moveable Moveable { get; private set; }

       public MyTile OnMyTile;
       
       private Collider2D collider;
       
       private void Start()
       {
           Moveable = GetComponent<Moveable>();
           collider = GetComponent<Collider2D>();
       }

       public void SetActiveCollider(bool active)
       {
           collider.enabled = active;
       }
    }
}