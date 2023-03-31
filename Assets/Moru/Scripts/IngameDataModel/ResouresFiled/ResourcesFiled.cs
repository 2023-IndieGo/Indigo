using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;

namespace Moru
{
    public class ResourcesFiled
    {
        #region Field
        private int[] reserves_Resources = new int[3];
        /// <summary>
        /// 현재 남아있는 자원잔량을 의미합니다.
        /// </summary>
        public int[] Reserves_Resources => reserves_Resources;

        private int[] init_Reserves = new int[3];
        /// <summary>
        /// 최초 마인의 자원량입니다.
        /// </summary>
        private int[] Init_Reserves => init_Reserves;
        #endregion


        #region Properties
        #endregion


        #region Events
        /// <summary>
        /// 자원잔량의 변동 시 실행되는 델리게이트 메서드
        /// </summary>
        public OnValueChange_Params<int[]> onReservesChange;
        #endregion


        #region Constructor

        /// <summary>
        /// 자원광산의 최초 생성자로, 자원매장량을 결정합니다.
        /// 매개변수값이 null일 경우, 임의의 값으로 초기화됩니다.
        /// </summary>
        /// <param name="init_ReservesAmount"></param>
        public ResourcesFiled(int[] init_ReservesAmount = null)
        {

            if(init_ReservesAmount == null)
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

        }
        #endregion


        #region Public Methods
        /// <summary>
        /// 매개변수값의 컬러를 채취하는 것을 시도 후, 시도결과를 반환합니다. out resources는 null이 될 수 있습니다.
        /// </summary>
        /// <param name="targetResources"></param>
        public bool TryMining(eRGB targetResources, out Res resources)
        {
            if(targetResources == eRGB.Black)
            {
                resources = null;
                Debug.Log($"잘못된 자원캐기 입력값 매개변수 : {targetResources}. 메서드 종료");
                return false;
            }
            int[] current_Reserves = reserves_Resources;
            //채취 결과를 저장할 boolean 배열
            bool[] colorResult = new bool[3];
            //채취 시도할 타겟자원을 boolean arr로 변환
            bool[] targetRGB_Value = MColorUtility.Generate_eRGB_to_BoolArr(targetResources);

            //내부로직
            for (int i = 0; i < targetRGB_Value.Length; i++)
            {
                if(targetRGB_Value[i])
                {
                    if(reserves_Resources[i] >0)
                    {
                        reserves_Resources[i]--;
                        colorResult[i] = true;
                    }
                    else
                    {
                        //자원이 모자람
                        //일단 모자라면 모자란것 제외하고 캐도록 설정
                    }
                }
            }

            eRGB enumTypeRGB_Result = MColorUtility.Generate_BoolArr_to_eRGB(colorResult);

            //resources가 black이면 (자원을 아예 채취를 못했으면)
            if(enumTypeRGB_Result == eRGB.Black)
            {
                resources = null;
                Debug.Log($"자원을 캐는데에 실패하였음");
                return false;
            }

            //생성 후 out으로 넘겨주고 캔 유닛이 알아서 처리하도록 설정,
            resources = new Res(enumTypeRGB_Result);

            onReservesChange?.Invoke(current_Reserves, reserves_Resources);
            return true;
        }

        /// <summary>
        /// 매개변수값의 컬러를 채취하는 것을 시도 후, 시도결과를 반환합니다.
        /// 특정컬러를 원하는 만큼 캘 수 있습니다. out resources는 0이 될 수 있습니다.
        /// </summary>
        /// <param name="targetResources"></param>
        public bool TryMining(eRGB targetResources, int amount, out Res[] resources)
        {
            List<Res> _res = new List<Res>();
            for (int i = 0; i < amount; i++)
            {
                if(TryMining(targetResources, out var result))
                {
                    _res.Add(result);
                }
            }
            resources = _res.ToArray();
            if(resources.Length == 0)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// ..현재 미구현..
        /// 매개변수값의 컬러배열을 채취하는 것을 시도 후, 시도결과를 반환합니다.
        /// 한번에 원하는 만큼 캘 수 있습니다.
        /// </summary>
        /// <param name="targetResources"></param>
        public bool TryMining(int[] targetResources, out Res[] resources)
        {

            resources = new Res[0];
            return true;
        }

        /// <summary>
        /// 자원매장량을 0으로 만듭니다.
        /// </summary>
        public void SetClear()
        {
            int[] current_Reserves = reserves_Resources;
            reserves_Resources = new int[3] { 0, 0, 0 };
            onReservesChange?.Invoke(current_Reserves, reserves_Resources);
        }

        /// <summary>
        /// 자원매장량을 매개변수값으로 초기화합니다. amount 배열 매개변수를 null로 전달할 경우
        /// 자동으로 최초 생성시의 자원매장량으로 초기화합니다.
        /// </summary>
        /// <param name="amount"></param>
        public void SetReserves_Amount(int[] amount = null)
        {
            int[] current_Reserves = reserves_Resources;
            if(amount == null)
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