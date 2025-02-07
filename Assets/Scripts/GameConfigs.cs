using Ebleme.Utility;
using UnityEngine;

namespace Ebleme
{
    [CreateAssetMenu(fileName = "GameConfigs", menuName = "Ebleme/GameConfigs", order = 0)]
    public class GameConfigs : SingletonScriptableObject<GameConfigs>
    {
        [SerializeField]
        private Vector2Int baseGridSize;

        
        [SerializeField]
        private Color linePassiveColor;
        
        [SerializeField]
        private Color hoverColor; 
        
        [SerializeField]
        private Color activeColor;

        [Header("Item")]
        [SerializeField]
        private Color itemStillColor;
    
        public Vector2Int BaseGridSize => baseGridSize;
        
        public Color LinePassiveColor => linePassiveColor;
        public Color ActiveColor => activeColor;
        public Color HoverColor => hoverColor;
        public Color ItemStillColor => itemStillColor;
        
    }
}