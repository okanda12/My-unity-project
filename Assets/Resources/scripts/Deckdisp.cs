using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using TMPro;




public class Deckdisp : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

    [SerializeField] private TextMeshProUGUI deckText;

    private bool isEnemyDeck;
    //private bool isHovering;

    // Start is called before the first frame update
    void Start()
    {
        //このオブジェクトの名前を取得します
        string thisName = transform != null ? transform.name: "";

        //もし,これがEnemy_deckならisEnemyDeckがtrue
        isEnemyDeck = thisName == "Enemy_deck";

        //初期状態では非表示
        deckText.gameObject.SetActive(false);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("デッキにマウスをかざしましたね?");

        //isHovering = true;
        UpdateDeckText();
        deckText.gameObject.SetActive(true);//テキストを表示

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //isHovering = false;
        deckText.gameObject.SetActive(false);//テキストを非表示
        Debug.Log("デッキからマウスをはずしましたね");
    }







    private void UpdateDeckText()
    {
        if (isEnemyDeck==true)
        {
            deckText.text = $"Deck: {BattleManager.Instance.enemyDeck.DeckCount}";
        }
        else 
        {
            deckText.text = $"Deck: {BattleManager.Instance.playerDeck.DeckCount}";
        }
       
    }





}
