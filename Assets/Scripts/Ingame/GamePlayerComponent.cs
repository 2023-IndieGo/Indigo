using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public partial class GamePlayer
{
    public Deck deckData;
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
                cards.Remove(card);
                GameManager.instance.events.about_Deck.OnDrawCard_From_Deck?.Invoke(card);
                card.OnDrawing();
                owner.hand.GetCard(card);
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

        public void GetCard(Card card)
        {
            cards.Add(card);
            card.unitWhere = UnitWhere.Deck;
            //UI변화같은거 이벤트로 추가해야 할 수 있음
        }
    }

    public Hand hand;
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
            GameManager.instance.events.about_Hand.OnGetCard_To_Hand?.Invoke(card);
            card.unitWhere = UnitWhere.Hand;
        }

        /// <summary>
        /// 핸드에 있는 카드가 사라집니다. (버리는 것이 아닙니다.)
        /// </summary>
        /// <param name="card"></param>
        public void BlowCard(Card card)
        {
           if(cards.Contains(card))
            {
                cards.Remove(card);
                GameManager.instance.events.about_Hand.OnBlowCard_From_Hand?.Invoke(card);
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
                GameManager.instance.events.about_Hand.OnThrowCard_From_Hand?.Invoke(card);
                
                owner.trash.GetCard_FromHand(card);
            }
        }
    }

    public TrashCan trash;
    /// <summary>
    /// 게임플레이어가 사용을 마치거나 들고 있는 상태에서 버려진 카드뭉치
    /// </summary>
    public class TrashCan //베이스모델을 상속할 것인지는 차후 기획 보고
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
        public void GetCard_FromHand(Card card)
        {
            cards.Add(card);
            card.OnThrowAway();
            card.unitWhere = UnitWhere.Trash;
            GameManager.instance.events.about_Trash.OnGetCard?.Invoke(card);
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
            }
            GameManager.instance.events.about_Trash.OnClear_TrashCan?.Invoke();
        }
    }


    public Field field;
    public Character character;
}