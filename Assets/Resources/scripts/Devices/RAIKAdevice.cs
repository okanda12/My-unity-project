using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using UnityEngine.EventSystems;


public class RAIKAdevice : Herodevices, IPointerEnterHandler, IPointerExitHandler
{


    
    [SerializeField] private int rewardCost ;//10�^�[���o�߂�����킽�������ȁ`
    [SerializeField] private int currentTurn = 0;//���݂܂ł̌o�߃^�[��
    [SerializeField] private Transform moontransform;//��]����I�u�W�F�N�g
    private Transform EnemyField;
    private Transform PlayerField;

    [SerializeField] private TextMeshProUGUI RAIKAtext;//�ތ^�^�[����\�����܂�.
    [SerializeField] Slider slider;


    [SerializeField] GameObject Moon;//��
    [SerializeField] Transform Moonpivot;//���̒��S



    //Activated�֘A
    [SerializeField] GameObject Activatedobject;
    [SerializeField] float ActivatedDuration;
    [SerializeField] GameObject cupin; //����҂�G�t�F�N�g


    [SerializeField] Image wakuimage;//�X�V,RAIKA�f�o�C�X�̘g���ς��܂�
    [SerializeField] Sprite RAIKA_Activated_sprite;//�X�V���sprite
    






    private bool Activated=false;//���ꂪtrue�ɂȂ�Ƒ���̏���ς��Ă��܂�
    private bool Activated_Anim = false;//���ꂪfalse�Ȃ�A�j���[�V�����J�n

    public float rotationDuration;

    private bool modemeflag = false;

    public override void Initialize()
    {
        currentTurn = 0;
    }
    public void Awake()
    {
        EnemyField=BattleManager.Instance.EnemyFieldTransform;
        PlayerField = BattleManager.Instance.PlayerFieldTransform;

        RAIKAtext.gameObject.SetActive(false);

        slider.value = 0;//�X���C�_�[�����l
    }


    //�����Ƀp�����^��S�ĕς���v���O�������ق����@

    public override void OnTurnStart() 
    {

        currentTurn += 1;

        if (currentTurn >= rewardCost)
        {
            Activated = true;

        }
        StartCoroutine(DeviceAnim());






        

    }


    public void ReverseStatsinField(Transform field)
    {
        //���ꂢ���ˁI�I�I


        foreach (Transform daiza in field)
        {

            //getcomponetinchildren�͍ŏ��̈���������Ă��Ȃ��̂Ń��X�g�ɂ���
            ReverseStatsAnim[] reverseAnims = daiza.GetComponentsInChildren<ReverseStatsAnim>();
            foreach (ReverseStatsAnim reverseAnim in reverseAnims)
            {
                StartCoroutine(reverseAnim.ReverseStats());
            }
            


        }

        /*
            foreach (Transform cardTransform in  field)
            {
                CardController cardController = cardTransform.GetComponent<CardController>();
                if (cardController != null)
                {
                    int originalAttack = cardController.model.at;
                    cardController.model.at = cardController.model.hp;
                    cardController.model.hp = originalAttack;

                cardController.view.Show(cardController.model);
                
                }



            }
        */

    }

    //�q�I�u�W�F�N�g�̌�����]���܂�
    private IEnumerator TurnMoon()
    {
        Debug.Log($"TURN MOOOn");
        float elapsedTime = 0f;
        Quaternion ini = moontransform.rotation;
        Quaternion end = Quaternion.Euler(0, 0, currentTurn * 180);
        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotationDuration;



            moontransform.rotation = Quaternion.Lerp(ini, end, t);


            yield return null;
        }
    }




    private IEnumerator DeviceAnim()
    {
        
        float elapsedTime = 0f;
       
        Quaternion endplus = Quaternion.Euler(0, 0,( currentTurn * 180)+10);


        
        //�A�N�e�B���F�[�V����
        if (Activated ==true && Activated_Anim==false)
        {

            
            
            Activated_Anim = true;

            yield return StartCoroutine(ActivatedAnim());

            

        }

        StartCoroutine(TurnMoon());
        


        slider.value = (float)currentTurn / (float)rewardCost;//�X���C�_�[�X�V
        //Debug.Log($"RAIKAs slider.value{slider.value}warizan{currentTurn / rewardCost}");




        if (Activated == false)
        {

            if (BattleManager.Instance.PlayerHERO == "RAIKA")
            {
                ReverseStatsinField(PlayerField);
            }
            else
            {
                ReverseStatsinField(EnemyField);
            }


        }
        else
        {   //activated��true�ɂ��鏈�����K�v
            ReverseStatsinField(PlayerField);
            ReverseStatsinField(EnemyField);

        }



    }



    //�A�N�e�B���F�C�e�b�h�A�j���V�����ł�
    public IEnumerator ActivatedAnim()
    {

        BattleManager.Instance.isAnimating = true;//���ꂪtrue���Ƒ���̍s�����~�܂�

        
        GameObject cupinobject=Instantiate(cupin, transform);
        yield return StartCoroutine(SpreadMoon());

        StartCoroutine(SpreadMoonRotate());

        Destroy(cupinobject);
        yield return new WaitForSeconds(1f);

        float t;
        float elapsedTime = 0f;
        GameObject RAIKA_modame=Instantiate(Activatedobject, null);

        Transform RAIKATransform = RAIKA_modame.transform.Find("RAIKA");

        Transform EYETransform = RAIKATransform.Find("RAIKA_EYE");
        //Debug.Log(EYETransform);

        Vector3 startPosition = RAIKATransform.localPosition;
        Vector3 startScale = RAIKATransform.localScale;



      
       
        Vector3 endPosition = RAIKATransform.localPosition + new Vector3(-1f,0,0);
        Vector3 endScale = RAIKATransform.localScale + new Vector3(0.2f, 0.2f, 0);

        Debug.Log($"RAIKA'sdevice   start{startPosition}, end{endPosition}");


        //////////////////////
        ///�ŏ��ɋ߂Â��Ă�����A�j���[�V����

        float ActivatedDuration_3 = ActivatedDuration / 3;

        while (elapsedTime<ActivatedDuration_3)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / ActivatedDuration;
            RAIKATransform.localPosition = startPosition + new Vector3(0.005f * Random.Range(-1f, 1f), 0.005f * Random.Range(-1f, 1f), 0);
            
            RAIKATransform.localScale= Vector3.Lerp(startScale, endScale, t);
            yield return null;

        }



        //�ς��Ə�����
        RAIKATransform.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);

        RAIKATransform.gameObject.SetActive(true);

        ///�Őڋ�

        //�����l�Đݒ�
        startScale = RAIKATransform.localScale+new Vector3(2.5f, 2.5f, 0);
        endScale =startScale+ new Vector3(0.3f, 0.3f, 0);
        
        elapsedTime = 0;

        //�������ɂ��炵�܂�
        RAIKATransform.localPosition=startPosition+new Vector3(0f, -1f, 0);
        startPosition = RAIKATransform.localPosition;

        float elapsedTime2 = 0f ;


        while (elapsedTime < ActivatedDuration_3)
        {

            elapsedTime += Time.deltaTime;
            elapsedTime2 += Time.deltaTime;

            t = elapsedTime / ActivatedDuration;


            
            if (elapsedTime2>=0.5f) {//�����ŕb�����߂�

                elapsedTime2 = 0f;
                StartCoroutine(EyeRotate(EYETransform, 0.4f));
                
  
            }

            RAIKATransform.localScale = Vector3.Lerp(startScale, endScale, t);

            RAIKATransform.localPosition = startPosition + new Vector3(0.005f * Random.Range(-1f, 1f), 0.005f * Random.Range(-1f, 1f), 0);
            
            yield return null;
        }


        Destroy(RAIKA_modame);

        yield return StartCoroutine(ConvergeMoon());
        



        modemeflag = true;//�߂Â��A�j���[�V�������I�����flag�ł�

        


        wakuimage.sprite = RAIKA_Activated_sprite;

        cupinobject = Instantiate(cupin, transform);

        

        elapsedTime = 0f;
        float spinDuration = 0.5f;
        while (elapsedTime<spinDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / spinDuration;

            this.transform.localRotation = Quaternion.Euler(0f, 360f*t, 0f );



        }

        this.transform.localRotation = Quaternion.Euler(0f,  0f, 0f);


        yield return new WaitForSeconds(1f);









        BattleManager.Instance.isAnimating = false;//���ꂪfalse���Ƒ��肪�s���ł���

        


    }







    //�ڂ���]����A�j���[�V�����ł�
    //EYEDuration �̂�����3��]�������D
    public IEnumerator EyeRotate(Transform EYETransform,float EYEDuration)
    {

        Vector3 startRotation = EYETransform.localEulerAngles;
        Vector3 endRotation = startRotation+new Vector3(0,0,90f);
        float elapsedTime = 0f;
        
        float t = 0f;

        float EYEDuration3 = EYEDuration / 3f;//EYEDuration��3������������

        for (int i = 0; i < 3;i++) {//�ڂ�90�h��]��3��J��Ԃ��܂�
            while (elapsedTime < EYEDuration3)
            {
                elapsedTime += Time.deltaTime;
                t = elapsedTime / EYEDuration3;

                if (EYETransform != null)
                {

                    

                    EYETransform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, t);


                }
                yield return null;
            }
            //Debug.Log(i);
            startRotation = EYETransform.localEulerAngles;
            endRotation = startRotation + new Vector3(0, 0, 90f);

        }



    }

    public IEnumerator SpreadMoon()
    {

        Vector3 startScale = Moonpivot.localScale;
        Debug.Log($"Moonpivot{startScale}");
        float r1 = 300f;
        float r2 = 500f;

        int Moon1 = 15;
        int Moon2 = 15;

        //Moonpivot�𒆐S�ɉ~�`�ɔz�u

        Moonpivot.gameObject.SetActive(false);


        //����
        for (int i=0; i<Moon1;i++ )
        {
            float angle = i * Mathf.PI * 2f / Moon1;//���W�A�� 2��/moon1
            float x = Mathf.Cos(angle) * r1;
            float y = Mathf.Sin(angle) * r1;

            Vector3 localPosition = new Vector3(x, y, 0f);

            GameObject moonInstance = Instantiate(Moon, Moonpivot);
            moonInstance.transform.localPosition = localPosition;
           
        }

        //�O��
        for (int i=0;i<Moon2;i++)
        {
            float angle = i * Mathf.PI * 2f / Moon2;//���W�A�� 2��/moon1
            float x = Mathf.Cos(angle) * r2;
            float y = Mathf.Sin(angle) * r2;

            Vector3 localPosition = new Vector3(x, y, 0f);

            GameObject moonInstance = Instantiate(Moon, Moonpivot);

            Image moonImage = moonInstance.GetComponent<Image>();


            moonInstance.transform.localPosition = localPosition;
            moonImage.color = new Color(0.98f, 0.35f, 0.94f, 1.0f); // �ԐF�ɕύX


        }

        yield return new WaitForSeconds(0.5f);




        //���X�ɐe�I�u�W�F�N�g���g�債�Ă���
        Moonpivot.gameObject.SetActive(true);

        float elapsedTime = 0f;
        float t = 0f;
        float MoonSpreadDuration=0.5f;

        while (elapsedTime < MoonSpreadDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / MoonSpreadDuration;

            Moonpivot.localScale = Vector3.Lerp(startScale, new Vector3(1.3f, 1.3f, 1.3f), t*t);
            Moonpivot.localRotation = Quaternion.Euler(0f,0f,  360*t);

           yield return null;

        }


    }

    //�o���������������܂�
    public IEnumerator ConvergeMoon()
    {
        Vector3 startScale = Moonpivot.localScale;


        float elapsedTime = 0f;
        float t = 0f;
        float MoonSpreadDuration = 0.5f;

        while (elapsedTime < MoonSpreadDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / MoonSpreadDuration;

            Moonpivot.localScale = Vector3.Lerp(startScale, new Vector3(0.1f, 0.1f, 0.1f), t * t);
            Moonpivot.localRotation = Quaternion.Euler(0f, 0f, 360 * t);

            yield return null;

        }

        Moonpivot.gameObject.SetActive(false);



    }



    //�A�N�e�B���F�[�V������,�������L����܂�
    public IEnumerator SpreadMoonRotate()
    {
        Quaternion startRotate = Moonpivot.localRotation;
        float elapsedTime = 0f;

        while (modemeflag==false)
        {
            elapsedTime += Time.deltaTime;

            Moonpivot.localRotation = startRotate * Quaternion.Euler(0f, 0f,180*elapsedTime);


            yield return null;
        }



    }
    



    /// <summary>
    /// /////////////////�������牺�̓}�E�X�������������̏����ł�
    /// </summary>
    /// <param name="eventData"></param>

    public void OnPointerEnter(PointerEventData eventData)
    {

        Debug.Log("RAIKA�f�o�C�X��G��܂�����");
        UpdateRAIKAText();//�R�X�g�����A�b�v�f�[�g���܂�
        RAIKAtext.gameObject.SetActive(true);//�e�L�X�g��\��

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RAIKAtext.gameObject.SetActive(false);

    }

    private void UpdateRAIKAText()
    {
        RAIKAtext.text = $"RAIKA:{currentTurn}/{rewardCost}";
    }



}







