// maebleme2

using Ebleme;
using StickBlast.Grid;
using UnityEngine;

namespace StickBlast
{
    public class BaseLine : MonoBehaviour
    {
        public TileController[] ConnectedTiles { get; private set; }
        private SpriteRenderer spriteRenderer;

        public Vector2Int coordinate;
        public LineDirection lineDirection;

        public bool IsOccupied;

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

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = GameConfigs.Instance.LinePassiveColor;
        }

        private void Start()
        {
            
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

        public void SetOccupied()
        {
            IsOccupied = true;

            ReColor(ColorTypes.Active);
        }

        public void DeOccupied()
        {
            IsOccupied = false;

            ReColor(ColorTypes.Passive);
        }

        public void DeHover()
        {
            ReColor(IsOccupied ? ColorTypes.Active: ColorTypes.Passive);
        }

        public void Hover()
        {
            ReColor(IsOccupied ? ColorTypes.Active: ColorTypes.Hover);
        }
    }
}