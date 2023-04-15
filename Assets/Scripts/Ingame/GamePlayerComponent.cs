using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public partial class GamePlayer
{
    [SerializeField]
    public Deck deckData;
    [System.Serializable]
    /// <summary>
    /// 게임플레이어가 드로우할 카드뭉치
    /// </summary>
    public class Deck //베이스모델을 상속할 것인지는 차후 기획 보고
    {
        public Deck(GamePlayer owner)
        {
            this.owner = owner;
        }
        private GamePlayer owner;
        [ShowInInspector, LabelText("쌓여있는 카드리스트")]
        private List<Card> _cards = new List<Card>();
        public List<Card> cards
        { get => _cards; set => _cards = value;}
        
        /// <summary>
        /// 특정카드를 뽑아 핸드에 넣습니다.
        /// 리스트에 없는 걸 뽑을 경우 카드를 뽑지 못합니다.
        /// </summary>
        /// <param name="card"></param>
        public void DrawCard_ToHand(Card card)
        {
            if(cards.Contains(card))
            {
                //카드뭉치 리스트에서 카드 제거
                cards.Remove(card);
                //핸드에 애딩
                owner.hand.GetCard(card);
                //구독자에게 알리기
                GameManager.instance.events.about_Prepare.OnDrawCard_From_Deck?.Invoke(card);
            }
            else
            {
                Debug.Log($"{card.name}이 현재 덱이 없습니다.");
            }
        }

        /// <summary>
        /// 덱에서 무작위 카드를 뽑아 핸드에 넣습니다.
        /// </summary>
        public void DrawRandomCard_ToHand()
        {
            int randomValue = Random.Range(0, cards.Count);
            var drawedCard = cards[randomValue];
            DrawCard_ToHand(drawedCard);
        }


        /// <summary>
        /// 덱이 카드를 얻습니다. 이벤트를 따로 실행하지 않습니다.
        /// </summary>
        /// <param name="card"></param>
        public void GetCard(Card card)
        {
            cards.Add(card);
            card.unitWhere = UnitWhere.Deck;
        }
    }

    [SerializeField]
    public Hand hand;
    [System.Serializable]
    /// <summary>
    /// 게임플레이어가 현재 들고 있는 카드뭉치
    /// </summary>
    public class Hand
    {
        public Hand(GamePlayer owner)
        {
            this.owner = owner;
        }
        private GamePlayer owner;
        private List<Card> _cards = new List<Card>();
        public List<Card> cards
        { get => _cards; set => _cards = value; }

        /// <summary>
        /// 핸드에 카드를 얻습니다.
        /// </summary>
        /// <param name="card"></param>
        public void GetCard(Card card)
        {
            cards.Add(card);
            card.unitWhere = UnitWhere.Hand;

        }

        /// <summary>
        /// 매개변수값의 핸드에 있는 카드를 없앱니다. (버리는 것이 아닙니다.)
        /// </summary>
        /// <param name="card"></param>
        public void BlowCard(Card card)
        {
           if(cards.Contains(card))
            {
                cards.Remove(card);
                GameManager.instance.events.about_Prepare.OnCardOutofTheGame?.Invoke(card);
            }
           else
            {
                Debug.Log($"{card}가 흘레이어 핸드에 포함되어있지 않습니다.");
            }

        }

        /// <summary>
        /// 카드를 쓰레기통으로 버립니다.
        /// </summary>
        /// <param name="card"></param>
        public void ThrowCard(Card card)
        {
            if(cards.Contains(card))
            {
                cards.Remove(card);
                owner.trash.GetCard(card);
                GameManager.instance.events.about_Prepare.OnThrowAwayFromHand?.Invoke(card);
                
            }
        }
    }

    [SerializeField]
    public TrashCan trash;
    [System.Serializable]
    /// <summary>
    /// 게임플레이어가 사용을 마치거나 들고 있는 상태에서 버려진 카드뭉치
    /// </summary>
    public class TrashCan
    {
        public TrashCan(GamePlayer owner)
        {
            this.owner = owner;
        }
        private GamePlayer owner;
        private List<Card> _cards = new List<Card>();
        public List<Card> cards
        { get => _cards; set => _cards = value; }

        /// <summary>
        /// 매개변수의 카드를 쓰레기통으로 가져옵니다.
        /// </summary>
        /// <param name="card"></param>
        public void GetCard(Card card)
        {
            cards.Add(card);
            card.OnThrowAway();
            card.unitWhere = UnitWhere.Trash;
        }

        /// <summary>
        /// 쓰레기통에 쌓인 모든 카드를 덱으로 다시 집어넣습니다.
        /// </summary>
        public void ClearAll()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                owner.deckData.cards.Add(cards[i]);
                cards[i].unitWhere = UnitWhere.Deck;
                GameManager.instance.events.about_Prepare.OnGetCardToDeck_FromTrash?.Invoke(cards[i]);
            }
            cards.Clear();
        }
    }


    //버프/디버프 아우라 관리용 필드 레퍼런스 필요
    //중요한 점 : 버프와 디버프는 각각 따로 별도로, 중첩될수도, 중첩안될수도
    //각각의 버프,디버프는 별도의 클래스로 관리되어야 함
    //그리고 그에 걸맞는 이벤트가 또 따로 필요함
    [SerializeField] public Dictionary<Buff, List<Buff>> BuffList;
    [SerializeField] public Dictionary<Debuff, List<Debuff>> DebuffList;

    [SerializeField] public Field field;
    [SerializeField] public Character character;
}