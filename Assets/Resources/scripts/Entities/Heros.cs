using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Heros",menuName ="HeroEntity")]
public class Heros : ScriptableObject,ICard
{
    public new string name;
    public int hp;
    public int at;
    public Sprite icon;
    public string cardType = "Hero";
    public string cardText;
    public MinionEffect[] effects;


    public int GetCardID()
    {

        string fileName = this.name;//Unity�t�@�C���̌��̖��O������
        string[] parts = fileName.Split(' ');
        //Tryparse�@�����ɕϊ��ł��邩
        if (parts.Length > 1 && int.TryParse(parts[1],out int cardID ))
        {
            return cardID;
        }
        return -1;//�G���[���͓K�؂�ID��Ԃ�

    }

    public string getCardName()
    {
        return name;//name�t�B�[���h��Ԃ�
    }


    public int Hp => hp;

    public int At => at;

    public int Cost => 0;
    public Sprite Icon => icon;
    public string CardText => cardText;
    public string CardType => cardType;


    public MinionEffect[] minioneffect => effects;






}