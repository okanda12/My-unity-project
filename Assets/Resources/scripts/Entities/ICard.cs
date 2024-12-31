using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    int GetCardID();
    string getCardName();


    //�J�[�h�̃v���p�e�B
    int Hp { get; }//�ǂݎ���p�v���p�e�B�̏�����
    int At { get; }

    int Cost { get; }
    Sprite Icon { get; }

    string CardText { get; }

    string CardType { get; }

    MinionEffect[] minioneffect {get;}



}
