using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Character : BaseModel
{

    #region Field
    public GamePlayer myGamePlayer { get; private set; }

    public string name;

    /*/
    public List<Card> specialCards;
    /*/
    #endregion


    #region Properties
    #endregion


    #region Events
    #endregion


    #region Constructor
    public Character(GamePlayer player)
    {
        this.myGamePlayer = player;
        passive = new Passive();
        ultimateSkill = new UltimateSkill();
    }
    #endregion


    #region Public Methods
    #endregion


    #region Private/Protected Methods
    #endregion




}

