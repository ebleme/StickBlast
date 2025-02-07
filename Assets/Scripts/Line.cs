// maebleme2

using System;
using Ebleme;
using StickBlast.Grid;
using UnityEngine;

namespace StickBlast
{
    public class Line:MonoBehaviour
    {
        public TileController[] Tiles { get; private set; }
        
        private SpriteRenderer spriteRenderer;

        private Vector3 startPosition;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void Start()
        {
            spriteRenderer.color = GameConfigs.Instance.LinePassiveColor;
            startPosition = transform.position;
        }
        
        public void BackToStartPosition()
        {
            transform.position = startPosition;
        }

        public void SetTiles(params TileController[] tiles)
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