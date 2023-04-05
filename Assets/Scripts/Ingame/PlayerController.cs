using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    

    //UI������� ������ ��� �ش罺ũ��Ʈ ��ä�� �ʿ���� ������ �����
    //����� UI����� ��ο����� �� ���� �� ����
    void Update()
    {
        // ���콺 ��ġ�� �������� ���� ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Input.GetMouseButtonDown(0))
        {
            
        }

        if(Input.GetMouseButton(0))
        {

        }


        if(Input.GetMouseButtonUp(0))
        {

        }


        if(!Input.GetMouseButton(0))
        {

        }


        // ����ĳ������ �����ϰ�, �浹�� ��ü ������ hit ������ ����
        if (Physics.Raycast(ray, out hit))
        {
            // �浹�� ��ü�� ���� ���
            Debug.Log("Hit object: " + hit.transform.name);
            if (hit.collider.TryGetComponent<Viewer>(out var comp))
            {
                
            }
        }
    }
}