using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 배틀페이즈 진입 시 플레이어의 필드 데이터를 업데이트합니다.
/// </summary>
public class BattleConnecter : MonoBehaviourPun
{
    [SerializeField] BattlePhaseController battleController;
    public bool isMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new NotImplementedException();
    }

    
}
