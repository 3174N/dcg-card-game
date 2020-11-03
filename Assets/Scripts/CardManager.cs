using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class CardManager : NetworkBehaviour
{
    #region Variables

    public Card card;

    [Header("Drag & Drop")] private GameObject _dropZone;

    private GameObject _enemyDropZone;
    private GameObject _playerDropZone;

    private Transform _canvas;
    private Transform _startParent;

    private bool _isDragging = false;
    private bool _isOverDropZone = false;
    private bool _isDraggable = true;

    private Vector2 _startPosition;

    [Header("Zoom")] private GameObject _zoomCard;

    private PlayerManager _manager;

    #endregion

    private void Start()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        _manager = networkIdentity.GetComponent<PlayerManager>();
        
        card = _manager.cards[UnityEngine.Random.Range(0, _manager.cards.Count)];
        
        _canvas = GameObject.Find("Main Canvas").transform;
        _enemyDropZone = GameObject.Find("EnemyDropZone");
        _playerDropZone = GameObject.Find("PlayerDropZone");

        _isDraggable = hasAuthority;

        _dropZone = hasAuthority ? _playerDropZone : _enemyDropZone;
        
        GetComponent<Image>().sprite = hasAuthority ? card.frontSprite : card.backSprite;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isDragging)
        {
            transform.position = Input.mousePosition;
            transform.SetParent(_canvas.transform, true);
        }
    }

    public void Flip()
    {
        Sprite currentSprite = GetComponent<Image>().sprite;

        GetComponent<Image>().sprite = (currentSprite == card.frontSprite) ? card.backSprite : card.frontSprite;
    }

    public void StartDrag()
    {
        if (!_isDraggable) return;

        _startPosition = transform.position;
        _startParent = transform.parent;
        _isDragging = true;
    }

    public void EndDrag()
    {
        if (!_isDraggable) return;

        _isDragging = false;

        if (_isOverDropZone)
        {
            _isDraggable = false;

            _manager.PlayCard(gameObject);

            card.action?.Invoke();
        }
        else
        {
            transform.position = _startPosition;
            transform.SetParent(_startParent, false);
        }
    }

    public void OnHoverEnter()
    {   
        _zoomCard = Instantiate(gameObject, _canvas, true);

        _zoomCard.layer = LayerMask.NameToLayer("Zoom");

        RectTransform rect = _zoomCard.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(240, 360);
        rect.anchorMin = new Vector2(1f, 0.5f);
        rect.anchorMax = new Vector2(1f, 0.5f);
        rect.anchoredPosition = new Vector3(-165, 0);
    }

    public void OnHoverExit()
    {
        Destroy(_zoomCard);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == _dropZone)
        {
            _isOverDropZone = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject == _dropZone)
        {
            _isOverDropZone = false;
        }
    }
}