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
    

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private int originalSortingOrder;

    private bool isDraggable;


    public void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

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

        isDraggable = (transform.parent == GameManager.Instance.PlayerHandTransform);
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
        canvasGroup.alpha = 0.8f;//少し透明に
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;
        

        Vector3 cardPos = Camera.main.ScreenToWorldPoint(eventData.position);
        cardPos.z = 0;
        transform.position = cardPos;
        //カードを引っ張ったときに行う処理
        //transform.position = eventData.position;


        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;
        //プレイヤーのカードでなければドラッグ不可
        
        //カードを話した時に行う処理　元の親に戻す
        transform.SetParent(defaultParent, false);

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
