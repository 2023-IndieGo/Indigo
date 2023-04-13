using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1";
    public Text DebugerText;
    public Debugger debugger;
    public LobbyManager lobby;


    public string SceneName;

    [TitleGroup("CreateComponent")]
    [SerializeField] private Text codeText;
    [SerializeField] private Toggle isPrivate;

    [TitleGroup("JoinComponent")]
    [SerializeField] private InputField inputCode;
    [SerializeField] private Button JoinBtn;



    private void Start()
    {
        if(PhotonNetwork.IsConnected &&PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
        }
        debugger = DebugerText.gameObject.AddComponent<Debugger>();
        debugger.Init(DebugerText);
        debugger.ShowText("Game Initialized...Try Connecting");
        lobby.SetInit(false);

        //�����ͼ��� ����
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        //���� �� Ȥ�� ����

        // -> ���� ���ӵ� ���� ó��


    }

    /// <summary>
    /// �����ͼ����� ����Ǹ� ����Ǵ� �ݹ�
    /// </summary>
    public override void OnConnectedToMaster()
    {

        base.OnConnectedToMaster();
        //������ ������ ���������� ���ӵǸ� �ڵ��ݹ�
        debugger.ShowText("Connected!");
        lobby.SetInit(true);
    }

    /// <summary>
    /// ������ ����� ����Ǵ� �ݹ�
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        //���� ���� (�Ű����� : ���� ����)
        lobby.SetInit(false);
        debugger.ShowText($"Server Disconnect : {cause}. Try Connecting");
        //������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// �游��� Ŀ���Ҹ޼���
    /// </summary>
    public void CreateRoom()
    {
        int randomNum = Random.Range(1000000, 10000000);
        var RoomOption = new RoomOptions();
        RoomOption.MaxPlayers = 2;
        RoomOption.IsVisible = !isPrivate.isOn;
        debugger.ShowText($"Try Createing Room...");
        codeText.text = "Creating...";
        PhotonNetwork.CreateRoom($"{randomNum}", RoomOption);

    }

    /// <summary>
    /// �� ���� : private?public
    /// value : true : private // false : public
    /// </summary>
    /// <param name="value"></param>
    public void OnToggleChange(bool value)
    {
        PhotonNetwork.CurrentRoom.IsVisible = !value;
    }

    /// <summary>
    /// ���� �����ϰ� �Ǹ� �ݹ�Ǵ� �޼���
    /// </summary>
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        var roomName = PhotonNetwork.CurrentRoom.Name;
        debugger.ShowText($"Success Create Room. RoomNumber : {roomName}");
        codeText.text = $"{roomName}";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        debugger.ShowText($"Fail to Create Room : {message} Try Connecting");
        codeText.text = "Try Creating...";
    }

    public void OnLeftServer()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
        }

    }


    //Join�� ��ġ����ŷ�����κ��� ���� �õ�,
    public void Connect()
    {
        //�ߺ����� �ȵǵ��� ó��


    }
    /// <summary>
    /// ���η����� -> ����ִ� ���� ����
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }

    /// <summary>
    /// �ۼ��� �ڵ���������� ������ �õ��ϴ� Ŀ���Ҹ޼���
    /// </summary>
    public void TryJoinGame()
    {
        string GameName = inputCode.text;
        //�� ���ӵǾ��ִ� ���¶��
        if (PhotonNetwork.IsConnected)
        {
            //������
            PhotonNetwork.JoinRoom($"{GameName}");
        }
        //�ƴ϶��
        else
        {
            //������ �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
        JoinBtn.interactable = false;
    }

    /// <summary>
    /// �� ���Կ� �����ϸ� �ݹ�Ǵ� �޼���
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        debugger.ShowText($"Fail to Join Game : {inputCode.text}. cause : {message}");
        JoinBtn.interactable = true;
    }

    /// <summary>
    /// �濡 ���� �� (�� ������ ����) �ݹ�Ǵ� �޼���
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        debugger.ShowText($"Success Join Room!");
        if(!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(SceneName);
        }
#if UNITY_EDITOR
        //�׽�Ʈ �ڵ��Դϴ�.
        //PhotonNetwork.LoadLevel("Ingame");

#endif
    }

    /// <summary>
    /// �������忡���� �ٸ� �÷��̾ �����ϸ� �ݹ�Ǵ� �޼���
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel(SceneName);
            PhotonNetwork.CurrentRoom.RemovedFromList = true;
        }
    }

    public class Debugger : MonoBehaviour
    {
        Text text;
        float maxTime = 3;
        float t;
        float threshold = 0.5f;
        float alpha = 1;
        public void Init(Text _text)
        {
            text = _text;
            text.color = new Color(1, 0, 0, 0);
        }
        private void Update()
        {
            if (t < maxTime)
            {
                t += Time.deltaTime;
            }
            else if (t >= maxTime && text.color.a > 0)
            {
                text.color = new Color(1, 0, 0, alpha);
                alpha -= Time.deltaTime * threshold;
            }
        }

        public void ShowText(string text)
        {
            alpha = 1;
            this.text.text = text;
            this.text.color = new Color(1, 0, 0, 1);
        }
    }
}
