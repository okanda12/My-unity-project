using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Herodevices : MonoBehaviour
{
    //�f�o�C�X�̏�����
    public abstract void Initialize();

    //�^�[���̊J�n����
    public virtual void OnTurnStart() { }

    //�^�[���̏I������
    public virtual void OnTurnEnd() { }
}
