using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;



//コストを表示するテキストです.hoverするようにします
public class Costdisp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    [SerializeField] private TextMeshProUGUI costText;

    private bool isEnemyCost;//敵のコスト？自分のコスト?
    private bool isHovering;//ホバー中かどうか


    private void Start()
    {


        //変数=条件文 ? trueの処理: falseの処理
        string thisName = transform != null ? transform.name : "";

        //もしこれが"Enemycost"ならEnemycostがtrue
        isEnemyCost = thisName == "Enemycost";

        //初期状態では非表示
        costText.gameObject.SetActive(false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnpointerEnter");

        isHovering = true;
        UpdateCostText();//テキストを更新
        costText.gameObject.SetActive(true);//テキストを表示
       
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        costText.gameObject.SetActive(false);//テキストを非表示
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
