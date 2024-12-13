using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//カードの見た目をきめるスクリプトです.
//cardcontrollerとcardmodelとcardviewがあります.



public class CardView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText, hpText, atText, cosText;
    [SerializeField] Image iconImage;
    [SerializeField] Image waku;
    [SerializeField] Image Dot;
    [SerializeField] Frames frame;
    [SerializeField] Image Heart;
    [SerializeField] Image Sword;



    public void Show(CardModel cardModel)
    {

        if (cardModel.cardType=="Hero")
        {
            nameText.text = null;
            hpText.text = cardModel.hp.ToString();
            atText.text = cardModel.at.ToString();
            Dot.enabled = false;
            cosText.text = null;
            iconImage.sprite = cardModel.icon;
            waku.sprite = frame.Heroframe;



            //スプライトsizeに合わせてrecttransformを変更

            AdjustImagetoSprite(iconImage);
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one * 1.1f;


            //ダメージを受けていたらテキストを赤く
            if (cardModel.hp < cardModel.maxhp)
            {
                hpText.color = new Color(0.9f, 0.2f, 0.2f, 0.8f);
            }

            //増強していたら緑色に

            if (cardModel.hp > cardModel.maxhp)
            {
                hpText.color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
            }


        }
        else if(cardModel.cardType == "Minion")
        {
            waku.sprite = frame.Minionframe;
            nameText.text = cardModel.name;
            hpText.text = cardModel.hp.ToString();
            atText.text = cardModel.at.ToString();
            cosText.text = cardModel.cost.ToString();
            iconImage.sprite = cardModel.icon;

            //ダメージを受けていたらテキストを赤く
            if (cardModel.hp < cardModel.maxhp)
            {
                hpText.color = new Color(0.9f, 0.2f, 0.2f, 0.8f);
            }

            //増強していたら緑色に
            if (cardModel.hp > cardModel.maxhp)
            {
                hpText.color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
            }

        }
        else if (cardModel.cardType == "Magic")
        {
            //魔法カードの場合はhp,atなどがいらないので削除しています
            waku.sprite = frame.Magicframe;
            nameText.text = cardModel.name;

            hpText.text = null;
            atText.text = null;
            cosText.text = cardModel.cost.ToString();
            iconImage.sprite = cardModel.icon;
            Heart.enabled = false;
            Sword.enabled = false;
        }

    }


    //Heroの部分は少し形が特殊なため,サイズを調整しています.
    private void AdjustImagetoSprite(Image image)
    {

        float kakudai = 0.53f;
        //スプライトのサイズを取得
        Vector2 spriteSize = new Vector2(kakudai*image.sprite.rect.width, kakudai*image.sprite.rect.height);

        //recttransformを取得してスプライトサイズを適用
        RectTransform rectTransform = image.GetComponent<RectTransform>();

        Debug.Log($"HEROrecttransform {rectTransform}");

        rectTransform.sizeDelta = spriteSize;


    }
}
