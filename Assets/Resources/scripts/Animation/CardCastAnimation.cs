using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCastAnimation : MonoBehaviour
{
    //�J�[�h���L���X�g���鎞�̃A�j���[�V�����ł�.
    //�~�j�I���ƃ}�W�b�N�ŕ���������

    //DropPlace����MinionCast���Ă�.
    //��]���Ȃ���t�B�[���h�ɏo������

    public float CastedDuration;




    private Transform rectTransform;
    float rotation = 1f;//��]��


    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }


    //��]���Ȃ���}�E�X�̈ʒu�����ɏo�܂�
    public IEnumerator MinionCast()
    {
        float elapsedTime = 0f;
        Vector3 EndPosition;
        
        

        Vector3 StartPosition = Input.mousePosition;//�}�E�X�̈ʒu���擾���܂�

        string parent = this.transform.parent.name;

        //
        if (parent=="PlayerHand")
        {
            EndPosition = GameManager.Instance.PlayerHandTransform.position;
        }
        else
        {
            EndPosition = GameManager.Instance.EnemyHandTransform.position;
        }
        


        while (elapsedTime < CastedDuration)
        {
            elapsedTime += Time.deltaTime;

            //���ړ]����悤��
            rotation=Mathf.Sin(2*Mathf.PI*  (elapsedTime/CastedDuration));

            rectTransform.rotation = Quaternion.Euler(0, rotation, 0);
            rectTransform.position = Vector3.Lerp(StartPosition, EndPosition, rotation);

            yield return null;

        }


        yield return new WaitForSeconds(CastedDuration);





    }




}
