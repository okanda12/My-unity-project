using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCastAnimation : MonoBehaviour
{
    //�J�[�h���L���X�g���鎞�̃A�j���[�V�����ł�.
    //�~�j�I���ƃ}�W�b�N�ŕ���������

    //CardMovement ��onenddrag����MinionCast���Ă�.
    //��]���Ȃ���t�B�[���h�ɏo������

    public float CastDuration;
    public float CantCastDuration;
    public float MagicCastDuration;
    public float MagicbanishDuration;
    CardController cardcon;
    public GameObject twinklePrefab;//���@���L���X�g�����Ƃ��̂܂�����

    private Transform rectTransform;
    private CanvasGroup canvasGroup;


    [SerializeField] GameObject RotateEffect;//��]����Ƃ��ɂł�G�t�F�N�g�ł�




    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
        cardcon = this.GetComponent<CardController>();
        canvasGroup  = GetComponent<CanvasGroup>();
    }


    public void IgnoreLayout(Transform from, bool ignore)
    {
        LayoutElement layoutElement = from.GetComponent<LayoutElement>();

        if (layoutElement != null)//����Ȃ�
        {
            layoutElement.ignoreLayout = ignore;//���C�A�E�g�𖳎����邩�ݒ�
        }
    }


    public IEnumerator MagicCast(Transform defaultParent)
    {
        float t;
        float elapsedTime = 0f;
        Vector3 EndPosition = BattleManager.Instance.MagiCasPlace.transform.position;
        Vector3 StartPosition;//�}�E�X�̈ʒu���擾���܂�


        //�}�E�X�̈ʒu�̓��[���h���W�Ȃ̂Ń��[�J���ɕς���K�v������D

        //Debug.Log($"defaultparent={defaultParent.name}");

        
        bool isPlayerTurn = BattleManager.Instance.isPlayerTurn;


        //
        if (isPlayerTurn == true)
        {
            StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            //Debug.Log("casting rotating2");
        }
        else//���肪��������Ȃ�
        {
            IgnoreLayout(BattleManager.Instance.EnemyHandTransform, true);
            StartPosition = BattleManager.Instance.EnemyHandTransform.position;    
            //Debug.Log("casting rotating3");
        }

        //Debug.Log("casting rotating4");
        //Debug.Log($"Start={StartPosition},End={EndPosition}");


        while (elapsedTime < MagicCastDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / CastDuration;

            float rotation = Mathf.Lerp(0,  360, 4 * t);//4��]

     
            

            rectTransform.rotation = Quaternion.Euler(0, rotation, 0);
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition,  4* t);
            //Debug.Log($"parent is {this.transform.parent}");

            yield return null;

        }


        transform.SetParent(BattleManager.Instance.MagiCasPlace, true);//magicasplace�ɂ���
       
        elapsedTime = 0f;
        while (elapsedTime < MagicbanishDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / MagicbanishDuration;

            canvasGroup.alpha = 1f -   t;

            //Debug.Log(canvasGroup.alpha);
            yield return null;

        }
        


        

        GameObject twinkle = Instantiate(twinklePrefab, BattleManager.Instance.MagiCasPlace);


        IgnoreLayout(BattleManager.Instance.EnemyHandTransform, false);

        
        rectTransform.localPosition = Vector3.zero;//�Ȃ񂩂��ꖳ���ƈ��肵�Ă���Ȃ�....


        //yield return new WaitForSeconds(CastDuration);
        if (BattleManager.Instance.isPlayerTurn == false)//����^�[���Ȃ�
        {
            cardcon.model.BattleCry(cardcon, BattleManager.Instance.EnemyHerocon);
        }
        else
        {
            cardcon.model.BattleCry(cardcon, BattleManager.Instance.PlayerHerocon);
        }
        

        yield return new WaitForSeconds(0.5f);


        Destroy(twinkle);
        Destroy(gameObject);//�������ɃA�j���[�V�����ɂ�������
        //Debug.Log("casting rotating5");



    }

    


    //��]���Ȃ���}�E�X�̈ʒu�����ɏo�܂�
    //endofdrag��defaultparent��ݒ肷��Ɠr����playerhand�ɂȂ��Ă��܂��̂ł����Őe��ݒ�
    public IEnumerator MinionCast(Transform defaultParent)
    {
        float elapsedTime = 0f;
        Vector3 EndPosition=defaultParent.position;
        Vector3 StartPosition;//�}�E�X�̈ʒu���擾���܂�

        Vector3 StartScale=this.transform.localScale;
        Vector3 EndScale = StartScale * 1.2f ;



        //IgnoreLayout(BattleManager.Instance.EnemyHandTransform, false);

        //�}�E�X�̈ʒu�̓��[���h���W�Ȃ̂Ń��[�J���ɕς���K�v������D

        //Debug.Log($"defaultparent={defaultParent.name}");

        //�����̃^�[�����ۂ��ɂ���Ĕ��f����.
        //�{���͐e�Ŕ��f���������ǂ��񂾂낤����Drag���͐e��canvas�ɂȂ����Ⴄ����....

        bool isPlayerTurn = BattleManager.Instance.isPlayerTurn;

        
        //�n�܂�ƏI����ݒ肵�܂�
        if (isPlayerTurn==true)
        {

            StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            EndPosition = defaultParent.position;
            //Debug.Log("casting rotating2");
        }
        else//���肪��������Ȃ�
        {
            //�n���h�̃��C�A�E�g�͖������܂�
            IgnoreLayout(BattleManager.Instance.EnemyHandTransform, true);
            StartPosition = BattleManager.Instance.EnemyHandTransform.position;
            EndPosition = defaultParent.position;
            //Debug.Log("casting rotating3");
        }

        StartScale = this.transform.localScale;
        EndScale = StartScale*1.3f;


        //Debug.Log("casting rotating4");
        //Debug.Log($"Start={StartPosition},End={EndPosition}");

        //�J�[�h���o���܂�

        yield return StartCoroutine(CardRotating(StartPosition, EndPosition,StartScale,EndScale));
       



        transform.SetParent(defaultParent, false);





        rectTransform.localPosition = Vector3.zero;//�Ȃ񂩂��ꖳ���ƈ��肵�Ă���Ȃ�....
        //��ɏo�������ɂ�����Ǝז��Ȃ̂Ŗ��O�������܂�
        cardcon.view.Show(cardcon.model);

        //yield return new WaitForSeconds(CastDuration);

       

        cardcon.model.BattleCry(cardcon, BattleManager.Instance.PlayerHerocon);

        //Debug.Log("casting rotating5");

        yield return null;




    }


    //�J�[�h����]����,�����Ȃ������ɐݒu���܂��D

    public IEnumerator CardRotating(Vector3 StartPosition,Vector3 EndPosition, Vector3 StartScale, Vector3 EndScale)
    {
        float elapsedTime = 0f;
        float elapsedTime2 = 0f;//�g��k���Ŏg���܂�
        float t = 0f;

        //��]�G�t�F�N�g���o���܂�.
        GameObject rotateEffect = Instantiate(RotateEffect, transform);

        //��]��,�g�傷��
        float go = 0.5f*CastDuration;
        while (elapsedTime < go)//�r���܂�
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / CastDuration;

            float rotation = Mathf.Lerp(0,  7*360, t);//4��]

          

            //Debug.Log(rotation);

            this.transform.localScale = Vector3.Lerp(StartScale, EndScale, t);
            rectTransform.rotation = Quaternion.Euler(0, rotation, 0);
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition,  t);
            //Debug.Log($"parent is {this.transform.parent}");

            yield return null;

        }




        //��u�~�܂��ă~�Z��
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);
        EndScale = this.transform.localScale;
        Vector3 EndScale2 = 1.08f * EndScale;
        elapsedTime = 0;
        float stopDuration = 0.2f;//��b���炢�H
        while (elapsedTime < stopDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / stopDuration;

            //float rotation = Mathf.Lerp(0, 7 * 360, t);//4��]

            //���ړ]����悤��


            //Debug.Log(rotation);
            this.transform.localScale = Vector3.Lerp(EndScale, EndScale2, t);
            
            yield return null;

        }


        Destroy(rotateEffect);
        //��C�ɑ���Ɏ��܂�

        //���݂�transform
        StartPosition = this.transform.position;


        elapsedTime = 0;
        float daizaDuration = 0.1f;
        
        while (elapsedTime < daizaDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / daizaDuration;

          

            //Debug.Log(rotation);
            this.transform.localScale = Vector3.Lerp(EndScale2, StartScale, t);
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition, t);


            yield return null;

        }







        
       
        IgnoreLayout(BattleManager.Instance.EnemyHandTransform, false);
       
    }

    public IEnumerator cantMinionCast(Transform defaultParent)
    {//�n���h����~�j�I�����o���Ȃ����̏����ł�.
        float elapsedTime = 0f;
        Vector3 EndPosition=defaultParent.position;
        Vector3 StartPosition;
        

        //�}�E�X�̈ʒu�̓��[���h���W�Ȃ̂Ń��[�J���ɕς���K�v������D
        if (BattleManager.Instance.isPlayerTurn==true)
        {
            StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//�}�E�X�̈ʒu���擾���܂�

        }
        else
        {

            StartPosition = BattleManager.Instance.EnemyFieldTransform.transform.position;

        }

        //Debug.Log($"Start={StartPosition}");

        //�����̃^�[�����ۂ��ɂ���Ĕ��f����.
        //�{���͐e�Ŕ��f���������ǂ��񂾂낤����Drag���͐e��canvas�ɂȂ����Ⴄ����....

        bool isPlayerTurn = BattleManager.Instance.isPlayerTurn;


        

        //Debug.Log("casting rotating4");
        //Debug.Log($"Start={StartPosition},End={EndPosition}");


        //��]�����f���ɖ߂�܂�
        while (elapsedTime < CantCastDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / CantCastDuration;
            
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition, 2* t);
            //Debug.Log($"parent is {this.transform.parent}");

            yield return null;

        }


        transform.SetParent(defaultParent, false);
        yield return new WaitForSeconds(CantCastDuration);

       
        //Debug.Log("casting rotating5");




    }






}
