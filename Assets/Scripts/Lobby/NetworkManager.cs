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

        //마스터서버 조인
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        //접속 중 혹은 실패

        // -> 완전 접속된 후의 처리


    }

    /// <summary>
    /// 마스터서버에 연결되면 실행되는 콜백
    /// </summary>
    public override void OnConnectedToMaster()
    {

        base.OnConnectedToMaster();
        //마스터 서버에 정상적으로 접속되면 자동콜백
        debugger.ShowText("Connected!");
        lobby.SetInit(true);
    }

    /// <summary>
    /// 연결이 끊기면 실행되는 콜백
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        //연결 끊김 (매개변수 : 끊긴 사유)
        lobby.SetInit(false);
        debugger.ShowText($"Server Disconnect : {cause}. Try Connecting");
        //재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// 방만들기 커스텀메서드
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
    /// 룸 설정 : private?public
    /// value : true : private // false : public
    /// </summary>
    /// <param name="value"></param>
    public void OnToggleChange(bool value)
    {
        PhotonNetwork.CurrentRoom.IsVisible = !value;
    }

    /// <summary>
    /// 방을 생성하게 되면 콜백되는 메서드
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


    //Join시 매치매이킹서버로부터 접속 시도,
    public void Connect()
    {
        //중복접속 안되도록 처리


    }
    /// <summary>
    /// 조인랜덤룸 -> 비어있는 방이 없음
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }

    /// <summary>
    /// 작성한 코드네임으로의 접속을 시도하는 커스텀메서드
    /// </summary>
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

    /// <summary>
    /// 방 진입에 실패하면 콜백되는 메서드
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
    /// 방에 진입 시 (방 생성시 포함) 콜백되는 메서드
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
        //테스트 코드입니다.
        //PhotonNetwork.LoadLevel("Ingame");

#endif
    }

    /// <summary>
    /// 방장입장에서의 다른 플레이어가 입장하면 콜백되는 메서드
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
