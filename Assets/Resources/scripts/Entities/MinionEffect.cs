using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionEffect : ScriptableObject
{
    //���ʂ𔭓����钊�ۃ��\�b�h

    public abstract void ApplyEffect(CardController source,CardController target);



}
