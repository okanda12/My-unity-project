using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAnimation : MonoBehaviour
{
    public GameObject heartPrefab; //ハートのプレファブ
    public int heartCount ;//飛ばすハートの数
    public float animationDuration ; //アニメーションの継続時間
    public float radius ;//ハートが飛ぶ半径

    //回復アニメーションであります

    public void PlayHealingAnimation(Transform targetTransform)
    {
        StartCoroutine(HealingAnimation(targetTransform));
    }

    private IEnumerator HealingAnimation(Transform targetTransform)
    {

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
        //最後のradiusを出力
        ;


        Destroy(heart.gameObject);


    }



    
}
