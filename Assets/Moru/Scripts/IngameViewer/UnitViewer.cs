using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;

public class UnitViewer : MonoBehaviour
{
    #region Field

    private Unit unitData;
    public Unit UnitData => unitData;

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

    private void UpdateViewModel()
    {
        //....Debug....//
        Debug.Log($"RGB ¹ë·ù :{DebugUtillity.ArrayToString(unitData.RGBValue)}//{unitData.unitColor}\n" +
            $"¹æÇâ : {DebugUtillity.ArrayToString(unitData.Input_Dir)}");
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
        Unit units;
    }
    public void ChangeTest()
    {
        Unit current = unitData;
        unitData = new Unit(eRGB.Blue, new bool[6] { false, false, true, true, true, false});
        if (onUnitChange != null) onUnitChange.Invoke(current, unitData);
    }


    #endregion
}