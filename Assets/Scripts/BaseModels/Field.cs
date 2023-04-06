using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : BaseModel
{
    public Zone firstZone;

    public class Zone : BaseModel
    {
        public Zone nextZone;
    }
}
