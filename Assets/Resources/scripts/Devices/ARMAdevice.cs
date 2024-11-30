using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMAdevice :Herodevices
{
    [SerializeField] private int rewardCost = 50;//特殊カードを得るためのコスト
    [SerializeField] private int currentCost = 0; //現在までの類型コスト
    [SerializeField] private Transform hane;//find使うより最初から設定しておくと良いかな
    [SerializeField] private GameObject rewardCardPrefab;//　報酬カードのPrefab

    //[SerializeField] private Color startColor;
   // [SerializeField] private Color endColor;
   //色変える場合はこれ使いましょう

    public float rotationDuration;


    //なんかさ～deviceの位置からカード飛ばした方が良いかも

    public override void Initialize()
    {
        
        
       currentCost = 0;
    }

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
        float currentspeed =4f;//maxspeedから始まり,徐々に下げていく


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





}
