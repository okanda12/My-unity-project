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
    [SerializeField] private int currentCost = 0; //���݂܂ł̗݌v�R�X�g
    [SerializeField] private Transform hane;//�J�[�h���g���Ɖ�]���镔��
    [SerializeField] private GameObject rewardCardPrefab;//�@��V�J�[�h��Prefab
    [SerializeField] private TextMeshProUGUI ARMAtext;//�ތ^�R�X�g��\�����܂�.
    [SerializeField] Slider slider;//ARMA�f�o�C�X�̉��̃X���C�_�[

    //Activated�֘A
    private bool Activated_Anim = false;//���ꂪfalse�̂܂܃R�X�g���B����ƃA�j���[�V�����J�n
    private bool Animlast = false;//�A�j���̃��X�g�����o����t���b�O�ł�
    [SerializeField] GameObject cupin;//����҂�G�t�F�N�g
    [SerializeField] Transform Wing_pivot;//�H���̒��S
    [SerializeField] GameObject wing_prefab;//�H��

    [SerializeField] GameObject ARMA_device1;//�A�j���[�V�����̃f�o�C�X
    [SerializeField] GameObject ARMA_fade;//�t�F�[�h



    [SerializeField] Image wakuimage;//�X�V�f�o�C�X�̘g���ς��܂�
    [SerializeField] Image haneimage;//�X�V�f�o�C�X�̘g���ς��܂�
    [SerializeField] Image backimage;//�X�V�f�o�C�X�̘g���ς��܂�
    

    [SerializeField] Sprite ARMA_Activated_sprite_waku;//�X�V���sprite
    [SerializeField] Sprite ARMA_Activated_sprite_hane;//
    [SerializeField] Sprite ARMA_Activated_sprite_back;//

    public float slow = 1f;
    private bool cloudStop = false;
    public bool isARMAinCloud = false;

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

        if (currentCost >= rewardCost�@&& Activated_Anim==false)
        {

            StartCoroutine(ActivatedAnim());
           
            currentCost = 0;//���Z�b�g

            Activated_Anim = true;//true�ɂ��Ă����΃A�j���[�V�����͍Ăюn�܂�Ȃ�


        }

        slider.value = (float)currentCost/(float)rewardCost;//�X���C�_�[�X�V
        Debug.Log($"ARMA slider.value{slider.value}warizan{currentCost / rewardCost}");

    }


    public IEnumerator ActivatedAnim()
    {
        BattleManager.Instance.isAnimating = true;//���ꂪtrue���Ƒ���̍s�����~�܂�

        StartCoroutine(cupinAnim());//����҂��
        yield return new WaitForSeconds(0.5f);

       //�H�����L���悤!
       yield return StartCoroutine(SpreadWing()); 
        StartCoroutine(SpreadWingRotate());//�e�I�u�W�F�N�g����]�����܂�
        
        //�t�F�[�h�o��
        GameObject ARMA_fadeobject = Instantiate(ARMA_fade, null);
        //�f�o�C�X����,�A�j���[�V������ɏ�ɂт�`��
        yield return StartCoroutine(DeviceSoaring(ARMA_fadeobject));

        //�t�F�[�h���̃I�u�W�F�N�g��S�Ď擾
        Transform cloud_1_1 = ARMA_fadeobject.transform.Find("cloud1_1");
        Transform cloud_1_2 = ARMA_fadeobject.transform.Find("cloud1_2");
        Transform cloud_1_3 = ARMA_fadeobject.transform.Find("cloud1_3");


        Transform cloud_2_1 = ARMA_fadeobject.transform.Find("cloud2_1");
        Transform cloud_2_2 = ARMA_fadeobject.transform.Find("cloud2_2");
        Transform cloud_2_3 = ARMA_fadeobject.transform.Find("cloud2_3");

        Transform ARMA = ARMA_fadeobject.transform.Find("ARMA_INACTIVATE");
        Transform ARMA_device = ARMA_fadeobject.transform.Find("ARMA_device_INACTIVE");

        cloud_1_3.localPosition = new Vector3(-4f, 0.2f, 0);
        cloud_2_2.localPosition = new Vector3(-4f, 0.2f, 0);

        cloud_1_1.localPosition = new Vector3(-4f, -0.16f, 0);
        cloud_1_2.localPosition = new Vector3(-4f, -0.16f, 0);
        cloud_2_1.localPosition = new Vector3(-4f, -0.16f, 0);
        cloud_2_3.localPosition = new Vector3(-4f, -0.16f, 0);

        ARMA.localPosition = new Vector3(-4f, -0.35f, 0);
        ARMA_device.localPosition = new Vector3(+3f, -0f, 0);

        slow = 1.5f;
        StartCoroutine(Cloudflow(cloud_1_3, 3.5f));
        StartCoroutine(Cloudflow(cloud_2_2, 1f));

        StartCoroutine(Cloudflow(cloud_1_1,1f));
        StartCoroutine(Cloudflow(cloud_1_2, 2.5f));
        StartCoroutine(Cloudflow(cloud_2_1, 8f));
        StartCoroutine(Cloudflow(cloud_2_3, 3f));

        yield return new WaitForSeconds(1f);

       
        StartCoroutine(DEVICEin(ARMA_device));

        //yield return new WaitForSeconds(f);

        StartCoroutine(ARMAin(ARMA));
        slow = 0.1f;

        yield return new WaitForSeconds(1.2f);


        isARMAinCloud = true;
        yield return new WaitForSeconds(0.1f);

        Destroy(ARMA_fadeobject);
        yield return StartCoroutine(ConvergeWing());
        //yield return StartCoroutine();



        wakuimage.sprite=ARMA_Activated_sprite_waku;//�X�V�f�o�C�X�̘g���ς��܂�
        backimage.sprite = ARMA_Activated_sprite_back;
        haneimage.sprite = ARMA_Activated_sprite_hane;
        


        StartCoroutine(cupinAnim());//����҂��


        GrantReward();

        BattleManager.Instance.isAnimating = false;



    }

    public IEnumerator ConvergeWing()
    {
        Vector3 startScale = Wing_pivot.localScale;


        float elapsedTime = 0f;
        float t = 0f;
        float MoonSpreadDuration = 0.5f;

        while (elapsedTime < MoonSpreadDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / MoonSpreadDuration;

            Wing_pivot.localScale = Vector3.Lerp(startScale, new Vector3(0.1f, 0.1f, 0.1f), t * t);
            Wing_pivot.localRotation = Quaternion.Euler(0f, 0f, 360 * t);

            yield return null;

        }

        Wing_pivot.gameObject.SetActive(false);



    }
    public IEnumerator DEVICEin(Transform ARMA_device)
    {
        Vector3 startPosition = ARMA_device.localPosition;
        Vector3 endPosition = ARMA_device.localPosition;
        endPosition.x = -0.55f;

        float elapsedTime = 0f;
        float t = 0f;
        float ARMADuration = 0.2f;


        while (elapsedTime < ARMADuration)
        {
            t = elapsedTime / ARMADuration;

            elapsedTime += Time.deltaTime;


            ARMA_device.localPosition = Vector3.Lerp(startPosition, endPosition, t);



            yield return null;
        }

    
    }



    public IEnumerator ARMAin(Transform ARMA)
    {
        Vector3 startPosition = ARMA.localPosition;
        Vector3 endPosition = ARMA.localPosition;
        endPosition.x = 0.8f;
        
        float elapsedTime = 0f;
        float t = 0f;
        float ARMADuration = 0.5f ;


        while (elapsedTime < ARMADuration)
        {
            t = elapsedTime / ARMADuration;

            elapsedTime += Time.deltaTime;


            ARMA.localPosition = Vector3.Lerp(startPosition, endPosition, t*t);



            yield return null;
        }

        }


        public IEnumerator Cloudflow(Transform cloud, float cloudDuration)
    {
        
        Vector3 startPosition =cloud.localPosition;
        Vector3 endPosition = cloud.localPosition+new Vector3(8f,0,0);

        float elapsedTime = 0f;

        float t = 0f;

        

            while (cloud.localPosition.x < endPosition.x && !isARMAinCloud)
            {
                t = elapsedTime / cloudDuration;

                elapsedTime += Time.deltaTime * slow;


                cloud.localPosition = Vector3.Lerp(startPosition, endPosition, t);

                if (cloud.localPosition.x >= endPosition.x)
                {
                    elapsedTime = 0;
                    cloud.localPosition = startPosition;
                }


                yield return null;

            }
        
        

        
    }


    //����҂�A�j���[�V����!�o��������
    public IEnumerator cupinAnim()
    {
        GameObject cupinobject = Instantiate(cupin, transform);

        yield return new WaitForSeconds(1f);

        Destroy(cupinobject);


    }

    //�H�����L���悤�I
    public IEnumerator SpreadWing()
    {
        Vector3 startScale = Wing_pivot.localScale;
        Debug.Log("SpreadWing!!");

        float r1 = 300f;

        int wing_number = 20;//�ݒu����H���̌�

        //Wing_pivot�𒆐S�ɉ~�`�ɔz�u
        Wing_pivot.gameObject.SetActive(false);

        //�H����z�u����
        for(int i=0;i<wing_number;i++)
        {
            float angle = i * Mathf.PI * 2f / wing_number;
            float x = Mathf.Cos(angle) * r1;
            float y = Mathf.Sin(angle) * r1;

            Vector3 localPosition = new Vector3(x, y, 0f);

            GameObject wingInstance = Instantiate(wing_prefab, Wing_pivot);
            wingInstance.transform.localPosition = localPosition;

        }

        yield return new WaitForSeconds(0.1f);




        //���X�ɐe�I�u�W�F�N�g���g�債�Ă���
        Wing_pivot.gameObject.SetActive(true);

        float elapsedTime = 0f;
        float t = 0f;
        float MoonSpreadDuration = 0.5f;

        while (elapsedTime < MoonSpreadDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / MoonSpreadDuration;

            Wing_pivot.localScale = Vector3.Lerp(startScale, new Vector3(1.3f, 1.3f, 1.3f), t * t);
            Wing_pivot.localRotation = Quaternion.Euler(0f, 0f, 360 * t);

            yield return null;

        }




    }


    public IEnumerator SpreadWingRotate()
    {
        Quaternion startRotate = Wing_pivot.localRotation;
        float elapsedTime = 0f;

        while (Animlast == false)
        {
            elapsedTime += Time.deltaTime;

            Wing_pivot.localRotation = startRotate * Quaternion.Euler(0f, 0f, 180 * elapsedTime);


            yield return null;
        }




    }


    //ARMA_device1_Anim��������,�㏸���܂�.
    public IEnumerator DeviceSoaring(GameObject ARMA_fadeobject)
    {
        //�f�o�C�X����.�A�j���[�V����������ɉ��܂�.
        //�A�j���[�V�����́@�W�J������Ղ������(�������[�v)
        GameObject ARMA_device1_Anim = Instantiate(ARMA_device1, ARMA_fadeobject.transform);

        yield return new WaitForSeconds(2f);

        float elapsedTime = 0f;
        float soaringDuration = 2f;
        float t;

        Vector3 startPositon = ARMA_device1_Anim.transform.localPosition;
        Vector3 endPositon = startPositon + new Vector3(10f, 10f, 0);


        while (elapsedTime < soaringDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / soaringDuration;

            ARMA_device1_Anim.transform.position = Vector3.Lerp(startPositon, endPositon, 4*t*t);
            yield return null;

        }

        Destroy(ARMA_device1_Anim);





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
