using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//ARMA���񂪎����Ă��邹��Ղ����݂����ȃf�o�C�X�ɂ��Ă���X�N���v�g�ł�.
//�������R�X�g���g���ƃQ�[�W�����܂��Ă���,��V�J�[�h�����炦�܂�.
//�R�X�g���g�����тɂ���Ղ��������܂�.

public class ARMAdevice :Herodevices,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private int rewardCost = 50;//����J�[�h�𓾂邽�߂̃R�X�g
    [SerializeField] private int currentCost = 0; //���݂܂ł̗ތ^�R�X�g
    [SerializeField] private Transform hane;//find�g�����ŏ�����ݒ肵�Ă����Ɨǂ�����
    [SerializeField] private GameObject rewardCardPrefab;//�@��V�J�[�h��Prefab
    [SerializeField] private TextMeshProUGUI ARMAtext;//�ތ^�R�X�g��\�����܂�.

    [SerializeField] Slider slider;//�X���C�_�[�����Ă�������
    //[SerializeField] private Color startColor;
   // [SerializeField] private Color endColor;
   //�F�ς���ꍇ�͂���g���܂��傤

    public float rotationDuration;



    public override void Initialize()
    {
        

        currentCost = 0;
    }

    void Start()
    {
        ARMAtext.gameObject.SetActive(false);//������ԂŃe�L�X�g�͔�\���Ƃ��܂�.
        slider.value = 0;
    }

    //
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

        slider.value = (float)currentCost/(float)rewardCost;//�X���C�_�[�X�V
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
        float currentspeed =8f;//maxspeed����n�܂�,���X�ɉ����Ă���


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










    /// <summary>
    /// ///////////////////////////////////
    /// </summary>
    /// <param name="eventData"></param>
    //�������牺�̓}�E�X�������������̏����ł�.

    public void OnPointerEnter(PointerEventData eventData)
    {

        Debug.Log("ARMA�f�o�C�X��G��܂�����");
        UpdateARMAText();//�R�X�g�����A�b�v�f�[�g���܂�
        ARMAtext.gameObject.SetActive(true);//�e�L�X�g��\��
        
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
