using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GamePlayer
{
    /// <summary>
    /// 게임플레이어가 드로우할 카드뭉치
    /// </summary>
    public class Deck //베이스모델을 상속할 것인지는 차후 기획 보고
    {
        private List<Card> _cards = new List<Card>();
        public List<Card> cards
        { get => _cards; set => _cards = value;}
    }

    /// <summary>
    /// 게임플레이어가 현재 들고 있는 카드뭉치
    /// </summary>
    public class Hand
    {
        private Stack<Card> _cards = new Stack<Card>();
        public Stack<Card> cards
        { get => _cards; set => _cards = value; }
    }

    /// <summary>
    /// 게임플레이어가 사용을 마치거나 들고 있는 상태에서 버려진 카드뭉치
    /// </summary>
    public class TrashCan //베이스모델을 상속할 것인지는 차후 기획 보고
    {
        private Stack<Card> _cards = new Stack<Card>();
        public Stack<Card> cards
        { get => _cards; set => _cards = value; }
    }
}