using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 게임 시작시 들고 있을 카드 리스트
/// 변동되는 카드 리스트
/// </summary>
public class Deck : BaseModel
{
    private List<Card> _current_Deck;
    public List<Card> current_Deck => _current_Deck;




}
