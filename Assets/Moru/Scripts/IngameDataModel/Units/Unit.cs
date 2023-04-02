using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Moru;
using Sirenix.OdinInspector;

//���ӽ����̽� ��ߵɼ���
namespace Moru
{
    [System.Serializable]
    public class Unit
    {
        #region Field
        #region Info Field
        protected string unitName = new string("");
        /// <summary>
        /// ������ �̸��Դϴ�.
        /// </summary>
        public string UnitName => unitName;


        protected int level = 0;
        /// <summary>
        /// ������ ����/����Դϴ�.
        /// </summary>
        public int Level => level;
        #endregion


        #region Value Field
        //....Color....//
        [ShowInInspector, LabelText("RGB ���� ���� �迭"), TitleGroup("�߿� �����Ͱ�")]
        protected bool[] rgbValue = new bool[3] { false, false, false };

        /// <summary>
        /// �� RGB���� ����迭�Դϴ�. 0 : R // G : 1 // B : 2
        /// </summary>
        public bool[] RGBValue => rgbValue;

        /// <summary>
        /// RGB�� eRGB�������� �޾ƿɴϴ�.
        /// </summary>
        [ShowInInspector, LabelText("�����÷�")]
        public eRGB unitColor
        { get { return MColorUtility.Generate_BoolArr_to_eRGB(RGBValue); } }

        //....Direction....//
        [SerializeField, LabelText("��ǲ����")]
        protected bool[] input_Dir = new bool[6];
        /// <summary>
        /// ������ ��ǲ������ �������ִ� boolean�Դϴ�. 0��(1�ù���)���� �ð���������� ���� ��ǲ�����Դϴ�.
        /// </summary>
        public bool[] Input_Dir => input_Dir;

        protected int max_MoveCount = 1;
        /// <summary>
        /// ������ �ִ�� �̵��� �� �ִ� Ÿ�� ���Դϴ�.
        /// </summary>
        public int Max_MoveCount => max_MoveCount;


        //....Move....//

        protected int cur_MoveCount = 0;
        /// <summary>
        /// ���� �̵������� Ÿ�� ���Դϴ�.
        /// </summary>
        public int Cur_MoveCount => cur_MoveCount;
        #endregion


        #region Reference Value
        /// <summary>
        /// ������ ���� ��ġ�� Ÿ��
        /// </summary>
        /// 
        [SerializeField, TitleGroup("������"), LabelText("���� �ڽ��� Ÿ��")]
        protected Tile cur_Tile;
        public Tile Cur_Tile => cur_Tile;

        [SerializeField, LabelText("���")] 
        protected Viewer<Unit> myUnitViewer;

        #endregion
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
        protected Unit()
        {

        }
        /// <summary>
        /// ������ ���ֵ����͸� �����ϰ��� �� ����� �������Դϴ�.
        /// </summary>
        public Unit(Tile targetTile) : base()
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

            cur_Tile = targetTile;
            //TsetMono.onTurnEnd += OnTurnEnd;
            ////Test���� ����
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
        /// ��ü���� ������ �÷��� ������ 
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
        /// ���� ���� �� �⺻����� �ʱ�ȭ
        /// </summary>
        public virtual void Init()
        {
            myUnitViewer.Init(this);
            cur_Tile.PushUnit(this);

        }

        /// <summary>
        /// �ܺ��̺�Ʈ�鿡 ���� �������
        /// </summary>
        public void OnDestroy()
        {

        }



        /// <summary>
        /// �ش� Ÿ�Ϸ� �̵���ŵ�ϴ�.
        /// </summary>
        public void OnMove(Tile target)
        {
            cur_Tile = target;
            this.Init();
            myUnitViewer.Init(this);
        }

        /// <summary>
        /// ������ �ش� Ÿ�Ϸ� ���� ��ġ�մϴ�.
        /// </summary>
        /// <param name="target"></param>
        public void OnLocate(Tile target)
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

        /// <summary>
        /// �̱���) where ���⿡ ���ؼ� value������ ������ �����մϴ�.
        /// </summary>
        /// <param name="where"></param>
        /// <param name="value"></param>
        public void SetInputDirection(int where, bool value)
        {

        }

        /// <summary>
        /// �̱���) bool[] values �迭����Ʈ�� ������ ��ǲ������ �ʱ�ȭ�մϴ�.
        /// </summary>
        /// <param name="values"></param>
        public void SetInputDirection(bool[] values)
        {

        }
        #endregion


        #region Private/Protected Methods
        /// <summary>
        /// GameManager�� �� ���� �̺�Ʈ �߻� �� �ڵ����� ȣ��Ǵ� �޼���
        /// </summary>
        protected virtual void OnTurnStart()
        {

        }

        /// <summary>
        /// GameManager�� �� ���� �̺�Ʈ �߻� �� �ڵ����� ȣ��Ǵ� �޼���
        /// </summary>
        protected virtual void OnTurnEnd()
        {
            if (cur_Tile == null) return;       //Ÿ���� ������� => ��ġ���� �ʾ��� ��� �������� �ʵ���

            //������ �������� ���̴��� ���������� ����
            //�ϴ� ���� ����, ���� ���̴�

            //����

            //���̴�
            //���� �����ڵ� ���� �ƴ϶� ���� ��ƿ��Ƽ���� ó���ǵ��� ���� �ʿ�
            //�ڽ��� �ֺ��� �ִ� �����߿� �ڽ��� ���� ��ǲ������ �ְ�, �� ������ �÷��� �ڽ��� �����ϰų� ������ ��� ����X (�ش������� �������� ���̱� ������)

            //�ڽ��� �ֺ��� �ڽ��� ���� ��ǲ�� ���� ������ �ִ��� üũ
            //�ֺ�Ÿ�� �˻�
            Tile[] aroundTiles = MHexagonUtility.GetNeighborTile(cur_Tile);
            for (int i = 0; i < aroundTiles.Length; i++)
            {
                //�ֺ�Ÿ�� �� �������� �ڽ��� �����ϰ� ������?
                if (MHexagonUtility.IsNeighborTileContainMe_throughUnit(this, aroundTiles[i]))
                {
                    //�������� �ʵ���
                    return;
                }
            }

            //�׷��� ���� ��� �ڽ��� �ڿ�ä���� �ù����̴�.
            //��������� �۾�
            recursioningMining();
        }

        /// <summary>
        /// �ڿ�ä���� �ù����� ��� �ڽ����κ��� ��������� ����� Ÿ�ϵ��� ������� ȣ���մϴ�.
        /// ��͵��� �ڽ��� ��ǲ���⿡ ������ ���� ��� ����ä�븦, �ƴ� ��� ����� Ÿ�Ϸκ��� �ڿ��������⸦ �õ��մϴ�.
        /// ���� ��ũ��Ʈ�� �ϴ� ���� ���� �׳� ���� �ű�� ���� ����
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
                            //�ϴ� �ѹ��� ĳ������ (��͸� ��� ��ġ�� �ٽ� �������� GetResourceFromInputDirTile�� �����)
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
        /// InputDir���⿡ ������ ���� ��� �ڿ��� ������ �ɴϴ�.
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
        /// InputDir������ Ÿ�����κ��� �ڿ��� ������ �ɴϴ�.
        /// ������ �� �ڿ��� �ٽ� �ڽ��� Ÿ�Ϸ� �׽��ϴ�.
        /// ���� �� �ڵ�� �ڿ��� ������ �ڽ��� �÷��� ��ȯ�Ѵٴ� ������ ����
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