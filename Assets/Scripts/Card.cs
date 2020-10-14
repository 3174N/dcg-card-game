using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    #region Variables
    
    public enum Player
    {
        Player,
        Enemy
    }
    public Player player;
    
    private GameObject dropZone;

    private bool isDragging = false;
    private bool isOverDropZone = false;

    private Vector2 startPosition;

    #endregion

    private void Start()
    {
        switch (player)
        {
            case Player.Player:
                dropZone = GameManager.instance.playerDropZone;
                break;
            case Player.Enemy:
                dropZone = GameManager.instance.enemyDropZone;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDragging)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void StartDrag()
    {
        startPosition = transform.position;
        isDragging = true;
    }

    public void EndDrag()
    {
        isDragging = false;

        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
        }
        else
        {
            transform.position = startPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == dropZone)
        {
            isOverDropZone = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject == dropZone)
        {
            isOverDropZone = false;
        }
    }
}
