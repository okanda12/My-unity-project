using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Costdisp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costText;

    private bool isEnemyCost;//敵のコスト？自分のコスト?

    private void Start()
    {
        //変数=条件文 ? trueの処理: falseの処理
        string parentName = transform.parent != null ? transform.parent.name : "";

        //もしparentName="Enemycost"ならEnemycostがtrue
        isEnemyCost = parentName == "Enemycost";
    }
    // Update is called once per frame
    void Update()
    {
        // costTextの状態に応じて Text を更新
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
