using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;

/// <summary>
/// 배틀페이즈 진입 시 플레이어의 필드 데이터를 업데이트합니다.
/// </summary>
public class BattleConnecter : MonoBehaviourPun, IPunObservable
{
    [SerializeField] BattlePhaseController battleController;
    public bool isMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;

    /// <summary>
    /// 0 : Host, 1 : Client
    /// </summary>
    [ShowInInspector]
    public GamePlayer[] players = null;

    public void Init(GamePlayer player)
    {
        if (players == null || players.Length != 2)
        { 
            players = new GamePlayer[2]; 
        }
        int adress = isMasterClientLocal ? 0 : 1;
        players[adress] = player;
    }

    public GamePlayer GetOtherPlayer()
    {
        if(!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Disconnected");
            return null;
        }
        if(GameManager.instance.server_Authority == Server_authority_Type.Host)
        { return players[1]; }
        else
        {
            return players[0];
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(players[0]);
            stream.SendNext(players[1]);
        }
        else
        {
            players[0] = (GamePlayer)stream.ReceiveNext();
            players[1] = (GamePlayer)stream.ReceiveNext();
        }

    }


}
