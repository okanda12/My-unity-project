using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

//カードの動きをつかさどるスクリプトになります

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //ドラッグ可能かどうかのフラグ


    
    //おそらく最初はHandになってる？
    public Transform defaultParent;
    public CardModel cardModel;
    CardController cardcon;
    public CardCastAnimation castAnim;

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private int originalSortingOrder;

    private bool isDraggable;






    public void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        cardcon = this.GetComponent<CardController>();

        if (canvas != null)
        {
            //元のソート順を記録
            originalSortingOrder = canvas.sortingOrder;

        }
    }

    //手札からフィールドにカードの位置を変更する.ドラッグなし
    public void SetCardTransform(Transform parentTransform)
    {
        
        defaultParent = parentTransform;
        transform.SetParent(defaultParent, false);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {

        isDraggable = (transform.parent == BattleManager.Instance.PlayerHandTransform);
        //親オブジェクトparentを変数に保存
        if (!isDraggable) return;
        defaultParent = transform.parent;
        //親オブジェクトを祖父母にする　false=ローカル座標を使用
        transform.SetParent(defaultParent.parent, false);
        //false=オブジェクトドラッグ中に他のUIがクリック可能に
        
        //描画順序を最前面にする
        if(canvas != null)
        {
            canvas.sortingOrder = 1000; //最前面に表示するためのソート順
        }

        //ドラッグ中に他のUIがクリック可能になる
        canvasGroup.blocksRaycasts = false;
        //canvasGroup.alpha = 0.8f;//少し透明に

        foreach (Transform daiza in BattleManager.Instance.PlayerFieldTransform)
        {
            if (daiza.childCount < 1)
            {
                Animator animator = daiza.GetComponent<Animator>();

                animator.SetBool("chose", true);
            }
        }



    }


    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;
        

        Vector3 cardPos = Camera.main.ScreenToWorldPoint(eventData.position);
        cardPos.z = 0;
        transform.position = cardPos;
        //カードを引っ張ったときに行う処理
        //transform.position = eventData.position;
        

        ///////////////////////////////
        //フィールドを発光させます
        

        ////////////////////////////
       




    }




    public void OnEndDrag(PointerEventData eventData)
    {//DropPlaceからdefaultParentがDropplaceがついているオブジェクトになって返ってくる
        if (!isDraggable) return;
        CardCastAnimation castAnim = this.GetComponent<CardCastAnimation>();


        ////フィールドの光を消します
        foreach (Transform daiza in BattleManager.Instance.PlayerFieldTransform)
        {
            Animator animator = daiza.GetComponent<Animator>();

            animator.SetBool("chose", false);
        }



        if (BattleManager.Instance.isPlayerTurn == false) //もし相手ターンならやらない.
        {
            StartCoroutine(castAnim.cantMinionCast(defaultParent));
            Debug.Log("youcan't play in Enemy Turn!!!");
        }
        else
        {
            //プレイヤーのカードでなければドラッグ不可



            if (cardModel.cost > BattleManager.Instance.Player_Mana || defaultParent ==BattleManager.Instance.PlayerHandTransform)
            {　//出せない
                Debug.Log("youcan't play this card!!");
                StartCoroutine(castAnim.cantMinionCast(defaultParent));
            
            
                
            }
            else//カードが出せるとき
            {
                //defaultParentがおそらくDaizaになってます
                if (defaultParent.childCount != 0)//既に親が子を持っていたら
                {
                    Debug.Log("youcan't play this card!!");
                    StartCoroutine(castAnim.cantMinionCast(BattleManager.Instance.PlayerHandTransform));
                }
                else
                {
                    

                    

                    

                    if (cardModel.cardType == "Magic")
                    {//source target

                        StartCoroutine(castAnim.MagicCast(defaultParent));
                        
                        
                        
                    }
                    else
                    {
                        StartCoroutine(castAnim.MinionCast(defaultParent));

                        



                    }

                    BattleManager.Instance.manasys.UseMana(cardModel.cost);


                    cardcon.ARMA(cardModel.cost);//アルマデバイスにコスト分追加する


                }

            }


        }


        //カードを話した時に行う処理　元の親に戻す
        //transform.SetParent(defaultParent, false);

        //描画順序を元に戻す
        if (canvas != null)
        {
            canvas.sortingOrder = originalSortingOrder;
        }

        //Raycastブロックを元に戻す
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

    }





















}
