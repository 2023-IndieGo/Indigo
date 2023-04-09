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


    [TitleGroup("CreateComponent")]
    [SerializeField] private Text codeText;
    [SerializeField] private Toggle isPrivate;

    [TitleGroup("JoinComponent")]
    [SerializeField] private InputField inputCode;
    [SerializeField] private Button JoinBtn;



    private void Start()
    {
        debugger = DebugerText.gameObject.AddComponent<Debugger>();
        debugger.Init(DebugerText);
        debugger.ShowText("Game Initialized...Try Connecting");
        lobby.SetInit(false);

        //부가적인 마스터서버 조인
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        //접속 중 혹은 실패

        // -> 완전 접속된 후의 처리


    }


    public override void OnConnectedToMaster()
    {

        base.OnConnectedToMaster();
        //마스터 서버에 정상적으로 접속되면 자동콜백
        debugger.ShowText("Connected!");
        lobby.SetInit(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        //연결 끊김 (매개변수 : 끊긴 사유)
        lobby.SetInit(false);
        debugger.ShowText($"Server Disconnect : {cause}. Try Connecting");
        //재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

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

    public void OnToggleChange(bool value)
    {
        PhotonNetwork.CurrentRoom.IsVisible = !value;
    }

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


    //Join시 매치매이킹서버로부터 접속 시도,
    public void Connect()
    {
        //중복접속 안되도록 처리


    }

    //조인랜덤룸 -> 비어있는 방이 없음
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }

    public void TryJoinGame()
    {
        string GameName = inputCode.text;
        //잘 접속되어있는 상태라면
        if (PhotonNetwork.IsConnected)
        {
            //접속중
            PhotonNetwork.JoinRoom($"{GameName}");
        }
        //아니라면
        else
        {
            //재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
        JoinBtn.interactable = false;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        debugger.ShowText($"Fail to Join Game : {inputCode.text}. cause : {message}");
        JoinBtn.interactable = true;
    }

    //방 접속 성공, 혹은 방을 만들고 접속완료
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        debugger.ShowText($"Success Join Room!");
        //씬 로딩
        //debugger.ShowText("");
#if UNITY_EDITOR
        //테스트 코드입니다.
        //PhotonNetwork.LoadLevel("Ingame");

#endif
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("Ingame");
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
