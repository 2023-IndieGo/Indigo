namespace TestCode.Moru
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Photon.Pun;

    public class EventTest : MonoBehaviourPunCallbacks, IPunObservable
    {
        public EventTrigger trigger;

        private void Start()
        {
            trigger = new EventTrigger();
            trigger.Init();
        }

        public void Adding()
        {
            trigger.about_Prepare.OnDrawOnDeck += (test) => Debug.LogError("이벤트 추가");
        }

        public void Debugging()
        {
            if (photonView.IsMine)
            {
                var trg = CustomBinaryFormatter.Serialized<EventTrigger>(trigger);
                photonView.RPC("SetEvent", RpcTarget.All, trg);
            }
            trigger.about_Prepare.OnDrawOnDeck?.Invoke(null);
        }

        [PunRPC]
        void SetEvent(byte[] bytes)
        {
            EventTrigger trg = CustomBinaryFormatter.DeserializedFromByte<EventTrigger>(bytes);
            this.trigger = trg;
        }

        // 플레이어들이 trigger 값을 변경할 수 있는 메소드
        public void SetAboutTrashEvent()
        {
            trigger.about_Prepare.OnDrawOnDeck += (test) => Debug.LogError("난 다른플레이어");
            if (!PhotonNetwork.IsMasterClient)
            {
                var trg = CustomBinaryFormatter.Serialized<EventTrigger>(trigger);
                photonView.RPC("SetEvent", RpcTarget.Others, trg);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // 내 캐릭터인 경우 trigger 값을 전송
                var bytes = CustomBinaryFormatter.Serialized<EventTrigger>(trigger);
                stream.SendNext(bytes);
            }
            else
            {
                // 다른 플레이어의 경우 trigger 값을 수신
                byte[] bytes = (byte[])stream.ReceiveNext();
                trigger = CustomBinaryFormatter.DeserializedFromByte<EventTrigger>(bytes);
            }
        }
    }

}