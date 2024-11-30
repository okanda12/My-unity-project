using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Herodevices : MonoBehaviour
{
    //デバイスの初期化
    public abstract void Initialize();

    //ターンの開始処理
    public virtual void OnTurnStart() { }

    //ターンの終了処理
    public virtual void OnTurnEnd() { }
}
