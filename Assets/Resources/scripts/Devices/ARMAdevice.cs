using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//ARMAくんが持っているせんぷうきみたいなデバイスについているスクリプトです.
//自分がコストを使うとゲージが溜まっていき,報酬カードがもらえます.
//コストを使うたびにせんぷうきが回ります.

public class ARMAdevice :Herodevices,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private int rewardCost = 50;//特殊カードを得るためのコスト
    [SerializeField] private int currentCost = 0; //現在までの類型コスト
    [SerializeField] private Transform hane;//find使うより最初から設定しておくと良いかな
    [SerializeField] private GameObject rewardCardPrefab;//　報酬カードのPrefab
    [SerializeField] private TextMeshProUGUI ARMAtext;//類型コストを表示します.

    [SerializeField] Slider slider;//スライダーを入れてください
    //[SerializeField] private Color startColor;
   // [SerializeField] private Color endColor;
   //色変える場合はこれ使いましょう

    public float rotationDuration;



    public override void Initialize()
    {
        

        currentCost = 0;
    }

    void Start()
    {
        ARMAtext.gameObject.SetActive(false);//初期状態でテキストは非表示とします.
        slider.value = 0;
    }

    //
    public void AddCost(int cost)
    {
        currentCost += cost;
        Debug.Log($"Cost added: {cost}. Current cost: {currentCost}/{rewardCost}");


        StartCoroutine(hanerotation());//羽根回転モーション

        if (currentCost >= rewardCost)
        {
            GrantReward();
            currentCost = 0;//リセット


        }

        slider.value = (float)currentCost/(float)rewardCost;//スライダー更新
        Debug.Log($"ARMA slider.value{slider.value}warizan{currentCost / rewardCost}");

    }

    private void GrantReward()
    {
        Debug.Log("Special card granted!");
        Instantiate(rewardCardPrefab, transform.position, Quaternion.identity);
    }



    private IEnumerator hanerotation()
    {
        //float 
        float minspeed = 0f;
        float acceleration = 6f;
        float currentspeed =8f;//maxspeedから始まり,徐々に下げていく


        float elapsedTime = 0f;

        while (elapsedTime<rotationDuration) {

            elapsedTime += Time.deltaTime;
            if (elapsedTime > rotationDuration/2)
            {
                currentspeed  -= acceleration * Time.deltaTime;
                //Debug.Log(currentspeed);
            }
            else
            {
                
            }
            
            
            float t = elapsedTime / rotationDuration;

            currentspeed = Mathf.Max(currentspeed, minspeed);//0か今の速度か大きい方
            //float rotation = Mathf.Lerp(0, 360, t);



            //初期位置取得childTransform.localRotation; 
            //Quaternion ini= haneTransform.localRotation;

            //Debug.Log($"Parent object: {this.name}");
            //Debug.Log(hane);
            //Debug.Log(rotation);
            //forward=(0,0,1)
            hane.transform.Rotate(0,0, currentspeed + Time.deltaTime);
            //色変えるやつ
            //hane.image.olor = Color.Lerp(startColor, endColor, t);


            yield return null;
        }
    }










    /// <summary>
    /// ///////////////////////////////////
    /// </summary>
    /// <param name="eventData"></param>
    //ここから下はマウスをかざした時の処理です.

    public void OnPointerEnter(PointerEventData eventData)
    {

        Debug.Log("ARMAデバイスを触りましたね");
        UpdateARMAText();//コスト情報をアップデートします
        ARMAtext.gameObject.SetActive(true);//テキストを表示
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ARMAtext.gameObject.SetActive(false);

    }

    private void UpdateARMAText()
    {
        ARMAtext.text=$"ARMA:{currentCost}/{rewardCost}" ;
    }

}
