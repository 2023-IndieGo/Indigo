using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����÷��̾ ���� ��Ʋ�� ���� ī�带 ��ġ��Ű�� ������ ���� ������
/// </summary>
public class Field : BaseModel
{
    public Zone firstZone;

    public class Zone : BaseModel
    {
        public Zone nextZone;
    }
}
