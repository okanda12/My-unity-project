using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardDrawAnimation : MonoBehaviour
{
    private Transform PlayerDeckTransform;//デッキの位置
    private Transform PlayerHandTransform;//手札の位置
    private Transform EnemyDeckTransform;//デッキの位置
    private Transform EnemyHandTransform;//手札の位置
    public float drawDuration = 0.1f;//ドローアニメーションの時間

    private RectTransform rectTransform;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        PlayerDeckTransform = BattleManager.Instance.PlayerDeckTransform;
        PlayerHandTransform = BattleManager.Instance.PlayerHandTransform;
        EnemyDeckTransform = BattleManager.Instance.EnemyDeckTransform;
        EnemyHandTransform = BattleManager.Instance.EnemyHandTransform;
    }
    
    public void DrawCard(Transform hand)
    {
        StartCoroutine(DrawCardCoroutine(hand));
    
    }

    public IEnumerator Drawsetparent(CardDrawAnimation cardAnim,Transform hand)
    {
        //カードのドローアニメーションを実行
        cardAnim.DrawCard(hand);

        //アニメーションが完了するまで大樹
        yield return new WaitForSeconds(cardAnim.drawDuration);

        //yield return StartCoroutine();
        //アニメーション終了後に,カードの親を手札に変更
        cardAnim.transform.SetParent(hand, false);



       
    }

    public IEnumerator DrawCardCoroutine(Transform hand)
    {
        float elapsedTime = 0f;
        //カードの位置をデッキの位置に設定
        Vector3 startPosition = PlayerDeckTransform.position;
        Vector3 endPosition = PlayerHandTransform.position;

        if (hand == PlayerHandTransform)
        {
            startPosition = PlayerDeckTransform.position;
            endPosition = PlayerHandTransform.position;

        }
        else
        {
            startPosition = EnemyDeckTransform.position;
            endPosition = EnemyHandTransform.position;
        }


        

        Quaternion startRotation = Quaternion.Euler(0, 0, -45);
        Quaternion endRotation = Quaternion.identity;//これは無回転

        while (elapsedTime < drawDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / drawDuration;

            //位置と回転を補完
            rectTransform.position = Vector3.Lerp(startPosition, endPosition, t);
            rectTransform.rotation = Quaternion.Lerp(startRotation, endRotation, t);

            yield return null;//1フレーム停止させる.秒数指定するには yield return new wait forseconds
        }

        rectTransform.position = endPosition;
        rectTransform.rotation = endRotation;

        //Debug.Log("Card Draw Coroutine");




    }


}
