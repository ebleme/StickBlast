// maebleme2

using System;
using System.Collections.Generic;
using DG.Tweening;
using Ebleme;
using Ebleme.Utility;
using StickBlast;
using UnityEngine;

namespace DefaultNamespace
{
    public class ItemSpawner : Singleton<ItemSpawner>
    {
        [SerializeField]
        private Transform spawnPoint;

        [SerializeField]
        private Transform[] itemPoints;

        private List<Item> items;

        private Sequence sequence;

        private void Start()
        {
            sequence = DOTween.Sequence();
            SpawnItems();
        }

        private void SpawnItems()
        {
            sequence.Kill();

            var itemPrefabs = CommonGameAssets.Instance.GetRandomItems();

            items = new List<Item>();
            foreach (var itemPrefab in itemPrefabs)
            {
                var item = Instantiate(itemPrefab);
                item.transform.position = spawnPoint.position;
                item.OnItemDestroyed += OnItemDestroyed;
                items.Add(item);
            }

            sequence = DOTween.Sequence();

            int index = 0;
            foreach (var item in items)
            {
                sequence.Append(item.transform.DOMove(itemPoints[index].position, GameConfigs.Instance.ItemMoveAnimDuration));
                sequence.AppendInterval(GameConfigs.Instance.ItemMoveAnimInterval);

                index++;
            }

            sequence.OnComplete(() =>
            {
                foreach (var item in items)
                    item.SetCanTouch();
            });
        }

        private void OnItemDestroyed(Item item)
        {
            if (items.Contains(item))
            {
                items.Remove(item);

                if (items.Count == 0)
                {
                    SpawnItems();
                }
            }
            else
            {
                Debug.LogError("Item should be in the list");
            }
        }
    }
}