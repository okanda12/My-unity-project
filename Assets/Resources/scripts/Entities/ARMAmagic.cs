using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="magic",menuName="ARMA/magic")]
public class ARMAmagic:ScriptableObject,ICard
{
    public new string name;
    public int cost;
    public Sprite icon;
    public string cardType="Magic";
    public string cardText;

    public MinionEffect[] effects;

    public int GetCardID()
    {
        string fileName = this.name;//Unity�t�@�C���̌��̖��O������
        string[] parts = fileName.Split(' ');//�X�y�[�X�ŕ���

        if(parts.Length>1 && int.TryParse(parts[1], out int cardID))
        {
            return cardID;
        }
        Debug.LogError("Invalid card file name format. Expected format: 'magic X'");
        return -1; // �G���[���͓K�؂�ID��Ԃ�
    }

    public string getCardName()
    {
        return name;//name�t�B�[���h��Ԃ�
    }


    public int Hp => 0;//magic�ɂ͂Ȃ�����
    public int At => 0;  // �U���͂��ǂ��悤
    public int Cost => cost;  // �R�X�g
    public Sprite Icon => icon;  // �A�C�R��

    public string CardType => cardType;

    public string CardText => cardText;

    public MinionEffect[] minioneffect =>effects;

}


