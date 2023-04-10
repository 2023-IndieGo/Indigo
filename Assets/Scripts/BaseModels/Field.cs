using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ?????????????? ???? ?????? ???? ?????? ?????????? ?????? ???? ??????
/// </summary>
public class Field : BaseModel
{
    public GamePlayer owner;
    public Field(GamePlayer owner)
    {
        this.owner = owner;
        zones = new List<Zone[]>();
        for (int i = 0; i < 10; i++)
        {
            if (i == 0)
            {
                zones.Add(new Zone[2] { new Zone(i, ZoneType.Attack), null });
            }
            else
            {
                zones.Add(new Zone[2] { new Zone(i, ZoneType.Attack), new Zone(i, ZoneType.Defence) });
            }
        }
    }
    public List<Zone[]> zones;
    


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
            { Debug.Log($"???? {currentCard}?? ????????. ???? ????"); }
            else
            {
                currentCard = card;
                currentCard.unitWhere = UnitWhere.Field;
                card.OnFieldDraw(this);
                GameManager.instance.events.about_Field.OnLocatedCard_At_Zone?.Invoke(card, this);
                Debug.Log($"{card}?? {this}?? ????????????.");
            }
        }


        public void ThrowAwayCard()
        {
            if (currentCard != null)
            {
                currentCard.OnThrowAway();
                GameManager.instance.events.about_Field.OnRemoveCard_From_Zone?.Invoke(currentCard, this);
                currentCard.unitWhere = UnitWhere.Trash;
                currentCard = null;
            }
        }
    }
}
