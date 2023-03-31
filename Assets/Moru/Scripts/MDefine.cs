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
        /// ���� ���� ���� �� ǥ�õ� �÷����� �ʱ�ȭ�մϴ�.
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