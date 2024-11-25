using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//PlayerFieldについているスクリプトです.

public class DropPlace : MonoBehaviour, IDropHandler
{


    CardModel model;
    public void OnDrop(PointerEventData eventData)
    {
        //カードが重なった時に親を変更する.


        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        CardController cardcon = eventData.pointerDrag.GetComponent<CardController>();
        CardCastAnimation castAnim = card.GetComponent<CardCastAnimation>();
        // Debug.Log(card.defaultParent);


        // ドロップされたカードのデータを参照する

        if (card != null)
        {
            model = card.cardModel;
            //Debug.Log(this.transform);
            //Debug.Log(card.defaultParent);
            
            //Debug.Log($"Card Dropped: {model.name}, AT: {model.at}, HP: {model.hp}, Cost: {model.cost}");
            if (model.cost > GameManager.Instance.Player_Mana || card.defaultParent == this.transform)
            {
                //Debug.Log(this.transform);
                Debug.Log("youcan't play this card!!");
                return;
            }
            else
            {//カードが出せるとき



                StartCoroutine(castAnim.MinionCast());
                card.defaultParent = this.transform;

                
                GameManager.Instance.manasys.UseMana(model.cost);

                if (model.cardType=="Magic")
                {//source target

                    
                    model.BattleCry(cardcon, GameManager.Instance.PlayerHerocon);
                    cardcon.Die();//破壊する
                }
                else
                {
                    
                    model.BattleCry(cardcon, GameManager.Instance.PlayerHerocon);



                }

                //効果使う
                




            }
            
        }
        else
        {
            Debug.Log("nocards from dropplace");
        }

    }

}
