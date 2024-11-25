using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Costdisp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costText;

    private bool isEnemyCost;//�G�̃R�X�g�H�����̃R�X�g?

    private void Start()
    {
        //�ϐ�=������ ? true�̏���: false�̏���
        string parentName = transform.parent != null ? transform.parent.name : "";

        //����parentName="Enemycost"�Ȃ�Enemycost��true
        isEnemyCost = parentName == "Enemycost";
    }
    // Update is called once per frame
    void Update()
    {
        // costText�̏�Ԃɉ����� Text ���X�V
        if (isEnemyCost==true)
        {
            costText.text = $"mana:{GameManager.Instance.Enemy_Mana}/{GameManager.Instance.Enemy_maxMana}";

        }
        else
        {
            costText.text = $"mana:{GameManager.Instance.Player_Mana}/{GameManager.Instance.Player_maxMana}";

        }



    }
}
