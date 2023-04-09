using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class LobbyManager : MonoBehaviour
{
    [TitleGroup("UI오브젝트")]
    public GameObject CreatUI;
    public GameObject JoinUI;
    public GameObject RandomJoinUI;
    public GameObject HowToPlayUI;

    [TitleGroup("버튼들")]
    public Button CreateBtn;
    public Button JoinBtn;

    public void OnShowUI(GameObject target)
    {
        if(!target.activeInHierarchy)
        {
            target.SetActive(true);
        }
    }

    public void SetInit(bool isReady)
    {
        CreateBtn.interactable = isReady;
        JoinBtn.interactable = isReady;
    }

    public void OnExit()
    {
        if(Application.isEditor)
        {
            UnityEditor.EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }
        
    }

    public void BackAndCloseUI(GameObject target)
    {
        if(target.activeInHierarchy)
        {
            target.SetActive(false);
        }
    }

}
