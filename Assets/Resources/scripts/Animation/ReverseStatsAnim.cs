using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Quaternion endRotation= startRotation * Quaternion.Euler(0, 0, 180);//180‚Ç‰ñ“]


        while (elapsedTime < ReverseDuration) {

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / ReverseDuration;


            pivot.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);


            HpText.rotation = Quaternion.identity;
            AtText.rotation = Quaternion.identity;




            yield return null;
        }


        //Œ³‚ÌˆÊ’u‚É–ß‚·
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
