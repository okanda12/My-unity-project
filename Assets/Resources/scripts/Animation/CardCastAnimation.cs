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

            float rotation = Mathf.Lerp(0, 4 * 360, 4 * t);//4��]

     
            

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

            Debug.Log(canvasGroup.alpha);
            yield return null;

        }
        


        

        GameObject twinkle = Instantiate(twinklePrefab, BattleManager.Instance.MagiCasPlace);


        IgnoreLayout(BattleManager.Instance.EnemyHandTransform, false);

        
        rectTransform.localPosition = Vector3.zero;//�Ȃ񂩂��ꖳ���ƈ��肵�Ă���Ȃ�....


        //yield return new WaitForSeconds(CastDuration);

        cardcon.model.BattleCry(cardcon, BattleManager.Instance.PlayerHerocon);

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

        
        //�}�E�X�̈ʒu�̓��[���h���W�Ȃ̂Ń��[�J���ɕς���K�v������D

        Debug.Log($"defaultparent={defaultParent.name}");

        //�����̃^�[�����ۂ��ɂ���Ĕ��f����.
        //�{���͐e�Ŕ��f���������ǂ��񂾂낤����Drag���͐e��canvas�ɂȂ����Ⴄ����....

        bool isPlayerTurn = BattleManager.Instance.isPlayerTurn;

        
        //
        if (isPlayerTurn==true)
        {
            StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            EndPosition = defaultParent.position;
            //Debug.Log("casting rotating2");
        }
        else//���肪��������Ȃ�
        {
            IgnoreLayout(BattleManager.Instance.EnemyHandTransform, true);
            StartPosition = BattleManager.Instance.EnemyHandTransform.position;
            EndPosition = defaultParent.position;
            //Debug.Log("casting rotating3");
        }

        //Debug.Log("casting rotating4");
        //Debug.Log($"Start={StartPosition},End={EndPosition}");


        while (elapsedTime < CastDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / CastDuration;

            float rotation=Mathf.Lerp(0, 4*360, 4*t);//4��]

            //���ړ]����悤��


            //Debug.Log(rotation);

            rectTransform.rotation = Quaternion.Euler(0, rotation, 0);
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition,1.5f*t);
            //Debug.Log($"parent is {this.transform.parent}");

            yield return null;

        }

        IgnoreLayout(BattleManager.Instance.EnemyHandTransform, false);

        transform.SetParent(defaultParent, false);
        rectTransform.localPosition = Vector3.zero;//�Ȃ񂩂��ꖳ���ƈ��肵�Ă���Ȃ�....


        //yield return new WaitForSeconds(CastDuration);

        cardcon.model.BattleCry(cardcon, BattleManager.Instance.PlayerHerocon);

        //Debug.Log("casting rotating5");




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
