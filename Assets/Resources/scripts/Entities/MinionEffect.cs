using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionEffect : ScriptableObject
{
    //効果を発動する抽象メソッド

    public abstract void ApplyEffect(CardController source,CardController target);



}
