using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer: MonoBehaviour 
{
    protected BaseModel model;

    public virtual void Init(BaseModel model)
    {

    }
}

public class TestViewer : Viewer
{

}
