using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//ARMA�f�o�C�X�̍U��𔽓]���铮��ł��D

public class ReverseStatsAnim : MonoBehaviour
{

    [SerializeField] private Transform HpText;
    [SerializeField] private Transform AtText;
    [SerializeField] private Transform pivot;
    public float ReverseDuration;

    

    public IEnumerator ReverseStats()
    {


        CardController cardCon = this.GetComponent<CardController>();

        

        float elapsedTime = 0f;

        Quaternion startRotation = pivot.rotation;
        Quaternion endRotation= startRotation * Quaternion.Euler(0, 0, 180);//180�ǉ�]


        while (elapsedTime < ReverseDuration) {

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / ReverseDuration;


            pivot.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);


            HpText.rotation = Quaternion.identity;
            AtText.rotation = Quaternion.identity;


            if (this == null)
            {
                break;
            }

            yield return null;
        }


        //���̈ʒu�ɖ߂�
        pivot.transform.rotation = startRotation;
        HpText.rotation = Quaternion.identity;
        AtText.rotation = Quaternion.identity;

        if (cardCon != null)
        {
            int originalAttack = cardCon.model.at;
            cardCon.model.at = cardCon.model.hp;
            cardCon.model.hp = originalAttack;

            cardCon.view.Show(cardCon.model);

        }



    }





}
