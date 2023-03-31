using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Moru;

//���ӽ����̽� ��ߵɼ���
namespace Moru
{
    [System.Serializable]
    public class Unit
    {
        #region Info Field


        private string unitName = new string("");
        /// <summary>
        /// ������ �̸��Դϴ�.
        /// </summary>
        public string UnitName => unitName;


        private int level = 0;
        /// <summary>
        /// ������ ����/����Դϴ�.
        /// </summary>
        public int Level => level;

        #endregion


        #region Value Field

        //....Color....//

        private bool[] rgbValue = new bool[3] { false, false, false };
        /// <summary>
        /// �� RGB���� ����迭�Դϴ�. 0 : R // G : 1 // B : 2
        /// </summary>
        public bool[] RGBValue => rgbValue;
        /// <summary>
        /// RGB�� eRGB�������� �޾ƿɴϴ�.
        /// </summary>
        public eRGB unitColor
        { get { return MColorUtility.Generate_BoolArr_to_eRGB(RGBValue); } }

        //....Direction....//

        private bool[] input_Dir = new bool[6];
        /// <summary>
        /// ������ ��ǲ������ �������ִ� boolean�Դϴ�. 0��(1�ù���)���� �ð���������� ���� ��ǲ�����Դϴ�.
        /// </summary>
        public bool[] Input_Dir => input_Dir;

        private int max_MoveCount = 1;
        /// <summary>
        /// ������ �ִ�� �̵��� �� �ִ� Ÿ�� ���Դϴ�.
        /// </summary>
        public int Max_MoveCount => max_MoveCount;


        //....Move....//

        private int cur_MoveCount = 0;
        /// <summary>
        /// ���� �̵������� Ÿ�� ���Դϴ�.
        /// </summary>
        public int Cur_MoveCount => cur_MoveCount;

        #endregion


        #region Events

        /// <summary>
        /// �÷����� ���� �� ȣ���Ű�� �޼���
        /// </summary>
        private OnValueChange_Params<bool[]> onColorChange;
        /// <summary>
        /// �÷����� ���� �� ȣ���Ű�� �޼���
        /// </summary>
        public OnValueChange OnColorChange;

        #endregion


        #region Constructor

        /// <summary>
        /// ������ ���ֵ����͸� �����ϰ��� �� ����� �������Դϴ�.
        /// </summary>
        public Unit()
        {
            System.Random random = new System.Random();
            //������ ���ֵ����͸� DataBase�κ��� �޾ƿ�
            //������ �÷�? (�÷��� ���ָ��� ��������������) (�ϴ� ������ �÷���)
            int randomColor = random.Next(0, 3);
            eRGB randomResult = (eRGB)randomColor;
            rgbValue = MColorUtility.Generate_eRGB_to_BoolArr(randomResult);

            //������ ���� (�ϴ� �� ���⸸)
            int RandomDir = random.Next(0, 6);
            for (int i = 0; i < input_Dir.Length; i++)
            {
                input_Dir[i] = (RandomDir == i) ? true : false;
            }
        }

        /// <summary>
        /// ��ü���� ������ �÷��� ������ 
        /// </summary>
        /// <param name="_unitColor"></param>
        /// <param name="_input_Dir"></param>
        public Unit(eRGB _unitColor, bool[] _input_Dir) : base() //������� : base()����
        {
            this.rgbValue = MColorUtility.Generate_eRGB_to_BoolArr(_unitColor);
            this.input_Dir = _input_Dir;
            Init();
        }


        #endregion



        #region Public Methods

        /// <summary>
        /// ���� ���� �� �⺻����� �ʱ�ȭ
        /// </summary>
        public void Init()
        {

        }

        /// <summary>
        /// �ܺ��̺�Ʈ�鿡 ���� �������
        /// </summary>
        public void OnDestroy()
        {

        }



        /// <summary>
        /// �Ķ���� : Ÿ����ġ
        /// </summary>
        public void OnMove(object target)
        {


        }

        /// <summary>
        /// �ش� ������ �÷��� �Ű����������� �ٲߴϴ�.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(eRGB color)
        {
            bool[] origin = rgbValue;
            bool[] recent = MColorUtility.Generate_eRGB_to_BoolArr(color);
            rgbValue = recent;

            //�޼��� ����
            if (onColorChange != null) 
            { 
                onColorChange.Invoke(origin, recent); 
                OnColorChange.Invoke(); 
            }
        }

        /// <summary>
        /// �ش� ���ֿ� �Ű����� �÷��� ���մϴ�.
        /// </summary>
        /// <param name="color"></param>
        public void AddColor(eRGB color)
        {
            //���� �÷�
            bool[] currentColor = rgbValue;
            //���ϴ� �÷���
            bool[] colorRGBValue = MColorUtility.Generate_eRGB_to_BoolArr(color);
            //���� �÷�
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
        /// �ش� ���ֿ� �Ű����� �÷��� �����մϴ�.
        /// </summary>
        /// <param name="color"></param>
        public void RemoveColor(eRGB color)
        {
            //���� �÷�
            bool[] currentColor = rgbValue;
            //�����ϴ� �÷���
            bool[] colorRGBValue = MColorUtility.Generate_eRGB_to_BoolArr(color);
            //���� �÷�
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
        /// GameManager�� �� ���� �̺�Ʈ �߻� �� �ڵ����� ȣ��Ǵ� �޼���
        /// </summary>
        private void OnTurnStart()
        {

        }

        /// <summary>
        /// GameManager�� �� ���� �̺�Ʈ �߻� �� �ڵ����� ȣ��Ǵ� �޼���
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