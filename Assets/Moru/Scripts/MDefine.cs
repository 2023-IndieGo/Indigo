using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region Field
#endregion


#region Properties
#endregion


#region Events
#endregion


#region Constructor
#endregion


#region Public Methods
#endregion


#region Private/Protected Methods
#endregion

namespace Moru
{
    public delegate void OnGameStateChagne();

    public enum eRGB
    {
        Red = 0,
        Green,
        Blue,
        Cyan,           //B+G
        Magenta,        //R+B
        Yellow,         //R+G
        White,          //R+G+B
        Black           //None
    }

    public delegate void OnValueChange();
    public delegate void OnValueChange_Params<T>(T origin, T input);


    public class MColorUtility
    {
        private static Dictionary<eRGB, bool[]> dic_eRGB_To_bool;

        private static void Init()
        {
            {
                dic_eRGB_To_bool = new Dictionary<eRGB, bool[]>();
                dic_eRGB_To_bool.Add(eRGB.Red, new bool[3] { true, false, false });
                dic_eRGB_To_bool.Add(eRGB.Blue, new bool[3] { false, false, true });
                dic_eRGB_To_bool.Add(eRGB.Green, new bool[3] { false, true, false });
                dic_eRGB_To_bool.Add(eRGB.Magenta, new bool[3] { true, false, true });
                dic_eRGB_To_bool.Add(eRGB.Cyan, new bool[3] { false, true, true });
                dic_eRGB_To_bool.Add(eRGB.Yellow, new bool[3] { true, true, false });
                dic_eRGB_To_bool.Add(eRGB.White, new bool[3] { true, true, true });
                dic_eRGB_To_bool.Add(eRGB.Black, new bool[3] { false, false, false });

            }
        }

        /// <summary>
        /// eRGB�� ���� bool[]������ ��ȯ�� �����Ͱ��� �޾ƿɴϴ�.
        /// </summary>
        /// <param name="value"></param>
        public static bool[] Generate_eRGB_to_BoolArr(eRGB value)
        {
            if (dic_eRGB_To_bool == null)
            { Init(); }

            bool[] result = new bool[3];

            if (dic_eRGB_To_bool.TryGetValue(value, out result))
            {
                return result;
            }
            else
            {
                result = new bool[3] { false, false, false };
                return result;
            }
        }

        /// <summary>
        /// bool[]�� ���� eRGB�� ��ȯ�� �����Ͱ��� �޾ƿɴϴ�.
        /// </summary>
        /// <param name="value"></param>
        public static eRGB Generate_BoolArr_to_eRGB(bool[] value)
        {
            //�����ߴ� ���۴�� �ȵǼ� �ϴ� �ϵ��ڵ�
            if (value[0])
            {
                if (value[1])
                {
                    if (value[2])
                        return eRGB.White;
                    else return eRGB.Yellow;
                }
                else if (value[2]) return eRGB.Magenta;
                else return eRGB.Red;
            }
            else if (value[1])
            {
                if (value[2]) return eRGB.Cyan;
                else return eRGB.Green;
            }
            else if (value[2])
                return eRGB.Blue;
            else
            {
                var stringValue = DebugUtillity.ArrayToString(value);
                Debug.Log($"{stringValue }�� Dic�� ���ԵǾ����� �ʽ��ϴ�.");
                return eRGB.Black;
            }

        }

        /// <summary>
        /// �÷� ���÷��� ǥ���� �ܰ��� ���Դϴ�.
        /// </summary>
        private const int colorThreshold_Level = 50;
        
        /// <summary>
        /// �ش簪 ���Ϸ��� �÷�rgb���� �������� �ʽ��ϴ�.
        /// </summary>
        private const float minColorValue = 0.3f;
        /// <summary>
        /// �ش簪 �̻������� �÷�rgb���� �ö��� �ʽ��ϴ�.
        /// </summary>
        private const float maxColorValue = 0.9f;

        /// <summary>
        /// �ش簪 ���Ϸκ����� ���밪�� ���� �÷�ǥ�� ����� ����˴ϴ�.
        /// </summary>
        private const int maxResValue_Per_ColorThreshold = 100;

        /// <summary>
        /// ���� ���� ���� �� ǥ�õ� �÷����� �ʱ�ȭ�մϴ�. (���밪)
        /// </summary>
        /// <param name="rgbValue"></param>
        /// <returns></returns>
        public static float[] ColorRateInitializer(int[] rgbValue)
        {
            if(rgbValue == null)
            {
                Debug.Log("�Ű������� null�Դϴ�. �޼��� ����");
                return new float[3] { 0.5f, 0.5f, 0.5f };
            }
            float[] result = new float[3];
            int cur_Max = rgbValue[0];
            for (int i = 0; i < rgbValue.Length; i++)
            {

            }
            return result;
        }

        /// <summary>
        /// A�÷��� B�÷��� �����ϴ��� ���� ���� ��ȯ�մϴ�.
        /// ex) Red�� Magenta�� �������� ������ Magenta�� Red�� �����մϴ�.
        /// ����� �����÷��� ���ٰ� �����մϴ�.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool IsAcolr_Contain_BColor(eRGB A, eRGB B)
        {
            bool[] AValue = MColorUtility.Generate_eRGB_to_BoolArr(A);
            bool[] BValue = MColorUtility.Generate_eRGB_to_BoolArr(B);
            bool result = false;
            for (int i = 0; i < AValue.Length; i++)
            {
                if(AValue[i])   //�ش� �迭�� ������ �䱸�� ��
                {
                    if(BValue[i]) //�ش������ ���� ���
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }

            }
            //ȭ��Ʈ�� ������ ��������
            if(A == eRGB.White)
            {
                result = true;
            }

            return result;
        }

        public static Color GetColor(eRGB rgb)
        {
            Color color = new Color();
            switch(rgb)
            {
                case eRGB.Red:
                    color = Color.red;
                    break;
                case eRGB.Blue:
                    color = Color.blue;
                    break;
                case eRGB.Green:
                    color = Color.green;
                    break;
                case eRGB.Cyan:
                    color = Color.cyan;
                    break;
                case eRGB.Magenta:
                    color = Color.magenta;
                    break;
                case eRGB.Yellow:
                    color = Color.yellow;
                    break;
                case eRGB.White:
                    color = Color.white;
                    break;
                case eRGB.Black:
                    color = Color.black;
                    break;
            }
            color = color * 0.8f;
            return color;
        }
    }

    public class MergeUtillity
    {


    }


    public class DebugUtillity
    {
        public static string ArrayToString<T>(T[] arr)
        {
            string result = $"\n{arr} : Array Result-----\n";
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == null) continue;
                result += arr[i].ToString();
                result += "\n";
            }
            result += "----Array End----\n";
            return result;
        }
        public static string ListToString<T>(List<T> arr)
        {
            string result = $"\n{arr} : List Result-----\n";
            for (int i = 0; i < arr.Count; i++)
            {
                result += arr[i].ToString();
                result += "\n";
            }
            result += "----List End----\n";
            return result;
        }
    }

    public class MHexagonUtility
    {
        /// <summary>
        /// �Ű����� int�� ���� ������ ����Ÿ�Ͽ����� 360���� �������� ��ȯ�մϴ�. 0=0, 1 = 60 ,...
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static float GetDirAngle(int num)
        {
            int _changeNum = num % 6;
            float result = _changeNum * 60;
            return result;
        }
        /// <summary>
        /// �Ű����� bool[]�� ���� ������ ����Ÿ�Ͽ����� 360���� �������� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static float GetDirAngle(bool[] dir_Arr)
        {
            int trueID = 0;
            for (int i = 0; i < dir_Arr.Length; i++)
            {
                if(dir_Arr[i])
                {
                    trueID = i;
                    break;
                }
            }
            float result = trueID * -60;
            return result;
        }

        /// <summary>
        /// Ÿ���� �ֺ�Ÿ�� 6���� �޾ƿɴϴ�.
        /// </summary>
        /// <param name="selectedTile"></param>
        /// <returns></returns>
        public static TestTile[] GetNeighborTile(TestTile selectedTile)
        {
            TestTile[] result = new TestTile[6];
            int x = selectedTile.adressX;
            int y = selectedTile.adressY;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = null;
            }
            //Ȧ��° �ε����� ���
            if (selectedTile.adressY % 2 != 1)
            {
                //������ 0���� Ŭ���
                if (y - 1 >= 0)
                {
                    result[0] = TestMap.GetTile(x, y - 1);

                }
                //�Ʒ��� �ִ밪���� �������
                if (y + 1 < TestMap.y)
                {

                    result[3] = TestMap.GetTile(x, y + 1);
                    if (x - 1 >= 0)
                    {
                        result[5] = TestMap.GetTile(x - 1, y);
                        result[4] = TestMap.GetTile(x - 1, y + 1);
                    }
                    if (x + 1 < TestMap.x)
                    {
                        result[1] = TestMap.GetTile(x + 1, y);
                        result[2] = TestMap.GetTile(x + 1, y + 1);
                    }
                }
            }
            //¦��° �ε����� ���
            else
            {
                if (y - 1 >= 0)
                {
                    result[0] = TestMap.GetTile(x, y - 1);
                    if (x - 1 >= 0)
                    {
                        result[5] = TestMap.GetTile(x - 1, y - 1);
                        result[4] = TestMap.GetTile(x - 1, y);
                    }
                    if (x + 1 < TestMap.x)
                    {
                        result[1] = TestMap.GetTile(x + 1, y - 1);
                        result[2] = TestMap.GetTile(x + 1, y);
                    }

                }
                if (y + 1 < TestMap.y)
                {
                    result[3] = TestMap.GetTile(x, y + 1);
                }
            }


            return result;
        }

        /// <summary>
        /// 2��° �Ű����� Ÿ���� �ڽ��� �����ϴ� Ÿ������ ������ Input������ ���ؼ� �Ǵ��մϴ�. ������ ���� ��� �׻� false
        /// </summary>
        /// <param name="myTile"></param>
        /// <param name="targetTile"></param>
        /// <returns></returns>
        public static bool IsNeighborTileContainMe_throughUnit(Unit cur_Unit, TestTile targetTile)
        {
            if (targetTile == null) return false;
            if (targetTile.Units.Count == 0) return false;

            Unit[] units = targetTile.Units.ToArray();
            //���ֵ��� ��
            for (int i = 0; i < units.Length; i++)
            {
                bool[] unitDirInfo = units[i].Input_Dir;
                TestTile[] aroundTile = GetNeighborTile(targetTile);

                for (int j = 0; j < unitDirInfo.Length; j++)
                {
                    if (unitDirInfo[j] && aroundTile[j] != null)
                    {
                        if (aroundTile[j].Units.Contains(cur_Unit))
                        {
                            //�÷� ��
                            if (MColorUtility.IsAcolr_Contain_BColor(units[i].unitColor, cur_Unit.unitColor))
                                return true;
                            else 
                                return false;
                        }
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// ��ǲ���⿡ �ִ� Ÿ�ϵ��� �����͸� �޾ƿɴϴ�.
        /// Ÿ���� ���� ���� �˾Ƽ� �ɷ����ϴ�.
        /// </summary>
        /// <param name="inputDir"></param>
        /// <returns></returns>
        public static TestTile[] GetTile_throughInputDir(TestTile from, bool[] input_Data)
        {
            List<TestTile> tiles = new List<TestTile>();
            var neighbors = GetNeighborTile(from);
            for (int i = 0; i < input_Data.Length; i++)
            {
                if(input_Data[i] && neighbors[i] != null)
                {
                    tiles.Add(neighbors[i]);
                }
            }
            return tiles.ToArray();
        }
    }
}