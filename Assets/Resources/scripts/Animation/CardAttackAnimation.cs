using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardAttackAnimation : MonoBehaviour
{
    
    //public Transform targetTransform;//ターゲット

    public float AttackDurationGO;//行きの時間
    public float AttackDurationBack;//戻りの時間

    public float CantAttackDuration;//攻撃できない

    private Transform rectTransform;


    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();

    }

   


    //}


    //カードに動きをつけるため,Horizontal Layoutを一瞬だけ無効にします.
    public void IgnoreLayout(Transform from,  bool ignore)
    {
        LayoutElement layoutElement = from.GetComponent<LayoutElement>();

        if (layoutElement != null)//あるなら
        {
            layoutElement.ignoreLayout = ignore;//レイアウトを無視するか設定
        }
    }

    //攻撃できないとき,ブルブル震えます.
    public IEnumerator CantAttack(Transform from)
    {
        float elapsedTime = 0f;

        //IgnoreLayout(from,true);まぁholizontalは回転を制約しないのでつけなくてもいっか


        Quaternion startRotation = from.GetComponent<Transform>().localRotation;



        while (elapsedTime < CantAttackDuration)
        {

            
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / CantAttackDuration;
            float rotationOffset = 4* Mathf.Sin(t);
            //位置と回転を補完
            //Debug.Log(rotationOffset);

            rectTransform.rotation = Quaternion.Euler(0, 0,rotationOffset);


            yield return null;

        }

        rectTransform.rotation = startRotation;




        //IgnoreLayout(from, false);

        yield return new WaitForSeconds(CantAttackDuration);




    }



    //攻撃モーションです.自分の位置(from)から相手の位置(target)へ行って戻ります

    public IEnumerator AttackAnim(Transform from,Transform target)
    {
        
        float elapsedTime = 0f;


        IgnoreLayout(from,true);

        Vector3 startPosition = from.GetComponent<Transform>().position;
        Vector3 endPosition = target.GetComponent<Transform>().position;

        Debug.Log($"startPosition:{startPosition},endPosition:{endPosition}");


        //identityEuler(0, 0, -45)
        Quaternion startRotation = Quaternion.identity;//これは無回転
        Quaternion endRotation = Quaternion.Euler(0,0,-45);

        while (elapsedTime <AttackDurationGO)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / AttackDurationGO;

            //位置と回転を補完
            rectTransform.position = Vector3.Lerp(startPosition,endPosition,t);
            rectTransform.rotation = Quaternion.Lerp(startRotation, endRotation, t);


            yield return null;

        }


        elapsedTime = 0f;

        while (elapsedTime < AttackDurationBack)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / AttackDurationBack;

            //位置と回転を補完
            rectTransform.position = Vector3.Lerp(endPosition, startPosition, t);
            rectTransform.rotation = Quaternion.Lerp(endRotation, startRotation, t);


            yield return null;

        }



        rectTransform.position = startPosition;
        rectTransform.rotation = startRotation;

        IgnoreLayout(from,false);

        yield return new WaitForSeconds(AttackDurationGO+AttackDurationBack);


    }
}
