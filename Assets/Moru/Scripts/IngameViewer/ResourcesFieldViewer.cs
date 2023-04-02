using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using Moru;
using Sirenix.OdinInspector;

public class ResourcesFieldViewer : Viewer<ResourcesFiled>
{
    //�ڿ��� �ܷ��� ���� �÷� ������Ʈ
    //������ �ڿ��� ĳ�� �𵨾�����Ʈ ��������Ʈ �޼��� ����

    #region Field
    [SerializeField, LabelText("���� ������"), TitleGroup("������")]
    private ResourcesFiled fieldData;
    public ResourcesFiled FieldData => fieldData;

    [SerializeField, TitleGroup("�ٹ̱�"), LabelText("�ܿ��� ǥ�� �÷�")] private MeshRenderer remind_Obj;
    [SerializeField, LabelText("��ƼŬ")] private ParticleSystem particle;

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
