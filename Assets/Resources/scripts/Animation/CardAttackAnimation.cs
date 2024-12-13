using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//カードが攻撃するアニメーションです.
public class CardAttackAnimation : MonoBehaviour
{

    public CardView View;
    public CardModel Model;
    public GameObject banish;//死ぬときにもわわ＝んとなるやつ

  
    //一連の動き
    //fromカードが攻撃してtargetに向かい,fromに戻ります.

    public float AttackDurationGO;//行きの時間
    public float AttackDurationBack;//戻りの時間
    public float CantAttackDuration;//攻撃できない

    public float DieDuration;//死んでいくときのアニメーション時間

    private Transform rectTransform;//アタッチされているカードのtransformです

    public Transform HeartPosition;

    public GameObject Attackedtext;//攻撃されたとき,したときにぽわわ～んとなるやつ
    public float AttackedDuration;//の時間
    public float bletime;

    [SerializeField] private Animator HEROdamageAnim;//ヒーろーがダメージを受けたときのアニメーション


    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
       


    }

    //ダメージを受けたときにちょっとぶれるアニメーションです
    public IEnumerator ble()
    {
        Vector3 startPosition = this.transform.position;
        
        float elapsedTime = 0f;

        while (elapsedTime < bletime)
        {

            elapsedTime += Time.deltaTime;
            this.transform.position = startPosition + new Vector3(0.03f*Random.Range(-1f,1f),0.03f* Random.Range(-1f, 1f), 0);

            yield return null;
        }


        this.transform.position = startPosition;
    }


    //ハートが飛んでいくアニメーション
    public IEnumerator HeartDamage(int damage)
    {

        Vector3 startPosition = HeartPosition.position;
        Vector3 endPosition = startPosition + new Vector3(0, 0.05f, 0);


        TextMeshProUGUI Damagetext = Attackedtext.GetComponentInChildren<TextMeshProUGUI>();


        Damagetext.text = damage.ToString();

        GameObject Damagetextobject = Instantiate(Attackedtext, HeartPosition, false);







        float elapsedTime = 0f;


        while (elapsedTime < AttackedDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / AttackedDuration;

            if (Damagetextobject != null)
            {
                Damagetextobject.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            }

            yield return null;


        }

        Destroy(Damagetextobject);//消す

        if (this != null)
        {
            Model = GetComponent<CardController>().model;
            View = GetComponent<CardController>().view;


            if (Model.cardType=="Hero")
            {
                HEROdamageAnim.SetTrigger("HeroDamage");
            }


            //ダメージを受けます
            Model.hp -= damage;
            if (Model.hp <= 0)
            {
                StartCoroutine(Die());

            }

            
            View.Show(Model);
        }

    }

    //ダメージを受けたときのアニメーション
    public IEnumerator GetDamage(int damage)
    {


        StartCoroutine(ble());
        //Transform HeartPosition = transform.Find("Heart");//ハートの位置から飛ばす
        yield return StartCoroutine(HeartDamage(damage));

    }


    //死ぬときです.爆発(banisher)して死にます.
  
    public IEnumerator Die()
    {


        GameObject banisher=Instantiate(banish, transform);


        yield return new WaitForSeconds(0.5f);

        Destroy(banisher);

        Destroy(gameObject);

    }
    //}


    //カードに動きをつけるため,Horizontal Layoutを一瞬だけ無効にします.
    //Horizontal Layout を付けているとpositionの変更ができません....
    public void IgnoreLayout(Transform from,  bool ignore)
    {
        LayoutElement layoutElement = from.GetComponent<LayoutElement>();

        if (layoutElement != null)//あるなら
        {
            layoutElement.ignoreLayout = ignore;//レイアウトを無視するか設定
        }
    }

    //攻撃できないとき,ちょっと回転して戻ります.はにゃ？見たいな感じ
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
    public IEnumerator AttackAnim(CardController fromCard,CardController targetCard)
    {



        


        Transform from = this.transform;

        
        Transform target = targetCard.transform;

        Transform defaultparent = this.transform.parent;

        //いったん一番うえに表示する


        float elapsedTime = 0f;


        IgnoreLayout(from,true);//一旦レイアウトを無視します.

        Vector3 startPosition = from.GetComponent<Transform>().position;
        Vector3 endPosition = target.GetComponent<Transform>().position;



        Debug.Log($"startPosition:{startPosition},endPosition:{endPosition}");


        //identityEuler(0, 0, -45)
        Quaternion startRotation = Quaternion.identity;//これは無回転
        Quaternion endRotation = Quaternion.Euler(0,0,-45);


        this.transform.SetParent(BattleManager.Instance.canvas2, true);//おそらくここで設定するのが良さそう

        //////////////////
        ///アタック行き
        while (elapsedTime <AttackDurationGO - 0.2*AttackDurationGO)//完全に重なる直前で止まる
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / AttackDurationGO;

            //位置と回転を補完
            rectTransform.position = Vector3.Lerp(startPosition,endPosition,t);
            rectTransform.rotation = Quaternion.Lerp(startRotation, endRotation, t);


            yield return null;

        }
        endPosition = rectTransform.position;//現在位置
        endRotation = rectTransform.rotation;//現在位置
        ////////////////////////
        //アタック途中

        CardAttackAnimation targetAnim = targetCard.GetComponent<CardAttackAnimation>();//無理やり持ってきてる
        
        
        StartCoroutine(targetAnim.GetDamage(fromCard.model.at));//相手に受けさせたい
        StartCoroutine(GetDamage(targetCard.model.at));//自分が受ける

        yield return new WaitForSeconds(0.2f);//なんかこれじゅうようやね


        /////////////////////////////////
        //アタック帰り
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
        this.transform.SetParent(defaultparent, true);
        Debug.Log($"defaultParent{defaultparent}");

        //yield return new WaitForSeconds(AttackDurationGO+AttackDurationBack);
        fromCard.canAttack = false;

    }
}
