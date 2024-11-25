using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="lifesteal", menuName="Effects/lifesteal")]
public class LifestealEffect : MinionEffect
{
    public int healAmount;//回復量
    //public GameObject healEffectPrefab;

    public override void ApplyEffect(CardController source, CardController target)
    {
        Debug.Log($"{source.model.getCardName()} heals for {healAmount} HP!");

        target.model.hp += healAmount; // 自ヒーローを回復

        //ハートアニメーション
        HeartAnimation animManager = target.GetComponent<HeartAnimation>();
        if (animManager != null)
        {
            animManager.PlayHealingAnimation(target.transform,healAmount);
        }


        //GameObject healEffect = Instantiate(healEffectPrefab, target.transform.position, Quaternion.identity);
        //Destroy(healEffect, 2f); // 2秒後にエフェクトを消去

        target.view.Show(target.model);


        //アニメーションもここに
    }





}
