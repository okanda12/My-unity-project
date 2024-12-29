using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName="RestoreCost",menuName="Effects/RestoreCost")]

public class RestoreCost : MinionEffect
{

    public int RestoreAmount;//‰ñ•œƒ}ƒi—Ê
                             // Start is called before the first frame update

    public override void ApplyEffect(CardController source, CardController target)
    {
        Debug.Log($"{source.model.getCardName()} Restores {RestoreAmount}mana ");



        BattleManager.Instance.manasys.AddEmptyMana(RestoreAmount);


    }


 }
