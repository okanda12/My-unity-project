using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCastAnimation : MonoBehaviour
{
    //カードをキャストする時のアニメーションです.
    //ミニオンとマジックで分けたいね

    //DropPlaceからMinionCastを呼ぶ.
    //回転しながらフィールドに出現する

    public float CastedDuration;




    private Transform rectTransform;
    float rotation = 1f;//回転数


    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }


    //回転しながらマウスの位置から場に出ます
    public IEnumerator MinionCast()
    {
        float elapsedTime = 0f;
        Vector3 EndPosition;
        
        

        Vector3 StartPosition = Input.mousePosition;//マウスの位置を取得します

        string parent = this.transform.parent.name;

        //
        if (parent=="PlayerHand")
        {
            EndPosition = GameManager.Instance.PlayerHandTransform.position;
        }
        else
        {
            EndPosition = GameManager.Instance.EnemyHandTransform.position;
        }
        


        while (elapsedTime < CastedDuration)
        {
            elapsedTime += Time.deltaTime;

            //一回移転するように
            rotation=Mathf.Sin(2*Mathf.PI*  (elapsedTime/CastedDuration));

            rectTransform.rotation = Quaternion.Euler(0, rotation, 0);
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition, rotation);

            yield return null;

        }


        yield return new WaitForSeconds(CastedDuration);





    }




}
