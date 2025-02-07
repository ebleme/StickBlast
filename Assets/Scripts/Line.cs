// maebleme2

using System;
using System.Linq;
using Ebleme;
using StickBlast.Grid;
using UnityEngine;

namespace StickBlast
{
    public class Line:MonoBehaviour
    {
        public TileController[] ConnectedTiles { get; private set; }
        
        
        private SpriteRenderer spriteRenderer;

        private Vector3 startPosition;


        public bool Compare(params TileController[] connectedTiles)
        {
            if (connectedTiles.Length != ConnectedTiles.Length) return false;
            
            for (int i = 0; i < ConnectedTiles.Length; i++)
            {
                if (ConnectedTiles[i].coordinate != connectedTiles[i].coordinate)
                {
                    return false;
                }
            }

            return true;
        }
        
        public Vector2Int[] GetCoordinates()
        {
            return ConnectedTiles.Select(p => p.coordinate).ToArray();
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

        public void SetConnectedTiles(params TileController[] tiles)
        {
            ConnectedTiles = tiles;
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
    }
}