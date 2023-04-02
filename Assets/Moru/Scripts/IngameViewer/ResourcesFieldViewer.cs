using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using Moru;
using Sirenix.OdinInspector;

public class ResourcesFieldViewer : Viewer<ResourcesFiled>
{
    //자원의 잔량에 따른 컬러 업데이트
    //유닛이 자원을 캐면 모델업데이트 델리게이트 메서드 구현

    #region Field
    [SerializeField, LabelText("광산 데이터"), TitleGroup("데이터")]
    private ResourcesFiled fieldData;
    public ResourcesFiled FieldData => fieldData;

    [SerializeField, TitleGroup("꾸미기"), LabelText("잔여량 표시 컬러")] private MeshRenderer remind_Obj;
    [SerializeField, LabelText("파티클")] private ParticleSystem particle;

    #endregion


    #region Properties
    #endregion


    #region Events
    #endregion


    #region Constructor
    #endregion


    #region Public Methods
    public override void Init(ResourcesFiled model)
    {
        base.Init(model);
        fieldData = model;

        transform.position = fieldData.Cur_Tile.curPosition;
        transform.position = new Vector3(fieldData.Cur_Tile.curPosition.x, 1, fieldData.Cur_Tile.curPosition.z);
    }
    #endregion


    #region Private/Protected Methods

    #endregion

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position +new Vector3(0,1,0), 0.5f);
    }
}
