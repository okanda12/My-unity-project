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
        //���̃I�u�W�F�N�g�̖��O���擾���܂�
        string thisName = transform != null ? transform.name: "";

        //����,���ꂪEnemy_deck�Ȃ�isEnemyDeck��true
        isEnemyDeck = thisName == "Enemy_deck";

        //������Ԃł͔�\��
        deckText.gameObject.SetActive(false);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("�f�b�L�Ƀ}�E�X���������܂�����?");

        //isHovering = true;
        UpdateDeckText();
        deckText.gameObject.SetActive(true);//�e�L�X�g��\��

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //isHovering = false;
        deckText.gameObject.SetActive(false);//�e�L�X�g���\��
        Debug.Log("�f�b�L����}�E�X���͂����܂�����");
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
