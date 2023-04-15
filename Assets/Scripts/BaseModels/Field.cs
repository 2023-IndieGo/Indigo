using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// ?????????????? ???? ?????? ???? ?????? ?????????? ?????? ???? ??????
/// </summary>
public class Field : BaseModel
{
    [SerializeField]
    public GamePlayer owner;
    public Field(GamePlayer owner)
    {
        this.owner = owner;
        zones = new List<Zone[]>();
        for (int i = 0; i < 10; i++)
        {
            zones.Add(new Zone[2] { new Zone(i, ZoneType.Attack), new Zone(i, ZoneType.Defence) });
        }
    }
    [SerializeField]
    public List<Zone[]> zones;


    [System.Serializable]
    public class Zone : BaseModel
    {
        public int roundAress;
        public ZoneType zoneType;
        public Card currentCard;
        public Zone(int _roundAress, ZoneType zoneType)
        {
            roundAress = _roundAress;
            this.zoneType = zoneType;
        }

        public void LocatedCard(Card card)
        {
            if (currentCard != null)
            { Debug.Log($"내고자 하는 카드인자가 null입니다. {currentCard}"); }
            else
            {
                currentCard = card;
                currentCard.unitWhere = UnitWhere.Field;
                card.OnFieldDraw(this);
                GameManager.instance.events.about_Prepare.OnDrawField?.Invoke(card, this);
                Debug.Log($"{card}를 {this}에 배치하였습니다..");
            }
        }


        public void ThrowAwayCard()
        {
            if (currentCard != null)
            {
                currentCard.OnThrowAway();
                GameManager.instance.events.about_Prepare.OnThrowAwayFromField?.Invoke(currentCard);
                currentCard.unitWhere = UnitWhere.Trash;
                currentCard = null;
            }
        }
    }
}
