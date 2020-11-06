using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerManager : NetworkBehaviour
{
    #region Variables

    public int HP = 20;

    public GameObject cardObject;
    public List<Card> cards = new List<Card>();

    public bool isTurn;

    private GameObject _playerArea;
    private GameObject _playerDropZone;
    private GameObject _enemyArea;
    private GameObject _enemyDropZone;

    [SyncVar] private int _cardsPlayed = 0;

    private List<PlayerManager> _players;

    #endregion

    public override void OnStartClient()
    {
        base.OnStartClient();

        _playerArea = GameObject.Find("PlayerArea");
        _playerDropZone = GameObject.Find("PlayerDropZone");
        _enemyArea = GameObject.Find("EnemyArea");
        _enemyDropZone = GameObject.Find("EnemyDropZone");
        
        _players = FindObjectsOfType<PlayerManager>().ToList();
        foreach (PlayerManager player in _players)
        {
            player.isTurn = (player == this);
        }
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    [Command]
    public void CmdDrawCards()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject card = Instantiate(cardObject, new Vector3(0f, 0f, 0f),
                Quaternion.identity);
            card.GetComponent<CardManager>().card = cards[Random.Range(0, cards.Count)];
            Debug.Log(card.GetComponent<CardManager>().card);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
        }
    }

    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
        _cardsPlayed++;
        Debug.Log(_cardsPlayed);
    }

    [Command]
    private void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "Played");
    }

    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        if (type == "Dealt")
        {
            card.transform.SetParent(hasAuthority ? _playerArea.transform : _enemyArea.transform);
            card.transform.localScale = Vector3.one;
            if (card.GetComponent<CardManager>().card == null) card.GetComponent<CardManager>().card = cards[Random.Range(0, cards.Count)];
        }
        else if (type == "Played")
        {
            card.transform.SetParent(hasAuthority ? _playerDropZone.transform : _enemyDropZone.transform);
            if (hasAuthority && isTurn)
            {
                card.GetComponent<CardManager>().card.action?.Invoke();
                foreach (PlayerManager player in _players)
                {
                    player.isTurn = (player != this);
                    Debug.Log(player + " : " + isTurn);
                }
                foreach (CardManager cardManager in FindObjectsOfType<CardManager>())
                {
                    cardManager.UpdateDrag();
                }
            }
        }

        if (!hasAuthority)
            card.GetComponent<CardManager>().Flip();
    }
}