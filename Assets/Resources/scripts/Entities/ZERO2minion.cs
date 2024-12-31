using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="minion",menuName ="ZERO2/minion")]

public class ZERO2minion : ScriptableObject, ICard
{
    public new string name;
    public int hp;
    public int at;
    public int cost;
    public Sprite icon;
    public string cardType = "Minion";

    public string cardText;
    public MinionEffect[] effects;

    public int GetCardID()
    {
        string fileName = this.name;//Unityファイルの元の名前を入れる
        string[] parts = fileName.Split(' ');//スペースで分割

        if (parts.Length > 1 && int.TryParse(parts[1], out int cardID))
        {
            return cardID;
        }
        Debug.LogError("Invalid card file name format. Expected format: 'magic X'");
        return -1; // エラー時は適切なIDを返す
    }

    public string getCardName()
    {
        return name;//nameフィールドを返す
    }

    public int Hp => hp;//magicにはないため
    public int At => at;  // 攻撃力もどうよう
    public int Cost => cost;  // コスト
    public Sprite Icon => icon;  // アイコン
    public string CardText => cardText;
    public string CardType => cardType;

    public MinionEffect[] minioneffect => effects;

}









