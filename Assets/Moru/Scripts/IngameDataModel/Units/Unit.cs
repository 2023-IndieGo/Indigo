using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Moru;

//네임스페이스 까야될수도
namespace Moru
{
    [System.Serializable]
    public class Unit
    {
        #region Info Field


        private string unitName = new string("");
        /// <summary>
        /// 유닛의 이름입니다.
        /// </summary>
        public string UnitName => unitName;


        private int level = 0;
        /// <summary>
        /// 유닛의 레벨/등급입니다.
        /// </summary>
        public int Level => level;

        #endregion


        #region Value Field

        //....Color....//

        private bool[] rgbValue = new bool[3] { false, false, false };
        /// <summary>
        /// 각 RGB값의 밸류배열입니다. 0 : R // G : 1 // B : 2
        /// </summary>
        public bool[] RGBValue => rgbValue;
        /// <summary>
        /// RGB를 eRGB형식으로 받아옵니다.
        /// </summary>
        public eRGB unitColor
        { get { return MColorUtility.Generate_BoolArr_to_eRGB(RGBValue); } }

        //....Direction....//

        private bool[] input_Dir = new bool[6];
        /// <summary>
        /// 유닛의 인풋라인을 결정해주는 boolean입니다. 0번(1시방향)부터 시계방향으로의 면이 인풋방향입니다.
        /// </summary>
        public bool[] Input_Dir => input_Dir;

        private int max_MoveCount = 1;
        /// <summary>
        /// 유닛이 최대로 이동할 수 있는 타일 수입니다.
        /// </summary>
        public int Max_MoveCount => max_MoveCount;


        //....Move....//

        private int cur_MoveCount = 0;
        /// <summary>
        /// 현재 이동가능한 타일 수입니다.
        /// </summary>
        public int Cur_MoveCount => cur_MoveCount;

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

        /// <summary>
        /// 랜덤한 유닛데이터를 생성하고자 할 경우의 생성자입니다.
        /// </summary>
        public Unit()
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
        }

        /// <summary>
        /// 구체적인 유닛의 컬러와 방향을 
        /// </summary>
        /// <param name="_unitColor"></param>
        /// <param name="_input_Dir"></param>
        public Unit(eRGB _unitColor, bool[] _input_Dir) : base() //실행순서 : base()부터
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
        public void Init()
        {

        }

        /// <summary>
        /// 외부이벤트들에 대한 구독취소
        /// </summary>
        public void OnDestroy()
        {

        }



        /// <summary>
        /// 파라미터 : 타겟위치
        /// </summary>
        public void OnMove(object target)
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

        public void SetInputDirection(int where, bool value)
        {

        }

        public void SetInputDirection(bool[] values)
        {

        }

        #endregion


        #region Private/Protected Methods

        /// <summary>
        /// GameManager의 턴 시작 이벤트 발생 시 자동으로 호출되는 메서드
        /// </summary>
        private void OnTurnStart()
        {

        }

        /// <summary>
        /// GameManager의 턴 종료 이벤트 발생 시 자동으로 호출되는 메서드
        /// </summary>
        private void OnTurnEnd()
        {

        }


        private void Merge()
        {

        }


        #endregion Private/Protected Methods
    }
}