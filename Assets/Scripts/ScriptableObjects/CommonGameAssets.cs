using System.Collections.Generic;
using System.Linq;
using Ebleme.Extensions;
using Ebleme.Utility;
using StickBlast;
using UnityEngine;

namespace Ebleme
{
    [CreateAssetMenu(fileName = "CommonGameAssets", menuName = "Ebleme/CommonGameAssets", order = 0)]
    public class CommonGameAssets : SingletonScriptableObject<CommonGameAssets>
    {
        [SerializeField]
        private List<Item> items;


        public List<Item> GetAllItems()
        {
            return items;
        }
        
        public Item GetItemByType(ItemTypes type)
        {
            return items.SingleOrDefault(p => p.ItemType == type);
        }
        
        public Item GetRandomItem()
        {
            return items.GetRandomElement();
        }
        
        public List<Item> GetRandomItems()
        {
            return items.GetRandomElements(GameConfigs.Instance.ItemSpawnCount);
        }
    }
}