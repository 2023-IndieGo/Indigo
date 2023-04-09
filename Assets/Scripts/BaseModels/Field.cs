using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임플레이어가 실제 배틀을 위한 카드를 배치시키는 영역에 대한 데이터
/// </summary>
public class Field : BaseModel
{

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
    private GamePlayer owner;

    public class NOde
    {
        Card card;
        Field.Zone myZone;

        void dd()
        {
            
        }
    }


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
            { Debug.Log($"이미 {currentCard}가 있습니다. 배치 불가"); }
            else
            {
                currentCard = card;
                currentCard.unitWhere = UnitWhere.Field;

                GameManager.instance.events.about_Field.OnLocatedCard_At_Zone?.Invoke(card, this);
                Debug.Log($"{card}를 {this}에 배치했습니다.");
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
