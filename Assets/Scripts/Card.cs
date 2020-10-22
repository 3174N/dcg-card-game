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

    [Header("Drag & Drop")] 
    public GameObject enemyDropZone;
    public GameObject playerDropZone;
    
    private GameObject dropZone;
    
    private Transform canvas;
    private Transform startParent;

    private bool isDragging = false;
    private bool isOverDropZone = false;

    private Vector2 startPosition;

    [Header("Zoom")]
    private GameObject zoomCard;

    #endregion

    private void Awake()
    {
        canvas = GameObject.Find("Main Canvas").transform;
    }

    private void Start()
    {
        switch (player)
        {
            case Player.Player:
                dropZone = playerDropZone;
                break;
            case Player.Enemy:
                dropZone = enemyDropZone;
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
            transform.SetParent(canvas.transform, true);
        }
    }

    public void StartDrag()
    {
        startPosition = transform.position;
        startParent = transform.parent;
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
            transform.SetParent(startParent, false);
        }
    }

    public void OnHoverEnter()
    {
        zoomCard = Instantiate(gameObject, canvas, true);

        zoomCard.layer = LayerMask.NameToLayer("Zoom");

        RectTransform rect = zoomCard.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(240, 360);
        rect.anchorMin = new Vector2(1f, 0.5f);
        rect.anchorMax = new Vector2(1f, 0.5f);
        rect.anchoredPosition = new Vector3(-165, 0);
    }

    public void OnHoverExit()
    {
        Destroy(zoomCard);
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
