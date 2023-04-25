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
            trigger.about_Prepare.OnDrawOnDeck += (test) => Debug.LogError("�̺�Ʈ �߰�");
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

        // �÷��̾���� trigger ���� ������ �� �ִ� �޼ҵ�
        public void SetAboutTrashEvent()
        {
            trigger.about_Prepare.OnDrawOnDeck += (test) => Debug.LogError("�� �ٸ��÷��̾�");
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
                // �� ĳ������ ��� trigger ���� ����
                var bytes = CustomBinaryFormatter.Serialized<EventTrigger>(trigger);
                stream.SendNext(bytes);
            }
            else
            {
                // �ٸ� �÷��̾��� ��� trigger ���� ����
                byte[] bytes = (byte[])stream.ReceiveNext();
                trigger = CustomBinaryFormatter.DeserializedFromByte<EventTrigger>(bytes);
            }
        }
    }

}