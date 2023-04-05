using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//예시 제안코드
public class Cmd
{
    public Cmd()
    {
        //생성과 동시에 CmdStack에 쌓이도록
        //Managers.gm.stackCmd.Push(this);
    }

    //gm.stackCmd.Pop().OnRewind()를 통해 뒤로가기를 했을 때
    //되돌리기를 가상메서드 선언
    public virtual void OnRewind()
    {

    }

}

public class UnitMoveCmd : Cmd
{
    //EX)
    private GameObject target;
    private GameObject current;

    public UnitMoveCmd(GameObject current ,GameObject target) : base()
    {
        //명령 수행
    }

    //뒤로가기 실행시 어떤 동작을 수행할 지 상세구현
    public override void OnRewind()
    {
        base.OnRewind();
        //target->current로 이동시킴.
    }

}
