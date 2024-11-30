using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMAdevice :Herodevices
{
    [SerializeField] private int rewardCost = 50;//����J�[�h�𓾂邽�߂̃R�X�g
    [SerializeField] private int currentCost = 0; //���݂܂ł̗ތ^�R�X�g
    [SerializeField] private Transform hane;//find�g�����ŏ�����ݒ肵�Ă����Ɨǂ�����
    [SerializeField] private GameObject rewardCardPrefab;//�@��V�J�[�h��Prefab

    //[SerializeField] private Color startColor;
   // [SerializeField] private Color endColor;
   //�F�ς���ꍇ�͂���g���܂��傤

    public float rotationDuration;


    //�Ȃ񂩂��`device�̈ʒu����J�[�h��΂��������ǂ�����

    public override void Initialize()
    {
        
        
       currentCost = 0;
    }

    public void AddCost(int cost)
    {
        currentCost += cost;
        Debug.Log($"Cost added: {cost}. Current cost: {currentCost}/{rewardCost}");


        StartCoroutine(hanerotation());//�H����]���[�V����

        if (currentCost >= rewardCost)
        {
            GrantReward();
            currentCost = 0;//���Z�b�g


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
        float currentspeed =4f;//maxspeed����n�܂�,���X�ɉ����Ă���


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

            currentspeed = Mathf.Max(currentspeed, minspeed);//0�����̑��x���傫����
            //float rotation = Mathf.Lerp(0, 360, t);



            //�����ʒu�擾childTransform.localRotation; 
            //Quaternion ini= haneTransform.localRotation;

            //Debug.Log($"Parent object: {this.name}");
            //Debug.Log(hane);
            //Debug.Log(rotation);
            //forward=(0,0,1)
            hane.transform.Rotate(0,0, currentspeed + Time.deltaTime);
            //�F�ς�����
            //hane.image.olor = Color.Lerp(startColor, endColor, t);


            yield return null;
        }
    }





}
