using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//�J�[�h�̌����ڂ����߂�X�N���v�g�ł�.
//cardcontroller��cardmodel��cardview������܂�.

public class CardView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText, hpText, atText, cosText,cardText;
    [SerializeField] Image iconImage;
    [SerializeField] Image waku;
    [SerializeField] Image Dot;
    [SerializeField] Frames frame;
    [SerializeField] Image Heart;
    [SerializeField] Image Sword;

 



    public void Show(CardModel cardModel)
    {
        if (string.IsNullOrEmpty(cardModel.cardText))
        {
            cardModel.cardText = " ";
        }

        if (cardModel.cardType=="Hero")
        {
            nameText.text = null;
            hpText.text = cardModel.hp.ToString();
            atText.text = cardModel.at.ToString();
            //cardText.text = cardModel.cardText;
            Dot.enabled = false;
            cosText.text = null;
            iconImage.sprite = cardModel.icon;
            waku.sprite = frame.Heroframe;


            //�X�v���C�gsize�ɍ��킹��recttransform��ύX

            AdjustImagetoSprite(iconImage);
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one * 1.1f;


            //�_���[�W���󂯂Ă�����e�L�X�g��Ԃ�
            if (cardModel.hp < cardModel.maxhp)
            {
                hpText.color = new Color(0.9f, 0.2f, 0.2f, 1f);
            }

            //�������Ă����甒�F��

            if (cardModel.hp > cardModel.maxhp)
            {
                hpText.color = new Color(0.5f, 1.0f, 0.5f, 1f);
            }


        }
        else if(cardModel.cardType == "Minion")
        {
            waku.sprite = frame.Minionframe;
            nameText.text = cardModel.name;
            hpText.text = cardModel.hp.ToString();
            atText.text = cardModel.at.ToString();
            cardText.text = cardModel.cardText;
            cosText.text = cardModel.cost.ToString();
            iconImage.sprite = cardModel.icon;

            //�_���[�W���󂯂Ă�����e�L�X�g��Ԃ�
            if (cardModel.hp < cardModel.maxhp)
            {
                hpText.color = new Color(0.9f, 0.2f, 0.2f, 1f);
            }

            //�������Ă�����ΐF��
            if (cardModel.hp > cardModel.maxhp)
            {
                hpText.color = new Color(0.5f, 1.0f, 0.5f, 1f);
            }


            //�e�̐e�I�u�W�F�N�g���`�F�b�N
            Transform parent = transform.parent;
            if (parent != null && parent.parent != null)
            {
                Transform grandparent = parent.parent;


                //��ɂ���Ƃ��͎ז��Ȃ̂Ŗ��O���폜
                if (grandparent.name=="PlayerField" || grandparent.name=="EnemyField")
                {
                    
                    nameText.text = null;
                    Dot.enabled= false;
                    cosText.text = null;
                }

            }





        }
        else if (cardModel.cardType == "Magic")
        {

            //���@�J�[�h�̏ꍇ��hp,at�Ȃǂ�����Ȃ��̂ō폜���Ă��܂�
            waku.sprite = frame.Magicframe;
            nameText.text = cardModel.name;
            cardText.text = cardModel.cardText;

            hpText.text = null;
            atText.text = null;
            cosText.text = cardModel.cost.ToString();
            iconImage.sprite = cardModel.icon;
            Heart.enabled = false;
            Sword.enabled = false;
        }

    }


    //Hero�̕����͏����`������Ȃ���,�T�C�Y�𒲐����Ă��܂�.
    private void AdjustImagetoSprite(Image image)
    {

        float kakudai = 0.53f;
        //�X�v���C�g�̃T�C�Y���擾
        Vector2 spriteSize = new Vector2(kakudai*image.sprite.rect.width, kakudai*image.sprite.rect.height);

        //recttransform���擾���ăX�v���C�g�T�C�Y��K�p
        RectTransform rectTransform = image.GetComponent<RectTransform>();

        Debug.Log($"HEROrecttransform {rectTransform}");

        rectTransform.sizeDelta = spriteSize;


    }
}
