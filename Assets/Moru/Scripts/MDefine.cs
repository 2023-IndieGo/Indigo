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
        /// eRGB의 값을 bool[]형으로 전환한 데이터값을 받아옵니다.
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
        /// bool[]의 값을 eRGB로 전환한 데이터값을 받아옵니다.
        /// </summary>
        /// <param name="value"></param>
        public static eRGB Generate_BoolArr_to_eRGB(bool[] value)
        {
            //생각했던 동작대로 안되서 일단 하드코딩
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
                Debug.Log($"{stringValue }가 Dic에 포함되어있지 않습니다.");
                return eRGB.Black;
            }

        }

        /// <summary>
        /// 컬러 디스플레이 표시의 단계의 수입니다.
        /// </summary>
        private const int colorThreshold_Level = 50;
        
        /// <summary>
        /// 해당값 이하로의 컬러rgb값이 떨어지지 않습니다.
        /// </summary>
        private const float minColorValue = 0.3f;
        /// <summary>
        /// 해당값 이상으로의 컬러rgb값이 올라가지 않습니다.
        /// </summary>
        private const float maxColorValue = 0.9f;

        /// <summary>
        /// 해당값 이하로부터의 절대값에 대한 컬러표기 계산이 적용됩니다.
        /// </summary>
        private const int maxResValue_Per_ColorThreshold = 100;

        /// <summary>
        /// 최초 광산 생성 시 표시될 컬러값을 초기화합니다. (절대값)
        /// </summary>
        /// <param name="rgbValue"></param>
        /// <returns></returns>
        public static float[] ColorRateInitializer(int[] rgbValue)
        {
            if(rgbValue == null)
            {
                Debug.Log("매개변수가 null입니다. 메서드 종료");
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
        /// A컬러가 B컬러를 포함하는지 비교한 값을 반환합니다.
        /// ex) Red는 Magenta를 포함하지 않지만 Magenta는 Red를 포함합니다.
        /// 현재는 동일컬러만 같다고 설정합니다.
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
                if(AValue[i])   //해당 배열의 색상을 요구할 때
                {
                    if(BValue[i]) //해당색상이 있을 경우
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }

            }
            //화이트면 무조건 가져오기
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
        /// 매개변수 int에 대한 방향을 육각타일에서의 360라디안 방향으로 반환합니다. 0=0, 1 = 60 ,...
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
        /// 매개변수 bool[]에 대한 방향을 육각타일에서의 360라디안 방향으로 반환합니다.
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
        /// 타일의 주변타일 6개를 받아옵니다.
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
            //홀수째 인덱스일 경우
            if (selectedTile.adressY % 2 != 1)
            {
                //위에께 0보다 클경우
                if (y - 1 >= 0)
                {
                    result[0] = TestMap.GetTile(x, y - 1);

                }
                //아래께 최대값보다 작을경우
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
            //짝수째 인덱스일 경우
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
        /// 2번째 매개변수 타일이 자신을 포함하는 타일인지 유닛의 Input방향을 통해서 판단합니다. 유닛이 없을 경우 항상 false
        /// </summary>
        /// <param name="myTile"></param>
        /// <param name="targetTile"></param>
        /// <returns></returns>
        public static bool IsNeighborTileContainMe_throughUnit(Unit cur_Unit, TestTile targetTile)
        {
            if (targetTile == null) return false;
            if (targetTile.Units.Count == 0) return false;

            Unit[] units = targetTile.Units.ToArray();
            //유닛들의 비교
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
                            //컬러 비교
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
        /// 인풋방향에 있는 타일들의 데이터를 받아옵니다.
        /// 타일이 없는 경우는 알아서 걸러냅니다.
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