using Ebleme.Utility;
using UnityEngine;

namespace Ebleme.ColowSwapMaddness
{
    [CreateAssetMenu(fileName = "GameConfigs", menuName = "Ebleme/GameConfigs", order = 0)]
    public class GameConfigs : SingletonScriptableObject<GameConfigs>
    {
        [SerializeField]
        private Vector2Int baseGridSize;

        
        [SerializeField]
        private Color linePassiveColor;
        
        [SerializeField]
        private Color lineHoverColor; 
        
        [SerializeField]
        private Color lineActiveColor;


        public Vector2Int BaseGridSize => baseGridSize;
        
        public Color LinePassiveColor => linePassiveColor;
        public Color LineActiveColor => lineActiveColor;
        public Color LineHoverColor => lineHoverColor;
        
    }
}