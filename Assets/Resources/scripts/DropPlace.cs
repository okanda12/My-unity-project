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
        CardController cardcon = eventData.pointerDrag.GetComponent<CardController>();
        CardCastAnimation castAnim = card.GetComponent<CardCastAnimation>();
        // Debug.Log(card.defaultParent);


        // �h���b�v���ꂽ�J�[�h�̃f�[�^���Q�Ƃ���

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
            {//�J�[�h���o����Ƃ�



                StartCoroutine(castAnim.MinionCast());
                card.defaultParent = this.transform;

                
                GameManager.Instance.manasys.UseMana(model.cost);

                if (model.cardType=="Magic")
                {//source target

                    
                    model.BattleCry(cardcon, GameManager.Instance.PlayerHerocon);
                    cardcon.Die();//�j�󂷂�
                }
                else
                {
                    
                    model.BattleCry(cardcon, GameManager.Instance.PlayerHerocon);



                }

                //���ʎg��
                




            }
            
        }
        else
        {
            Debug.Log("nocards from dropplace");
        }

    }

}
