using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;
using Sirenix.OdinInspector;

public class UnitViewer : Viewer<Unit>
{
    #region Field
    [SerializeField, LabelText("���ֵ�����")]
    private Unit unitData;
    public Unit UnitData => unitData;

    [SerializeField, LabelText("��ǲ���� ���"), TitleGroup("��� ������Ʈ")] 
    private GameObject direction_Obj;

    #endregion

    #region Events
    
    private OnValueChange_Params<Unit> onUnitChange;

    #endregion



    #region Private Methods

    private void OnEnable()
    {
        onUnitChange += OnUnitChange;
    }

    private void OnDisable()
    {
        onUnitChange -= OnUnitChange;
    }

    public override void Init(Unit model)
    {
        base.Init(model);
        unitData = model;

        //���� ��ġ ����
        transform.position = unitData.Cur_Tile.curPosition;
        transform.position = new Vector3(unitData.Cur_Tile.curPosition.x, 1, unitData.Cur_Tile.curPosition.z);
        //Debug.Log($"���� ��ġ : {transform.position}");

        //���� ����
        float angle = MHexagonUtility.GetDirAngle(unitData.Input_Dir);
        direction_Obj.transform.eulerAngles = new Vector3(0, angle, 0);

        //���� �÷� ����
        var mat = direction_Obj.GetComponent<MeshRenderer>().materials;
        Color color = MColorUtility.GetColor(unitData.unitColor);
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i].color = color;
        }
        direction_Obj.GetComponent<MeshRenderer>().materials = mat;
    }

    private void UpdateViewModel()
    {
        //....Debug....//
        Debug.Log($"RGB ��� :{DebugUtillity.ArrayToString(unitData.RGBValue)}//{unitData.unitColor}\n" +
            $"���� : {DebugUtillity.ArrayToString(unitData.Input_Dir)}");
    }

    private void OnUnitChange(Unit current, Unit next)
    {
        if (current != null)
        {
            current.OnColorChange -= UpdateViewModel;
        }
        if (next != null)
        {
            next.OnColorChange += UpdateViewModel;
        }
        UpdateViewModel();
    }



    //.....TestCode.....//
    private void Start()
    {
        ChangeTest();
    }
    public void ChangeTest()
    {
        Unit current = unitData;
        //unitData = new Unit(eRGB.Blue, new bool[6] { false, false, true, true, true, false});
        if (onUnitChange != null) onUnitChange.Invoke(current, unitData);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;

        if (unitData.Cur_Tile == null) return;
        var tiles = MHexagonUtility.GetNeighborTile(unitData.Cur_Tile);
        foreach(var t in tiles)
        {
            if(t != null)
            Gizmos.DrawSphere(t.curPosition, 1);
        }
    }
    #endregion
}