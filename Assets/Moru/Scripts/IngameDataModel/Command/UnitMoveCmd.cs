using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���� �����ڵ�
public class Cmd
{
    public Cmd()
    {
        //������ ���ÿ� CmdStack�� ���̵���
        //Managers.gm.stackCmd.Push(this);
    }

    //gm.stackCmd.Pop().OnRewind()�� ���� �ڷΰ��⸦ ���� ��
    //�ǵ����⸦ ����޼��� ����
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
        //��� ����
    }

    //�ڷΰ��� ����� � ������ ������ �� �󼼱���
    public override void OnRewind()
    {
        base.OnRewind();
        //target->current�� �̵���Ŵ.
    }

}
