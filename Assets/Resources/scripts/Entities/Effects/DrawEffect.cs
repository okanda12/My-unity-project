using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="drawcard", menuName="Effects/drawcard")]
public class DrawEffect : MinionEffect
{
    public int Draw;//ドロー枚数

    public override void ApplyEffect(CardController source, CardController target)
    {

        Debug.Log($"draw effect!!");

        if (BattleManager.Instance.isPlayerTurn == false)//相手のターンなら
        {
            ICard enemyCardEntity = BattleManager.Instance.enemyDeck.DrawCard();

            if (enemyCardEntity != null)
            {
                BattleManager.Instance.CreateHand(BattleManager.Instance.EnemyHandTransform, enemyCardEntity);
            }
            else
            {
                Debug.Log("Enemy cant draw card!");
            }
            
        }
        else
        {
            ICard playerCardEntity = BattleManager.Instance.playerDeck.DrawCard();

            if (playerCardEntity != null)
            {
                BattleManager.Instance.CreateHand(BattleManager.Instance.PlayerHandTransform, playerCardEntity);
            }
            else
            {
                Debug.Log("you cant draw card!");
            }

            

        }
        




    }


}
