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



    /// <summary>
    /// 배틀커넥터를 초기화합니다. 배틀커넥터는 로컬에서 플레이어 배열을 가지고 있으며 서버권한에 따른 플레이어 배열의 인덱스를 정리합니다. 
    /// </summary>
    /// <param name="player"></param>
    public void Init(GamePlayer player)
    {
        int adress = isMasterClientLocal ? 0 : 1;
        if (players == null || players.Length != 2)
        {
            players = new GamePlayer[2];
        }
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
    /// 매개변수에 해당하는 턴타입의 플레이어를 받아옵니다.
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

    [PunRPC]
    public void SetSyncPlayerData(byte[] bytes)
    {
        GamePlayer player = CustomBinaryFormatter.DeserializedFromByte<GamePlayer>(bytes);
        if(PhotonNetwork.IsMasterClient)
        {
            players[0] = player;
        }
        else
        {
            players[1] = player;
        }
    }


    /// <summary>
    /// 최신버전의 동기화를 위해 기존플레이어 데이터는 없앱니다.
    /// </summary>
    public void ConnectedAndTrySync()
    {
        players = null;
        if (!PhotonNetwork.IsMasterClient)
        {
            var trg = CustomBinaryFormatter.Serialized<GamePlayer>(GameManager.instance.players[0]);
            photonView.RPC("SetSyncPlayerData", RpcTarget.Others, trg);

        }
        else
        {
            var trg = CustomBinaryFormatter.Serialized<GamePlayer>(GameManager.instance.players[0]);
            photonView.RPC("SetSyncPlayerData", RpcTarget.Others, trg);
        }
    }

    /// <summary>
    /// 일정주기 , 기실시간으로 데이터를 원격서버로 받거나 보냅니다.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new NotImplementedException();
    }
}
