// maebleme2

using System;
using Ebleme.ColowSwapMaddness;
using UnityEngine;

namespace StickBlast
{
    public class Line:MonoBehaviour
    {
        public MyTile[] Tiles { get; private set; }
        
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void Start()
        {
            spriteRenderer.color = GameConfigs.Instance.LinePassiveColor;
        }

        public void SetTiles(params MyTile[] tiles)
        {
            Tiles = tiles;
        }

        public void Hover()
        {
            spriteRenderer.color = GameConfigs.Instance.LineHoverColor;

        }
        
        public void Active()
        {
            spriteRenderer.color = GameConfigs.Instance.LineActiveColor;
        }
        
        public void Passive()
        {
            spriteRenderer.color = GameConfigs.Instance.LinePassiveColor;
        }
    }
}