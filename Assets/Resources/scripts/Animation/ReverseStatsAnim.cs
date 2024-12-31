using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//ARMAデバイスの攻守を反転する動作です．

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
        Quaternion endRotation= startRotation * Quaternion.Euler(0, 0, 180);//180ど回転


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


        //元の位置に戻す
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
