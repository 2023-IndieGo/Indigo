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
            if (dic_eRGB_To_bool == null)
            { Init(); }

            eRGB result = eRGB.Black;
            if (dic_eRGB_To_bool.ContainsValue(value))
            {
                for (int i = 0; i < dic_eRGB_To_bool.Count; i++)
                {
                    if (dic_eRGB_To_bool.TryGetValue((eRGB)i, out var _dic_Value))
                    {
                        if (_dic_Value == value)
                        {
                            result = (eRGB)i;
                            break;
                        }
                    }
                }
            }
            else
            {
                result = eRGB.Black;
            }

            return result;
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
        /// 최초 광산 생성 시 표시될 컬러값을 초기화합니다.
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

    }
}