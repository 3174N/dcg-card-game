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
            //card.GetComponent<Card>().player = Card.Player.Player;
        }
        
        //for (int i = 0; i < 5; i++)
        //{
        //    GameObject enemyCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0f, 0f, 0f), Quaternion.identity);
        //    enemyCard.transform.SetParent(enemyArea.transform, false);
        //    enemyCard.GetComponent<Card>().player = Card.Player.Enemy;
        //}
    }
}
