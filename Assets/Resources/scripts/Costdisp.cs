using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;



//�R�X�g��\������e�L�X�g�ł�.hover����悤�ɂ��܂�
public class Costdisp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    [SerializeField] private TextMeshProUGUI costText;

    private bool isEnemyCost;//�G�̃R�X�g�H�����̃R�X�g?
    private bool isHovering;//�z�o�[�����ǂ���


    private void Start()
    {


        //�ϐ�=������ ? true�̏���: false�̏���
        string thisName = transform != null ? transform.name : "";

        //�������ꂪ"Enemycost"�Ȃ�Enemycost��true
        isEnemyCost = thisName == "Enemycost";

        //������Ԃł͔�\��
        costText.gameObject.SetActive(false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnpointerEnter");

        isHovering = true;
        UpdateCostText();//�e�L�X�g���X�V
        costText.gameObject.SetActive(true);//�e�L�X�g��\��
       
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        costText.gameObject.SetActive(false);//�e�L�X�g���\��
        Debug.Log("OnpointerExit");
    }

    private void UpdateCostText()
    {
        if (isEnemyCost == true)
        {
            costText.text = $"DOT:{BattleManager.Instance.Enemy_Mana}/{BattleManager.Instance.Enemy_maxMana}";

        }
        else
        {
            costText.text = $"DOT:{BattleManager.Instance.Player_Mana}/{BattleManager.Instance.Player_maxMana}";

        }

    }
}
