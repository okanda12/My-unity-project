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


    [SerializeField] GameObject RotateEffect;//回転するときにでるエフェクトです




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

            float rotation = Mathf.Lerp(0,  360, 4 * t);//4回転

     
            

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

            //Debug.Log(canvasGroup.alpha);
            yield return null;

        }
        


        

        GameObject twinkle = Instantiate(twinklePrefab, BattleManager.Instance.MagiCasPlace);


        IgnoreLayout(BattleManager.Instance.EnemyHandTransform, false);

        
        rectTransform.localPosition = Vector3.zero;//なんかこれ無いと安定してくれない....


        //yield return new WaitForSeconds(CastDuration);
        if (BattleManager.Instance.isPlayerTurn == false)//相手ターンなら
        {
            cardcon.model.BattleCry(cardcon, BattleManager.Instance.EnemyHerocon);
        }
        else
        {
            cardcon.model.BattleCry(cardcon, BattleManager.Instance.PlayerHerocon);
        }
        

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

        Vector3 StartScale=this.transform.localScale;
        Vector3 EndScale = StartScale * 1.2f ;



        //IgnoreLayout(BattleManager.Instance.EnemyHandTransform, false);

        //マウスの位置はワールド座標なのでローカルに変える必要がある．

        //Debug.Log($"defaultparent={defaultParent.name}");

        //自分のターンか否かによって判断する.
        //本当は親で判断した方が良いんだろうけどDrag中は親がcanvasになっちゃうから....

        bool isPlayerTurn = BattleManager.Instance.isPlayerTurn;

        
        //始まりと終わりを設定します
        if (isPlayerTurn==true)
        {

            StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            EndPosition = defaultParent.position;
            //Debug.Log("casting rotating2");
        }
        else//相手が召喚するなら
        {
            //ハンドのレイアウトは無視します
            IgnoreLayout(BattleManager.Instance.EnemyHandTransform, true);
            StartPosition = BattleManager.Instance.EnemyHandTransform.position;
            EndPosition = defaultParent.position;
            //Debug.Log("casting rotating3");
        }

        StartScale = this.transform.localScale;
        EndScale = StartScale*1.3f;


        //Debug.Log("casting rotating4");
        //Debug.Log($"Start={StartPosition},End={EndPosition}");

        //カードを出します

        yield return StartCoroutine(CardRotating(StartPosition, EndPosition,StartScale,EndScale));
       



        transform.SetParent(defaultParent, false);





        rectTransform.localPosition = Vector3.zero;//なんかこれ無いと安定してくれない....
        //場に出した時にちょっと邪魔なので名前を消します
        cardcon.view.Show(cardcon.model);

        //yield return new WaitForSeconds(CastDuration);

       

        cardcon.model.BattleCry(cardcon, BattleManager.Instance.PlayerHerocon);

        //Debug.Log("casting rotating5");

        yield return null;




    }


    //カードを回転させ,見せながら台座に設置します．

    public IEnumerator CardRotating(Vector3 StartPosition,Vector3 EndPosition, Vector3 StartScale, Vector3 EndScale)
    {
        float elapsedTime = 0f;
        float elapsedTime2 = 0f;//拡大縮小で使います
        float t = 0f;

        //回転エフェクトを出します.
        GameObject rotateEffect = Instantiate(RotateEffect, transform);

        //回転し,拡大する
        float go = 0.5f*CastDuration;
        while (elapsedTime < go)//途中まで
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / CastDuration;

            float rotation = Mathf.Lerp(0,  7*360, t);//4回転

          

            //Debug.Log(rotation);

            this.transform.localScale = Vector3.Lerp(StartScale, EndScale, t);
            rectTransform.rotation = Quaternion.Euler(0, rotation, 0);
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition,  t);
            //Debug.Log($"parent is {this.transform.parent}");

            yield return null;

        }




        //一瞬止まってミセル
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);
        EndScale = this.transform.localScale;
        Vector3 EndScale2 = 1.08f * EndScale;
        elapsedTime = 0;
        float stopDuration = 0.2f;//壱秒くらい？
        while (elapsedTime < stopDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / stopDuration;

            //float rotation = Mathf.Lerp(0, 7 * 360, t);//4回転

            //一回移転するように


            //Debug.Log(rotation);
            this.transform.localScale = Vector3.Lerp(EndScale, EndScale2, t);
            
            yield return null;

        }


        Destroy(rotateEffect);
        //一気に台座に収まる

        //現在のtransform
        StartPosition = this.transform.position;


        elapsedTime = 0;
        float daizaDuration = 0.1f;
        
        while (elapsedTime < daizaDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / daizaDuration;

          

            //Debug.Log(rotation);
            this.transform.localScale = Vector3.Lerp(EndScale2, StartScale, t);
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition, t);


            yield return null;

        }







        
       
        IgnoreLayout(BattleManager.Instance.EnemyHandTransform, false);
       
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
