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
                result += arr[i].ToString();
                result += "\n";
            }
            result += "----Array End----\n";
            return result;
        }
        public static string ListToString<T>(T[] arr)
        {
            return ";";
        }
    }

    
}