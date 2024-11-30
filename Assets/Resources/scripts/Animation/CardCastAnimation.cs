using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCastAnimation : MonoBehaviour
{
    //カードをキャストする時のアニメーションです.
    //ミニオンとマジックで分けたいね

    //CardMovement のonenddragからMinionCastを呼ぶ.
    //回転しながらフィールドに出現する

    public float CastDuration;
    public float CantCastDuration;




    private Transform rectTransform;
    


    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

 
    

    //回転しながらマウスの位置から場に出ます
    //endofdragでdefaultparentを設定すると途中でplayerhandになってしまうのでここで親を設定
    public IEnumerator MinionCast(Transform defaultParent)
    {
        float elapsedTime = 0f;
        Vector3 EndPosition;
        
        
        //マウスの位置はワールド座標なのでローカルに変える必要がある．
        Vector3 StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//マウスの位置を取得します

        //Debug.Log($"Start={StartPosition}");

        //自分のターンか否かによって判断する.
        //本当は親で判断した方が良いんだろうけどDrag中は親がcanvasになっちゃうから....

        bool isPlayerTurn = GameManager.Instance.isPlayerTurn;

        
        //
        if (isPlayerTurn==true)
        {
            EndPosition = GameManager.Instance.PlayerFieldTransform.position;
            //Debug.Log("casting rotating2");
        }
        else
        {
            EndPosition = GameManager.Instance.EnemyFieldTransform.position;
            //Debug.Log("casting rotating3");
        }

        //Debug.Log("casting rotating4");
        //Debug.Log($"Start={StartPosition},End={EndPosition}");


        while (elapsedTime < CastDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / CastDuration;

            float rotation=Mathf.Lerp(0, 720, 4*t);//一回転

            //一回移転するように


            //Debug.Log(rotation);

            rectTransform.rotation = Quaternion.Euler(0, rotation, 0);
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition,1.5f*t);
            //Debug.Log($"parent is {this.transform.parent}");

            yield return null;

        }
        transform.SetParent(defaultParent, false);

        

        //yield return new WaitForSeconds(CastDuration);


        
        //Debug.Log("casting rotating5");

     


    }

    public IEnumerator cantMinionCast(Transform defaultParent)
    {
        float elapsedTime = 0f;
        Vector3 EndPosition=defaultParent.position;
        Vector3 StartPosition;


        //マウスの位置はワールド座標なのでローカルに変える必要がある．
        if (GameManager.Instance.isPlayerTurn==true)
        {
            StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//マウスの位置を取得します

        }
        else
        {

            StartPosition = GameManager.Instance.EnemyFieldTransform.transform.position;

        }

        //Debug.Log($"Start={StartPosition}");

        //自分のターンか否かによって判断する.
        //本当は親で判断した方が良いんだろうけどDrag中は親がcanvasになっちゃうから....

        bool isPlayerTurn = GameManager.Instance.isPlayerTurn;


        

        //Debug.Log("casting rotating4");
        //Debug.Log($"Start={StartPosition},End={EndPosition}");


        //回転せず素直に戻ります
        while (elapsedTime < CantCastDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / CantCastDuration;
            
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition, 2* t);
            //Debug.Log($"parent is {this.transform.parent}");

            yield return null;

        }


        transform.SetParent(defaultParent, false);
        yield return new WaitForSeconds(CantCastDuration);

       
        //Debug.Log("casting rotating5");




    }






}
