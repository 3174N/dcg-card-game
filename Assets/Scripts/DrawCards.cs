using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DrawCards : NetworkBehaviour
{
    #region Variables

    private PlayerManager _manager;

    #endregion

    public void Draw()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        _manager = networkIdentity.GetComponent<PlayerManager>();
        
        _manager.CmdDrawCards();
    }
}
