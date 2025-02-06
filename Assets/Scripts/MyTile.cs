using System;
using MyGrid.Code;

namespace StickBlast
{
    public class MyTile : TileController
    {
       public Moveable Moveable { get; private set; }

       public MyTile OnMyTile;
       
       private void Start()
       {
           Moveable = GetComponent<Moveable>();
       }
    }
}