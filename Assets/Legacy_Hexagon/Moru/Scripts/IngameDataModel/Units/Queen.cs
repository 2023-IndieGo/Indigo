using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lagacy_Hexagon.Moru;

namespace Lagacy_Hexagon
{
    namespace Moru
    {
        public class Queen : Unit
        {


            public Queen(Tile targetTile) : base()
            {
                rgbValue = new bool[3] { true, true, true };
                //unitColor = eRGB.White;
                input_Dir = new bool[6] { true, true, true, true, true, true };
                unitName = "여왕";
                cur_Tile = targetTile;

                //Test성격 강함
                //GameObject obj = MonoBehaviour.Instantiate(TsetMono.instance.testUnitPrefap);
                //if (obj.TryGetComponent<UnitViewer>(out var comp))
                //{
                //    myUnitViewer = comp;
                //    comp.Init(this);
                //}
                //else
                //{
                //    myUnitViewer = obj.AddComponent<UnitViewer>();
                //    myUnitViewer.Init(this);
                //}

                Init();
            }

            public override void Init()
            {
                //base.Init();
                cur_Tile.PushUnit(this);
            }

            protected override void OnTurnStart()
            {
                //base.OnTurnStart();
            }


            protected override void OnTurnEnd()
            {
                base.OnTurnEnd();
            }


        }
    }
}