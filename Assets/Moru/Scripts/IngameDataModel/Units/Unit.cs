using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Moru;
using Sirenix.OdinInspector;

//네임스페이스 까야될수도
namespace Moru
{
    [System.Serializable]
    public class Unit
    {
        #region Field
        #region Info Field
        protected string unitName = new string("");
        /// <summary>
        /// 유닛의 이름입니다.
        /// </summary>
        public string UnitName => unitName;


        protected int level = 0;
        /// <summary>
        /// 유닛의 레벨/등급입니다.
        /// </summary>
        public int Level => level;
        #endregion


        #region Value Field
        //....Color....//
        [ShowInInspector, LabelText("RGB 포함 여부 배열"), TitleGroup("중요 데이터값")]
        protected bool[] rgbValue = new bool[3] { false, false, false };

        /// <summary>
        /// 각 RGB값의 밸류배열입니다. 0 : R // G : 1 // B : 2
        /// </summary>
        public bool[] RGBValue => rgbValue;

        /// <summary>
        /// RGB를 eRGB형식으로 받아옵니다.
        /// </summary>
        [ShowInInspector, LabelText("유닛컬러")]
        public eRGB unitColor
        { get { return MColorUtility.Generate_BoolArr_to_eRGB(RGBValue); } }

        //....Direction....//
        [SerializeField, LabelText("인풋방향")]
        protected bool[] input_Dir = new bool[6];
        /// <summary>
        /// 유닛의 인풋라인을 결정해주는 boolean입니다. 0번(1시방향)부터 시계방향으로의 면이 인풋방향입니다.
        /// </summary>
        public bool[] Input_Dir => input_Dir;

        protected int max_MoveCount = 1;
        /// <summary>
        /// 유닛이 최대로 이동할 수 있는 타일 수입니다.
        /// </summary>
        public int Max_MoveCount => max_MoveCount;


        //....Move....//

        protected int cur_MoveCount = 0;
        /// <summary>
        /// 현재 이동가능한 타일 수입니다.
        /// </summary>
        public int Cur_MoveCount => cur_MoveCount;
        #endregion


        #region Reference Value
        /// <summary>
        /// 유닛이 현재 위치한 타일
        /// </summary>
        /// 
        [SerializeField, TitleGroup("참조값"), LabelText("현재 자신의 타일")]
        protected Tile cur_Tile;
        public Tile Cur_Tile => cur_Tile;

        [SerializeField, LabelText("뷰어")] 
        protected Viewer<Unit> myUnitViewer;

        #endregion
        #endregion

        #region Events

        /// <summary>
        /// 컬러값의 변경 시 호출시키는 메서드
        /// </summary>
        private OnValueChange_Params<bool[]> onColorChange;
        /// <summary>
        /// 컬러값의 변경 시 호출시키는 메서드
        /// </summary>
        public OnValueChange OnColorChange;

        #endregion


        #region Constructor
        protected Unit()
        {

        }
        /// <summary>
        /// 랜덤한 유닛데이터를 생성하고자 할 경우의 생성자입니다.
        /// </summary>
        public Unit(Tile targetTile) : base()
        {
            System.Random random = new System.Random();
            //무작위 유닛데이터를 DataBase로부터 받아옴
            //무작위 컬러? (컬러가 유닛마다 정해져있을수도) (일단 무작위 컬러로)
            int randomColor = random.Next(0, 3);
            eRGB randomResult = (eRGB)randomColor;
            rgbValue = MColorUtility.Generate_eRGB_to_BoolArr(randomResult);

            //무작위 방향 (일단 한 방향만)
            int RandomDir = random.Next(0, 6);
            for (int i = 0; i < input_Dir.Length; i++)
            {
                input_Dir[i] = (RandomDir == i) ? true : false;
            }

            cur_Tile = targetTile;
            //TsetMono.onTurnEnd += OnTurnEnd;
            ////Test성격 강함
            //GameObject obj = MonoBehaviour.Instantiate(TsetMono.instance.testUnitPrefap);
            //if (obj.TryGetComponent<UnitViewer>(out var comp))
            //{
            //    myUnitViewer = comp;
            //    comp.Init(this);
            //}
            //else
            //{
            //    myUnitViewer = obj.AddComponent<UnitViewer>();
            //    myUnitViewer.Init(this);
            //}

            Init();
        }

        /// <summary>
        /// 구체적인 유닛의 컬러와 방향을 
        /// </summary>
        /// <param name="_unitColor"></param>
        /// <param name="_input_Dir"></param>
        public Unit(eRGB _unitColor, bool[] _input_Dir, Tile targetTile)
        {
            this.rgbValue = MColorUtility.Generate_eRGB_to_BoolArr(_unitColor);
            this.input_Dir = _input_Dir;
            Init();
        }
        #endregion



        #region Public Methods
        /// <summary>
        /// 유닛 생성 시 기본밸류값 초기화
        /// </summary>
        public virtual void Init()
        {
            myUnitViewer.Init(this);
            cur_Tile.PushUnit(this);

        }

        /// <summary>
        /// 외부이벤트들에 대한 구독취소
        /// </summary>
        public void OnDestroy()
        {

        }



        /// <summary>
        /// 해당 타일로 이동시킵니다.
        /// </summary>
        public void OnMove(Tile target)
        {
            cur_Tile = target;
            this.Init();
            myUnitViewer.Init(this);
        }

        /// <summary>
        /// 유닛을 해당 타일로 새로 배치합니다.
        /// </summary>
        /// <param name="target"></param>
        public void OnLocate(Tile target)
        {

        }

        /// <summary>
        /// 해당 유닛의 컬러를 매개변수값으로 바꿉니다.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(eRGB color)
        {
            bool[] origin = rgbValue;
            bool[] recent = MColorUtility.Generate_eRGB_to_BoolArr(color);
            rgbValue = recent;

            //메서드 실행
            if (onColorChange != null)
            {
                onColorChange.Invoke(origin, recent);
                OnColorChange.Invoke();
            }
        }

        /// <summary>
        /// 해당 유닛에 매개변수 컬러를 더합니다.
        /// </summary>
        /// <param name="color"></param>
        public void AddColor(eRGB color)
        {
            //현재 컬러
            bool[] currentColor = rgbValue;
            //더하는 컬러값
            bool[] colorRGBValue = MColorUtility.Generate_eRGB_to_BoolArr(color);
            //최종 컬러
            bool[] inputColor = new bool[3];

            for (int i = 0; i < currentColor.Length; i++)
            {
                inputColor[i] = (currentColor[i] || colorRGBValue[i]) ? true : false;
            }

            eRGB result = MColorUtility.Generate_BoolArr_to_eRGB(inputColor);
            if (result != unitColor)
            {
                SetColor(result);
            }
        }

        /// <summary>
        /// 해당 유닛에 매개변수 컬러를 제거합니다.
        /// </summary>
        /// <param name="color"></param>
        public void RemoveColor(eRGB color)
        {
            //현재 컬러
            bool[] currentColor = rgbValue;
            //제거하는 컬러값
            bool[] colorRGBValue = MColorUtility.Generate_eRGB_to_BoolArr(color);
            //최종 컬러
            bool[] inputColor = new bool[3];

            for (int i = 0; i < currentColor.Length; i++)
            {
                //ex compare : {true, true, false} {true, false, true}
                if (currentColor[i])
                {
                    inputColor[i] = (currentColor[i] && colorRGBValue[i]) ? false : true;
                }
                else
                {
                    inputColor[i] = false;
                }
            }

            eRGB result = MColorUtility.Generate_BoolArr_to_eRGB(inputColor);
            if (result != unitColor)
            {
                SetColor(result);
            }
        }

        /// <summary>
        /// 미구현) where 방향에 대해서 value값으로 방향을 결정합니다.
        /// </summary>
        /// <param name="where"></param>
        /// <param name="value"></param>
        public void SetInputDirection(int where, bool value)
        {

        }

        /// <summary>
        /// 미구현) bool[] values 배열리스트로 유닛의 인풋방향을 초기화합니다.
        /// </summary>
        /// <param name="values"></param>
        public void SetInputDirection(bool[] values)
        {

        }
        #endregion


        #region Private/Protected Methods
        /// <summary>
        /// GameManager의 턴 시작 이벤트 발생 시 자동으로 호출되는 메서드
        /// </summary>
        protected virtual void OnTurnStart()
        {

        }

        /// <summary>
        /// GameManager의 턴 종료 이벤트 발생 시 자동으로 호출되는 메서드
        /// </summary>
        protected virtual void OnTurnEnd()
        {
            if (cur_Tile == null) return;       //타일이 없을경우 => 배치되지 않았을 경우 동작하지 않도록

            //머지를 먼저할지 마이닝을 먼저할지는 추후
            //일단 머지 먼저, 그후 마이닝

            //머지

            //마이닝
            //추후 유닛코드 내가 아니라 헥사곤 유틸리티에서 처리되도록 이전 필요
            //자신의 주변에 있는 유닛중에 자신을 향해 인풋방향이 있고, 그 유닛의 컬러가 자신을 포함하거나 동일할 경우 실행X (해당유닛이 실행해줄 것이기 때문에)

            //자신의 주변에 자신을 향한 인풋을 가진 유닛이 있는지 체크
            //주변타일 검색
            Tile[] aroundTiles = MHexagonUtility.GetNeighborTile(cur_Tile);
            for (int i = 0; i < aroundTiles.Length; i++)
            {
                //주변타일 중 누군가가 자신을 포함하고 있으면?
                if (MHexagonUtility.IsNeighborTileContainMe_throughUnit(this, aroundTiles[i]))
                {
                    //동작하지 않도록
                    return;
                }
            }

            //그렇지 않을 경우 자신이 자원채취의 시발점이다.
            //재귀형으로 작업
            recursioningMining();
        }

        /// <summary>
        /// 자원채취의 시발점일 경우 자신으로부터 재귀형으로 연결된 타일들을 순서대로 호출합니다.
        /// 재귀도중 자신의 인풋방향에 광산이 있을 경우 광산채취를, 아닐 경우 연결된 타일로부터 자원가져오기를 시도합니다.
        /// 유닛 스크립트가 하는 일이 많아 그냥 어디다 옮기고 싶은 심정
        /// </summary>
        private void recursioningMining()
        {
            Tile[] InputDir_Tiles = MHexagonUtility.GetTile_throughInputDir(cur_Tile, input_Dir);
            for (int i = 0; i < InputDir_Tiles.Length; i++)
            {
                if (InputDir_Tiles[i].MiningField != null && InputDir_Tiles[i].Units.Count == 0)
                {
                    Mining();
                }
                else if (InputDir_Tiles[i].Units.Count != 0)
                {
                    var unit_arr = InputDir_Tiles[i].Units.ToArray();
                    for (int j = 0; j < unit_arr.Length; j++)
                    {
                        if (MColorUtility.IsAcolr_Contain_BColor(unitColor, unit_arr[j].unitColor))
                        {
                            //일단 한번만 캐오도록 (재귀를 모두 마치고 다시 역순으로 GetResourceFromInputDirTile이 실행됨)
                            unit_arr[j].recursioningMining();
                        }
                    }
                }
            }
            GetResourceFromInputDirTile();
        }


        private void Merge()
        {

        }



        /// <summary>
        /// InputDir방향에 광산이 있을 경우 자원을 가지고 옵니다.
        /// </summary>
        private void Mining()
        {
            var _tiles = MHexagonUtility.GetTile_throughInputDir(this.Cur_Tile, Input_Dir);
            for (int i = 0; i < _tiles.Length; i++)
            {
                if(_tiles[i].MiningField != null)
                {
                    Res _result;
                    if (_tiles[i].MiningField.TryMining(this, unitColor, out _result))
                    {
                        if (_result != null)
                        {
                            cur_Tile.AddRes(_result);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// InputDir방향의 타일으로부터 자원을 가지고 옵니다.
        /// 가지고 온 자원을 다시 자신의 타일로 쌓습니다.
        /// 현재 이 코드는 자원을 무조건 자신의 컬러로 변환한다는 문제가 있음
        /// </summary>
        private void GetResourceFromInputDirTile()
        {
            //new 
            var tiles = MHexagonUtility.GetTile_throughInputDir(cur_Tile, Input_Dir);
            for (int i = 0; i < tiles.Length; i++)
            {
                var res = tiles[i].PopRes(unitColor);
                if(res!= null)
                {
                    cur_Tile.AddRes(res);
                }
            }

            //old
            //var tiles = MHexagonUtility.GetNeighborTile(cur_Tile);
            //for (int i = 0; i < input_Dir.Length; i++)
            //{
            //    if (input_Dir[i] && tiles[i] != null)
            //    {

            //        var res = tiles[i].PopRes(unitColor);
            //        if (res != null)
            //        {
            //            cur_Tile.AddRes(res);
            //        }
            //    }
            //}
        }


        #endregion Private/Protected Methods
    }
}