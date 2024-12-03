using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAIKAdevice : Herodevices
{

    [SerializeField] private int rewardCost ;//10ターン経過したらわたそうかな～
    [SerializeField] private int currentTurn = 0;//現在までの経過ターン
    [SerializeField] private Transform moontransform;//回転するオブジェクト
    [SerializeField] private Transform EnemyFieldTransform;
    [SerializeField] private Transform PlayerFieldTransform;

    private bool Activated=false;//これがtrueになると相手の場も変えてしまう

    public float rotationDuration;


    public override void Initialize()
    {
        currentTurn = 0;
    }


    //ここにパラメタを全て変えるプログラムがほしい　

    public override void OnTurnStart() 
    {

        currentTurn += 1;
        StartCoroutine(TurnMoon());

        if (Activated == false)
        {
            //これ敵がARMAだったら逆になるよね？
            ReverseStatsinField(EnemyFieldTransform);

        }
        else
        {   //activatedをtrueにする処理も必要
            ReverseStatsinField(PlayerFieldTransform);
            ReverseStatsinField(EnemyFieldTransform);

        }

    }


    public void ReverseStatsinField(Transform field)
    {
        //これいいね！！！


        foreach (Transform daiza in field)
        {

            //getcomponetinchildrenは最初の一つしか持ってこないのでリストにして
            ReverseStatsAnim[] reverseAnims = daiza.GetComponentsInChildren<ReverseStatsAnim>();
            foreach (ReverseStatsAnim reverseAnim in reverseAnims)
            {
                StartCoroutine(reverseAnim.ReverseStats());
            }
            


        }

        /*
            foreach (Transform cardTransform in  field)
            {
                CardController cardController = cardTransform.GetComponent<CardController>();
                if (cardController != null)
                {
                    int originalAttack = cardController.model.at;
                    cardController.model.at = cardController.model.hp;
                    cardController.model.hp = originalAttack;

                cardController.view.Show(cardController.model);
                
                }



            }
        */

    }


    //子オブジェクトの月が回転します
    private IEnumerator TurnMoon()
    {
        Debug.Log($"TURN MOOOn");
        float elapsedTime = 0f;
        Quaternion ini = moontransform.rotation;
        Quaternion end = Quaternion.Euler(0,0, currentTurn*180);
        Quaternion endplus = Quaternion.Euler(0, 0,( currentTurn * 180)+10);


        
        
        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotationDuration;



            moontransform.rotation = Quaternion.Lerp(ini, end, t);


            yield return null;
        }


        /*
        //Debug.Log($"currentTurn{currentTurn},ini{ini}, end{end}");

        float subrotationDuration = rotationDuration - (1f/3f)*rotationDuration;
        Debug.Log($"subrotationDuration{subrotationDuration}");

        while (elapsedTime < subrotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / subrotationDuration;



            moontransform.rotation=Quaternion.Lerp(ini,endplus,t);


            yield return null;
        }

        ini = moontransform.rotation;
        elapsedTime = 0f;

        //Debug.Log($"elapsedTime{elapsedTime}, subrotation{subrotationDuration}  rotation{rotationDuration},");
        //ガコッモーション
        while (elapsedTime + subrotationDuration  < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (rotationDuration-subrotationDuration);

            Debug.Log(t);

            moontransform.rotation = Quaternion.Lerp(ini, end, t);



            yield return null;
        }
        */

    }
}
