using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCastAnimation : MonoBehaviour
{
    //�J�[�h���L���X�g���鎞�̃A�j���[�V�����ł�.
    //�~�j�I���ƃ}�W�b�N�ŕ���������

    //CardMovement ��onenddrag����MinionCast���Ă�.
    //��]���Ȃ���t�B�[���h�ɏo������

    public float CastDuration;
    public float CantCastDuration;




    private Transform rectTransform;
    


    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

 
    

    //��]���Ȃ���}�E�X�̈ʒu�����ɏo�܂�
    //endofdrag��defaultparent��ݒ肷��Ɠr����playerhand�ɂȂ��Ă��܂��̂ł����Őe��ݒ�
    public IEnumerator MinionCast(Transform defaultParent)
    {
        float elapsedTime = 0f;
        Vector3 EndPosition;
        
        
        //�}�E�X�̈ʒu�̓��[���h���W�Ȃ̂Ń��[�J���ɕς���K�v������D
        Vector3 StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//�}�E�X�̈ʒu���擾���܂�

        //Debug.Log($"Start={StartPosition}");

        //�����̃^�[�����ۂ��ɂ���Ĕ��f����.
        //�{���͐e�Ŕ��f���������ǂ��񂾂낤����Drag���͐e��canvas�ɂȂ����Ⴄ����....

        bool isPlayerTurn = GameManager.Instance.isPlayerTurn;

        
        //
        if (isPlayerTurn==true)
        {
            EndPosition = GameManager.Instance.PlayerFieldTransform.position;
            //Debug.Log("casting rotating2");
        }
        else
        {
            EndPosition = GameManager.Instance.EnemyFieldTransform.position;
            //Debug.Log("casting rotating3");
        }

        //Debug.Log("casting rotating4");
        //Debug.Log($"Start={StartPosition},End={EndPosition}");


        while (elapsedTime < CastDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / CastDuration;

            float rotation=Mathf.Lerp(0, 720, 4*t);//���]

            //���ړ]����悤��


            //Debug.Log(rotation);

            rectTransform.rotation = Quaternion.Euler(0, rotation, 0);
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition,1.5f*t);
            //Debug.Log($"parent is {this.transform.parent}");

            yield return null;

        }
        transform.SetParent(defaultParent, false);

        

        //yield return new WaitForSeconds(CastDuration);


        
        //Debug.Log("casting rotating5");

     


    }

    public IEnumerator cantMinionCast(Transform defaultParent)
    {
        float elapsedTime = 0f;
        Vector3 EndPosition=defaultParent.position;
        Vector3 StartPosition;


        //�}�E�X�̈ʒu�̓��[���h���W�Ȃ̂Ń��[�J���ɕς���K�v������D
        if (GameManager.Instance.isPlayerTurn==true)
        {
            StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//�}�E�X�̈ʒu���擾���܂�

        }
        else
        {

            StartPosition = GameManager.Instance.EnemyFieldTransform.transform.position;

        }

        //Debug.Log($"Start={StartPosition}");

        //�����̃^�[�����ۂ��ɂ���Ĕ��f����.
        //�{���͐e�Ŕ��f���������ǂ��񂾂낤����Drag���͐e��canvas�ɂȂ����Ⴄ����....

        bool isPlayerTurn = GameManager.Instance.isPlayerTurn;


        

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
