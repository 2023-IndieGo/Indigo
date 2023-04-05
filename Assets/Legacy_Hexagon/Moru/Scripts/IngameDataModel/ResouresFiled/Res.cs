using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lagacy_Hexagon.Moru;
using Sirenix.OdinInspector;


namespace Lagacy_Hexagon
{
    namespace Moru
    {
        public class Res
        {
            #region Field
            [LabelText("자원 컬러")]
            public eRGB colorType;
            #endregion


            #region Properties
            #endregion


            #region Events
            #endregion


            #region Constructor
            public Res(eRGB colorType, Tile targetTile)
            {
                this.colorType = colorType;
            }


            #endregion


            #region Public Methods
            #endregion


            #region Private/Protected Methods
            #endregion
        }
    }
}