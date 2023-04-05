using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lagacy_Hexagon.Moru;
using Sirenix.OdinInspector;


namespace Lagacy_Hexagon
{
    namespace Moru
    {
        [System.Serializable]
        public class ResourcesFiled
        {
            #region Field
            [SerializeField, LabelText("�ڿ� �ܿ���")]
            private int[] reserves_Resources = new int[3];
            /// <summary>
            /// ���� �����ִ� �ڿ��ܷ��� �ǹ��մϴ�.
            /// </summary>
            public int[] Reserves_Resources => reserves_Resources;

            [SerializeField, LabelText("�ʱ� �ڿ� �ܿ���"), ReadOnly]
            private int[] init_Reserves = new int[3];
            /// <summary>
            /// ���� ������ �ڿ����Դϴ�.
            /// </summary>
            private int[] Init_Reserves => init_Reserves;

            [SerializeField, LabelText("���� ��ġ�� Ÿ�ϵ�����")]
            Tile cur_Tile;
            public Tile Cur_Tile => cur_Tile;

            [SerializeField, LabelText("�ڿ����� ��� ������Ʈ"), ReadOnly]
            ResourcesFieldViewer ResFieldViewer;
            #endregion


            #region Properties
            #endregion


            #region Events
            /// <summary>
            /// �ڿ��ܷ��� ���� �� ����Ǵ� ��������Ʈ �޼���
            /// </summary>
            public OnValueChange_Params<int[]> onReservesChange;
            #endregion


            #region Constructor

            /// <summary>
            /// �ڿ������� ���� �����ڷ�, �ڿ����差�� �����մϴ�.
            /// �Ű��������� null�� ���, ������ ������ �ʱ�ȭ�˴ϴ�.
            /// </summary>
            /// <param name="init_ReservesAmount"></param>
            public ResourcesFiled(Tile targetTile, int[] init_ReservesAmount = null)
            {

                if (init_ReservesAmount == null)
                {
                    System.Random random = new System.Random();
                    for (int i = 0; i < init_Reserves.Length; i++)
                    {
                        init_Reserves[i] = random.Next(100, 200);
                        reserves_Resources[i] = init_Reserves[i];
                    }
                }
                else
                {
                    init_Reserves = init_ReservesAmount;
                    reserves_Resources = init_Reserves;
                }
                cur_Tile = targetTile;

                //GameObject obj = MonoBehaviour.Instantiate(TsetMono.instance.testFieldPrefap);
                //if (obj.TryGetComponent<ResourcesFieldViewer>(out var comp))
                //{
                //    ResFieldViewer = comp;
                //    comp.Init(this);
                //}
                //else
                //{
                //    ResFieldViewer = obj.AddComponent<ResourcesFieldViewer>();
                //    ResFieldViewer.Init(this);
                //}
            }
            #endregion


            #region Public Methods
            /// <summary>
            /// �Ű��������� �÷��� ä���ϴ� ���� �õ� ��, �õ������ ��ȯ�մϴ�. out resources�� null�� �� �� �ֽ��ϴ�.
            /// </summary>
            /// <param name="targetResources"></param>
            public bool TryMining(Unit who, eRGB targetResources, out Res resources)
            {
                if (targetResources == eRGB.Black)
                {
                    resources = null;
                    Debug.Log($"�߸��� �ڿ�ĳ�� �Է°� �Ű����� : {targetResources}. �޼��� ����");
                    return false;
                }
                //���� �ڿ���
                int[] current_Reserves = reserves_Resources;
                //ä�� ����� ������ boolean �迭
                bool[] colorResult = new bool[3];
                //ä�� �õ��� Ÿ���ڿ��� boolean arr�� ��ȯ
                bool[] targetRGB_Value = MColorUtility.Generate_eRGB_to_BoolArr(targetResources);

                //���η���
                for (int i = 0; i < targetRGB_Value.Length; i++)
                {
                    if (targetRGB_Value[i])
                    {
                        if (reserves_Resources[i] > 0)
                        {
                            reserves_Resources[i]--;
                            colorResult[i] = true;
                        }
                        else
                        {
                            //�ڿ��� ���ڶ�
                            //�ϴ� ���ڶ�� ���ڶ��� �����ϰ� ĳ���� ����
                        }
                    }
                }

                eRGB enumTypeRGB_Result = MColorUtility.Generate_BoolArr_to_eRGB(colorResult);

                //resources�� black�̸� (�ڿ��� �ƿ� ä�븦 ��������)
                if (enumTypeRGB_Result == eRGB.Black)
                {
                    resources = null;
                    Debug.Log($"{who}�� {targetResources}�ڿ�ĳ�� �õ�, ����� : {enumTypeRGB_Result} ::�ڿ��� ĳ�µ��� �����Ͽ���");
                    return false;
                }

                //���� �� out���� �Ѱ��ְ� ĵ ������ �˾Ƽ� ó���ϵ��� ����,
                resources = new Res(enumTypeRGB_Result, who.Cur_Tile);

                onReservesChange?.Invoke(current_Reserves, reserves_Resources);
                Debug.Log($"{who}�� {targetResources}�ڿ�ĳ�� �õ�, ����� : {enumTypeRGB_Result} ::�ڿ��� ĳ�µ��� �����Ͽ���");
                return true;
            }

            /// <summary>
            /// �Ű��������� �÷��� ä���ϴ� ���� �õ� ��, �õ������ ��ȯ�մϴ�.
            /// Ư���÷��� ���ϴ� ��ŭ Ķ �� �ֽ��ϴ�. out resources�� 0�� �� �� �ֽ��ϴ�.
            /// </summary>
            /// <param name="targetResources"></param>
            public bool TryMining(Unit who, eRGB targetResources, int amount, out Res[] resources)
            {
                List<Res> _res = new List<Res>();
                for (int i = 0; i < amount; i++)
                {
                    if (TryMining(who, targetResources, out var result))
                    {
                        _res.Add(result);
                    }
                }
                resources = _res.ToArray();
                if (resources.Length == 0)
                {
                    return false;
                }
                return true;
            }


            /// <summary>
            /// ..���� �̱���..
            /// �Ű��������� �÷��迭�� ä���ϴ� ���� �õ� ��, �õ������ ��ȯ�մϴ�.
            /// �ѹ��� ���ϴ� ��ŭ Ķ �� �ֽ��ϴ�.
            /// </summary>
            /// <param name="targetResources"></param>
            public bool TryMining(Unit who, int[] targetResources, out Res[] resources)
            {

                resources = new Res[0];
                return true;
            }

            /// <summary>
            /// �ڿ����差�� 0���� ����ϴ�.
            /// </summary>
            public void SetClear()
            {
                int[] current_Reserves = reserves_Resources;
                reserves_Resources = new int[3] { 0, 0, 0 };
                onReservesChange?.Invoke(current_Reserves, reserves_Resources);
            }

            /// <summary>
            /// �ڿ����差�� �Ű����������� �ʱ�ȭ�մϴ�. amount �迭 �Ű������� null�� ������ ���
            /// �ڵ����� ���� �������� �ڿ����差���� �ʱ�ȭ�մϴ�.
            /// </summary>
            /// <param name="amount"></param>
            public void SetReserves_Amount(int[] amount = null)
            {
                int[] current_Reserves = reserves_Resources;
                if (amount == null)
                {
                    reserves_Resources = init_Reserves;
                }
                else
                {
                    reserves_Resources = amount;
                }
                onReservesChange?.Invoke(current_Reserves, reserves_Resources);
            }
            #endregion


            #region Private/Protected Methods
            #endregion
        }
    }
}