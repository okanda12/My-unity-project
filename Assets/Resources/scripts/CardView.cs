using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText, hpText, atText, cosText;
    [SerializeField] Image iconImage;
    [SerializeField] Image waku;
    [SerializeField] Frames frame;



    public void Show(CardModel cardModel)
    {

        if (cardModel.cardType=="Hero")
        {
            nameText.text = null;
            hpText.text = cardModel.hp.ToString();
            atText.text = cardModel.at.ToString();
            cosText.text = null;
            iconImage.sprite = cardModel.icon;
            waku.sprite = frame.Heroframe;



            //スプライトsizeに合わせてrecttransformを変更

            AdjustImagetoSprite(iconImage);
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one * 1.1f;

           
        }
        else if(cardModel.cardType == "Minion")
        {
            waku.sprite = frame.Minionframe;
            nameText.text = cardModel.name;
            hpText.text = cardModel.hp.ToString();
            atText.text = cardModel.at.ToString();
            cosText.text = cardModel.cost.ToString();
            iconImage.sprite = cardModel.icon;

        }
        else if (cardModel.cardType == "Magic")
        {
            waku.sprite = frame.Magicframe;
            nameText.text = cardModel.name;
            hpText.text = cardModel.hp.ToString();
            atText.text = cardModel.at.ToString();
            cosText.text = cardModel.cost.ToString();
            iconImage.sprite = cardModel.icon;

        }

    }

    private void AdjustImagetoSprite(Image image)
    {

        float kakudai = 2f;
        //スプライトのサイズを取得
        Vector2 spriteSize = new Vector2(kakudai*image.sprite.rect.width, kakudai*image.sprite.rect.height);

        //recttransformを取得してスプライトサイズを適用
        RectTransform rectTransform = image.GetComponent<RectTransform>();

        Debug.Log($"HEROrecttransform {rectTransform}");

        rectTransform.sizeDelta = spriteSize;


    }
}
