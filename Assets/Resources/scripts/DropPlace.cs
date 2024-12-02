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
        
        
        // Debug.Log(card.defaultParent);


        // ドロップされたカードのデータを参照する

        if (card != null)
        {
            model = card.cardModel;
            //Debug.Log(this.transform);
            //Debug.Log(card.defaultParent);
            
            //Debug.Log($"Card Dropped: {model.name}, AT: {model.at}, HP: {model.hp}, Cost: {model.cost}");
            if (model.cost > BattleManager.Instance.Player_Mana || card.defaultParent == this.transform)
            {
                //親をhandにしたまま返す
               
                return;
            }
            else
            {//カードが出せるとき



                //親をこれにして返す
                card.defaultParent = this.transform;

                
                


            }
            
        }
        else
        {
            Debug.Log("nocards from dropplace");
        }

    }

}
