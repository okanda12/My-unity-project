using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//PlayerField�ɂ��Ă���X�N���v�g�ł�.

public class DropPlace : MonoBehaviour, IDropHandler
{


    CardModel model;
    public void OnDrop(PointerEventData eventData)
    {
        //�J�[�h���d�Ȃ������ɐe��ύX����.


        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        
        
        // Debug.Log(card.defaultParent);


        // �h���b�v���ꂽ�J�[�h�̃f�[�^���Q�Ƃ���

        if (card != null)
        {
            model = card.cardModel;
            //Debug.Log(this.transform);
            //Debug.Log(card.defaultParent);
            
            //Debug.Log($"Card Dropped: {model.name}, AT: {model.at}, HP: {model.hp}, Cost: {model.cost}");
            if (model.cost > BattleManager.Instance.Player_Mana || card.defaultParent == this.transform)
            {
                //�e��hand�ɂ����܂ܕԂ�
               
                return;
            }
            else
            {//�J�[�h���o����Ƃ�



                //�e������ɂ��ĕԂ�
                card.defaultParent = this.transform;

                
                


            }
            
        }
        else
        {
            Debug.Log("nocards from dropplace");
        }

    }

}
