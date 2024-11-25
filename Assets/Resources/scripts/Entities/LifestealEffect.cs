using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="lifesteal", menuName="Effects/lifesteal")]
public class LifestealEffect : MinionEffect
{
    public int healAmount;//�񕜗�
    //public GameObject healEffectPrefab;

    public override void ApplyEffect(CardController source, CardController target)
    {
        Debug.Log($"{source.model.getCardName()} heals for {healAmount} HP!");

        target.model.hp += healAmount; // ���q�[���[����

        //�n�[�g�A�j���[�V����
        HeartAnimation animManager = target.GetComponent<HeartAnimation>();
        if (animManager != null)
        {
            animManager.PlayHealingAnimation(target.transform,healAmount);
        }


        //GameObject healEffect = Instantiate(healEffectPrefab, target.transform.position, Quaternion.identity);
        //Destroy(healEffect, 2f); // 2�b��ɃG�t�F�N�g������

        target.view.Show(target.model);


        //�A�j���[�V������������
    }





}
