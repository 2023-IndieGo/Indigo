using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임플레이어가 실제 배틀을 위한 카드를 배치시키는 영역에 대한 데이터
/// </summary>
public class Field : BaseModel
{
    public Zone firstZone;

    public class Zone : BaseModel
    {
        public Zone nextZone;
    }
}
