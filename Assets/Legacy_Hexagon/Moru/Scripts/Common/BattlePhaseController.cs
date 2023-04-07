using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투단계에서의 전투진행 및 전투결과를 관리하는 컴포넌트
/// </summary>
public class BattlePhaseController : MonoBehaviour
{
    //
    public void Init()
    {
        //플레이어 정보 받아옴
    }


    public void TryBattle(Card Attacker, Card Defender)
    {

    }

    private CardBattleResult GetBattleResult_Attaker()
    {
        return CardBattleResult.Success;
    }
}
