using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����ܰ迡���� �������� �� ��������� �����ϴ� ������Ʈ
/// </summary>
public class BattlePhaseController : MonoBehaviour
{
    //
    public void Init()
    {
        //�÷��̾� ���� �޾ƿ�
    }


    public void TryBattle(Card Attacker, Card Defender)
    {

    }

    private CardBattleResult GetBattleResult_Attaker()
    {
        return CardBattleResult.Success;
    }
}
