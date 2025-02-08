// maebleme2

using System;
using System.Linq;
using Ebleme;
using StickBlast.Grid;
using UnityEngine;

namespace StickBlast
{
    public class ItemLine:MonoBehaviour
    {
        public TileController[] ConnectedTiles { get; private set; }
        public LineDirection lineDirection;
        public Vector2Int coordinate;

        private SpriteRenderer spriteRenderer;
        private Vector3 startPosition;
        private bool IsOccupied;

        // public bool Compare(params TileController[] connectedTiles)
        // {
        //     if (connectedTiles.Length != ConnectedTiles.Length) return false;
        //     
        //     for (int i = 0; i < ConnectedTiles.Length; i++)
        //     {
        //         if (ConnectedTiles[i].coordinate != connectedTiles[i].coordinate)
        //         {
        //             return false;
        //         }
        //     }
        //
        //     return true;
        // }
        
        public RaycastHit2D Hit()
        {
            return Physics2D.Raycast(transform.position, Vector3.forward, 30, GameConfigs.Instance.BaseLineLayer);
        }
        
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = GameConfigs.Instance.LinePassiveColor;
        }

        private void Start()
        {
            startPosition = transform.position;
        }
        
        public void BackToStartPosition()
        {
            transform.position = startPosition;
        }

        public void Set(Vector2Int coordinate, LineDirection direction, params TileController[] tiles)
        {
            ConnectedTiles = tiles;
            this.coordinate = coordinate;

            lineDirection = direction;
        }    
        
        

        public void ReColor(ColorTypes status)
        {
            switch (status)
            {
                case ColorTypes.ItemStill:
                    spriteRenderer.color = GameConfigs.Instance.ItemStillColor;
                    break;
                case ColorTypes.Passive:
                    spriteRenderer.color = GameConfigs.Instance.LinePassiveColor;
                    break;
                case ColorTypes.Hover:
                    spriteRenderer.color = GameConfigs.Instance.HoverColor;
                    break;
                case ColorTypes.Active:
                    spriteRenderer.color = GameConfigs.Instance.ActiveColor;
                    break;
                default:
                    break;
            }
        }
        
        private void FixedUpdate()
        {
            var hit = Hit();
        
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 20, hit ? Color.yellow : Color.white);
        }
    }
}