// maebleme2

using System;
using MyGrid.Code;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StickBlast
{
    public class Moveable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField]
        private LayerMask layerMask;

        private Vector2 offset;
        private Vector3 startPosition;

        private Transform currentMoveable;
        private GridManager gridManager;
        private MyTile myTile;
        
        private void Start()
        {
            currentMoveable = transform.parent;
            startPosition = transform.position;
            gridManager = currentMoveable.GetComponent<GridManager>();
            myTile = GetComponent<MyTile>();
        }

        #region Pointers

        public void OnPointerDown(PointerEventData eventData)
        {
            var target = Camera.main.ScreenToWorldPoint(eventData.position);
            offset = currentMoveable.position - target;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(eventData.position);
            target += offset;

            currentMoveable.position = target;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var allowSetToGrid = AllowSetToGrid();

            if (allowSetToGrid)
            {
                SetPositionAll();
                BaseGrid.Instance.CheckGrid();
            }
            else
            {
                BackToStartPositionAll();
            }
        }

        #endregion

        #region Manager

        private bool AllowSetToGrid()
        {
            var allowSetToGrid = true;

            foreach (var tile in gridManager.Tiles)
            {
                if (!tile.gameObject.activeSelf) continue;

                var myTile = (MyTile)tile;

                var hit = myTile.Moveable.Hit();
                if (!hit)
                {
                    allowSetToGrid = false;
                    break;

                }

                // base tile ın üstünde herhangi birşey var mı kontrolü
                var baseTile = hit.transform.GetComponent<MyTile>();
                if (baseTile.OnMyTile)
                {
                    allowSetToGrid = false;
                    break;
                }
            }

            return allowSetToGrid;
        }

        private void SetPositionAll()
        {
            foreach (var tile in gridManager.Tiles)
            {
                if (!tile.gameObject.activeSelf)
                    continue;

                var myTile = (MyTile)tile;
                myTile.Moveable.SetPositionToHit();
            }
        }

        private void BackToStartPositionAll()
        {
            foreach (var tile in gridManager.Tiles)
            {
                if (!tile.gameObject.activeSelf) continue;
                var myTile = (MyTile)tile;
                myTile.Moveable.BackToStartPosition();
            }
        }

        #endregion


        private void SetPositionToHit()
        {
            var hit = Hit();
            var baseTile = hit.transform.GetComponent<MyTile>();
            baseTile.OnMyTile = myTile;
            var target = hit.transform.position;
            target.z = 0.5f;
            transform.position = target;
            myTile.SetActiveCollider(false);
        }


        private void BackToStartPosition()
        {
            transform.position = startPosition;
        }


        private RaycastHit2D Hit()
        {
            return Physics2D.Raycast(transform.position, Vector3.forward, 10, layerMask);
        }

        // private void FixedUpdate()
        // {
        //     var hit = Hit();
        //
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, hit ? Color.yellow : Color.white);
        // }
    }
}