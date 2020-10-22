using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    #region Variables

    public List<GameObject> cards = new List<GameObject>();

    private GameObject playerArea;
    private GameObject playerDropZone;
    private GameObject enemyArea;
    private GameObject enemyDropZone;

    #endregion

    public override void OnStartClient()
    {
        base.OnStartClient();

        playerArea = GameObject.Find("PlayerArea");
        playerDropZone = GameObject.Find("PlayerDropZone");
        enemyArea = GameObject.Find("EnemyArea");
        enemyDropZone = GameObject.Find("EnemyDropZone");
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
            GameObject card = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0f, 0f, 0f), Quaternion.identity);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
        }
    }

    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
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
            card.transform.SetParent(hasAuthority ? playerArea.transform : enemyArea.transform);
        }
        else if (type == "Played")
        {
            card.transform.SetParent(hasAuthority ? playerDropZone.transform : enemyDropZone.transform);
        }
    }
}
