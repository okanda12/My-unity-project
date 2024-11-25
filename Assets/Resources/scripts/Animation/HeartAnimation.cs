using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class HeartAnimation : MonoBehaviour
{
    public GameObject heartPrefab; //ハートのプレファブ
    public int heartCount ;//飛ばすハートの数
    public float animationDuration ; //アニメーションの継続時間
    public float radius ;//ハートが飛ぶ半径

    public GameObject hearttextprefab;//ハート位置にぽわわーんてなるやつ
    public float hearttextDuration;//ぽわわーん時間

    //回復アニメーションであります

    public void PlayHealingAnimation(Transform targetTransform,int healAmount)
    {
        StartCoroutine(HealingAnimation(targetTransform,healAmount));
    }

    private IEnumerator HealingAnimation(Transform targetTransform, int healAmount)
    {
        
        float elapsedTime = 0f;
        Transform HeartPosition = transform.Find("Heart");//ハートの位置から飛ばす
        
        Vector3 startPosition = HeartPosition.position;
        Vector3 endPosition = startPosition + new Vector3(0, 0.1f, 0); 

        TextMeshProUGUI hearttext = hearttextprefab.GetComponentInChildren<TextMeshProUGUI>();
        //子オブジェクトから探す

        hearttext.text = healAmount.ToString();

        GameObject hearttextobject=Instantiate(hearttextprefab,HeartPosition,false);




        while (elapsedTime<hearttextDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / hearttextDuration;
            hearttextobject.transform.position = Vector3.Lerp(startPosition, endPosition, t * t);

            yield return null;
        }

        Destroy(hearttextobject);



        //GameObject heart1 = Instantiate(heartPrefab, targetTransform.position, Quaternion.identity,targetTransform);
        for (int i=0;i<heartCount;i++)
        {
            //ハートを生成
            //なんか最後の引数で親を設定しないとうまくいかないみたい
            GameObject heart = Instantiate(heartPrefab,targetTransform.position,Quaternion.identity, targetTransform);

            //ランダムな方向に飛ばす
            Vector2 randomDirection = Random.insideUnitCircle.normalized * radius;
            Vector3 targetPosition = targetTransform.position + new Vector3(randomDirection.x, randomDirection.y, 0);

            //ハートを動かす
            StartCoroutine(MoveHeart(heart.transform, targetPosition, animationDuration));

            yield return new WaitForSeconds(0.1f);
            //ちょっと待って次のハートを動かす
        
        }


        
    }


    //ハートひとつひとつに動きを与える
    private IEnumerator MoveHeart(Transform heart,Vector3 targetPosition,float duration)
    {
        Vector3 startPosition = heart.position;
        float elapsedTime = 0f;

        //Debug.Log($"Heart Position: {startPosition}, Target Position: {targetPosition}");



        while (elapsedTime<duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            //緩やかに動く
            heart.position = Vector3.Lerp(startPosition,targetPosition,t*t);
            //Debug.Log(heart.position);

            //Debug.Log($"radius: {Vector3.Distance(startPosition, heart.position)}");


            yield return null;
        }
        

        Destroy(heart.gameObject);


    }



    
}
