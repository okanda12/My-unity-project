using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    int GetCardID();
    string getCardName();


    //カードのプロパティ
    int Hp { get; }//読み取り専用プロパティの書き方
    int At { get; }

    int Cost { get; }
    Sprite Icon { get; }

    string CardText { get; }

    string CardType { get; }

    MinionEffect[] minioneffect {get;}



}
