using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;

/// <summary>
/// 배틀페이즈 진입 시 플레이어의 필드 데이터와 각 플레이어의 이벤트핸들러 중 배틀이벤트 핸들러를 업데이트합니다.
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
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Disconnected");
            return null;
        }
        if (GameManager.instance.server_Authority == Server_authority_Type.Host)
        { return players[1]; }
        else
        {
            return players[0];
        }
    }
    public GamePlayer GetMyPlayer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Disconnected");
            return null;
        }
        if (GameManager.instance.server_Authority == Server_authority_Type.Host)
        { return players[0]; }
        else
        {
            return players[1];
        }
    }

    /// <summary>
    /// 매개변수에 해당하는 플레이어를 받아옵니다.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GamePlayer GetPlayer_CompareTurnType(TurnType type)
    {
        GamePlayer returnValue = null;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].current_TurnType == type)
            {
                returnValue = players[i];
                break;
            }
        }
        return returnValue;
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
