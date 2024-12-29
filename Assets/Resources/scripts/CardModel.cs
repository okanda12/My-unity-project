using UnityEngine;



//�J�[�h�̃f�[�^
public class CardModel
{
    public string name;
    public int hp;
    public int at;
    public int cost;
    public Sprite icon;
    public string cardType;

    public MinionEffect[] effects;

    public int maxhp;


    //�R���X�g���N�^ �쐬���ꂽ�Ƃ��Ɏ��s����鏉�����֐�
    public CardModel(ICard cardEntity,MinionEffect[] CardBattlecry)
    {
        //�X�y�[�X����

        name = cardEntity.getCardName();
        hp = cardEntity.Hp;
        at = cardEntity.At;
        cost = cardEntity.Cost;
        icon = cardEntity.Icon;

        cardType = cardEntity.CardType;

        effects = CardBattlecry;

        maxhp = hp;

    }

    public string getCardName()
    {
        return name;
    }

    public void BattleCry(CardController me, CardController target)
    {

        if (effects == null || effects.Length == 0) return;

        //�~�j�I���̍Z�̂̓K�p
        foreach (MinionEffect effect in effects)
        {
            effect.ApplyEffect(me, target);
        }



    }




}
