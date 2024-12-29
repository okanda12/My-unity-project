using UnityEngine;



//カードのデータ
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


    //コンストラクタ 作成されたときに実行される初期化関数
    public CardModel(ICard cardEntity,MinionEffect[] CardBattlecry)
    {
        //スペース注意

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

        //ミニオンの校歌の適用
        foreach (MinionEffect effect in effects)
        {
            effect.ApplyEffect(me, target);
        }



    }




}
