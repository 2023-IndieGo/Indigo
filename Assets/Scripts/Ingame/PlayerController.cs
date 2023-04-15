using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    

    //UI기반으로 동작할 경우 해당스크립트 통채로 필요없을 것으로 예상됨
    //참고로 UI기반은 드로우콜이 좀 있을 수 있음
    void Update()
    {
        // 마우스 위치를 기준으로 레이 생성
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


        // 레이캐스팅을 수행하고, 충돌한 객체 정보를 hit 변수에 저장
        if (Physics.Raycast(ray, out hit))
        {
            // 충돌한 객체의 정보 출력
            Debug.Log("Hit object: " + hit.transform.name);
            if (hit.collider.TryGetComponent<Viewer>(out var comp))
            {
                
            }
        }
    }
}