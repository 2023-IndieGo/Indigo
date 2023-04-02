using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;

public class ResourceViewer : Viewer<Res>
{
    #region Field
    private Res resData;
    public Res ResData => resData;
    #endregion


    #region Properties
    #endregion


    #region Events
    #endregion


    #region Constructor
    #endregion


    #region Public Methods
    public override void Init(Res model)
    {
        base.Init(model);
        resData = model;
    }
    #endregion


    #region Private/Protected Methods
    #endregion
}
