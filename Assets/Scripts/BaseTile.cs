// maebleme2

using System.Collections.Generic;
using Ebleme;
using StickBlast.Grid;
using UnityEngine;

namespace StickBlast
{
    public class BaseTile : TileController
    {
        // public int Count { get; set; }

        private Collider2D collider2d;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ReColor(ColorTypes type)
        {
            switch (type)
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