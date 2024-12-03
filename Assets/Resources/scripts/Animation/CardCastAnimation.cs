using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCastAnimation : MonoBehaviour
{
    //カードをキャストする時のアニメーションです.
    //ミニオンとマジックで分けたいね

    //CardMovement のonenddragからMinionCastを呼ぶ.
    //回転しながらフィールドに出現する

    public float CastDuration;
    public float CantCastDuration;
    public float MagicCastDuration;
    public float MagicbanishDuration;
    CardController cardcon;
    public GameObject twinklePrefab;//魔法をキャストしたときのまたたき

    private Transform rectTransform;
    private CanvasGroup canvasGroup;





    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
        cardcon = this.GetComponent<CardController>();
        canvasGroup  = GetComponent<CanvasGroup>();
    }


    public void IgnoreLayout(Transform from, bool ignore)
    {
        LayoutElement layoutElement = from.GetComponent<LayoutElement>();

        if (layoutElement != null)//あるなら
        {
            layoutElement.ignoreLayout = ignore;//レイアウトを無視するか設定
        }
    }


    public IEnumerator MagicCast(Transform defaultParent)
    {
        float t;
        float elapsedTime = 0f;
        Vector3 EndPosition = BattleManager.Instance.MagiCasPlace.transform.position;
        Vector3 StartPosition;//マウスの位置を取得します


        //マウスの位置はワールド座標なのでローカルに変える必要がある．

        //Debug.Log($"defaultparent={defaultParent.name}");

        
        bool isPlayerTurn = BattleManager.Instance.isPlayerTurn;


        //
        if (isPlayerTurn == true)
        {
            StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            //Debug.Log("casting rotating2");
        }
        else//相手が発動するなら
        {
            IgnoreLayout(BattleManager.Instance.EnemyHandTransform, true);
            StartPosition = BattleManager.Instance.EnemyHandTransform.position;    
            //Debug.Log("casting rotating3");
        }

        //Debug.Log("casting rotating4");
        //Debug.Log($"Start={StartPosition},End={EndPosition}");


        while (elapsedTime < MagicCastDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / CastDuration;

            float rotation = Mathf.Lerp(0, 4 * 360, 4 * t);//4回転

     
            

            rectTransform.rotation = Quaternion.Euler(0, rotation, 0);
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition,  4* t);
            //Debug.Log($"parent is {this.transform.parent}");

            yield return null;

        }


        transform.SetParent(BattleManager.Instance.MagiCasPlace, true);//magicasplaceにおく
       
        elapsedTime = 0f;
        while (elapsedTime < MagicbanishDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / MagicbanishDuration;

            canvasGroup.alpha = 1f -   t;

            Debug.Log(canvasGroup.alpha);
            yield return null;

        }
        


        

        GameObject twinkle = Instantiate(twinklePrefab, BattleManager.Instance.MagiCasPlace);


        IgnoreLayout(BattleManager.Instance.EnemyHandTransform, false);

        
        rectTransform.localPosition = Vector3.zero;//なんかこれ無いと安定してくれない....


        //yield return new WaitForSeconds(CastDuration);

        cardcon.model.BattleCry(cardcon, BattleManager.Instance.PlayerHerocon);

        yield return new WaitForSeconds(0.5f);


        Destroy(twinkle);
        Destroy(gameObject);//ここ死にアニメーションにかえたい
        //Debug.Log("casting rotating5");



    }

    


    //回転しながらマウスの位置から場に出ます
    //endofdragでdefaultparentを設定すると途中でplayerhandになってしまうのでここで親を設定
    public IEnumerator MinionCast(Transform defaultParent)
    {
        float elapsedTime = 0f;
        Vector3 EndPosition=defaultParent.position;
        Vector3 StartPosition;//マウスの位置を取得します

        
        //マウスの位置はワールド座標なのでローカルに変える必要がある．

        Debug.Log($"defaultparent={defaultParent.name}");

        //自分のターンか否かによって判断する.
        //本当は親で判断した方が良いんだろうけどDrag中は親がcanvasになっちゃうから....

        bool isPlayerTurn = BattleManager.Instance.isPlayerTurn;

        
        //
        if (isPlayerTurn==true)
        {
            StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            EndPosition = defaultParent.position;
            //Debug.Log("casting rotating2");
        }
        else//相手が召喚するなら
        {
            IgnoreLayout(BattleManager.Instance.EnemyHandTransform, true);
            StartPosition = BattleManager.Instance.EnemyHandTransform.position;
            EndPosition = defaultParent.position;
            //Debug.Log("casting rotating3");
        }

        //Debug.Log("casting rotating4");
        //Debug.Log($"Start={StartPosition},End={EndPosition}");


        while (elapsedTime < CastDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / CastDuration;

            float rotation=Mathf.Lerp(0, 4*360, 4*t);//4回転

            //一回移転するように


            //Debug.Log(rotation);

            rectTransform.rotation = Quaternion.Euler(0, rotation, 0);
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition,1.5f*t);
            //Debug.Log($"parent is {this.transform.parent}");

            yield return null;

        }

        IgnoreLayout(BattleManager.Instance.EnemyHandTransform, false);

        transform.SetParent(defaultParent, false);
        rectTransform.localPosition = Vector3.zero;//なんかこれ無いと安定してくれない....


        //yield return new WaitForSeconds(CastDuration);

        cardcon.model.BattleCry(cardcon, BattleManager.Instance.PlayerHerocon);

        //Debug.Log("casting rotating5");




    }

    public IEnumerator cantMinionCast(Transform defaultParent)
    {//ハンドからミニオンが出せない時の処理です.
        float elapsedTime = 0f;
        Vector3 EndPosition=defaultParent.position;
        Vector3 StartPosition;
        

        //マウスの位置はワールド座標なのでローカルに変える必要がある．
        if (BattleManager.Instance.isPlayerTurn==true)
        {
            StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//マウスの位置を取得します

        }
        else
        {

            StartPosition = BattleManager.Instance.EnemyFieldTransform.transform.position;

        }

        //Debug.Log($"Start={StartPosition}");

        //自分のターンか否かによって判断する.
        //本当は親で判断した方が良いんだろうけどDrag中は親がcanvasになっちゃうから....

        bool isPlayerTurn = BattleManager.Instance.isPlayerTurn;


        

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
