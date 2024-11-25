using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardAttackAnimation : MonoBehaviour
{
    
    //public Transform targetTransform;//�^�[�Q�b�g

    public float AttackDurationGO;//�s���̎���
    public float AttackDurationBack;//�߂�̎���

    public float CantAttackDuration;//�U���ł��Ȃ�

    private Transform rectTransform;


    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();

    }

   


    //}


    //�J�[�h�ɓ��������邽��,Horizontal Layout����u���������ɂ��܂�.
    public void IgnoreLayout(Transform from,  bool ignore)
    {
        LayoutElement layoutElement = from.GetComponent<LayoutElement>();

        if (layoutElement != null)//����Ȃ�
        {
            layoutElement.ignoreLayout = ignore;//���C�A�E�g�𖳎����邩�ݒ�
        }
    }

    //�U���ł��Ȃ��Ƃ�,�u���u���k���܂�.
    public IEnumerator CantAttack(Transform from)
    {
        float elapsedTime = 0f;

        //IgnoreLayout(from,true);�܂�holizontal�͉�]�𐧖񂵂Ȃ��̂ł��Ȃ��Ă�������


        Quaternion startRotation = from.GetComponent<Transform>().localRotation;



        while (elapsedTime < CantAttackDuration)
        {

            
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / CantAttackDuration;
            float rotationOffset = 4* Mathf.Sin(t);
            //�ʒu�Ɖ�]��⊮
            //Debug.Log(rotationOffset);

            rectTransform.rotation = Quaternion.Euler(0, 0,rotationOffset);


            yield return null;

        }

        rectTransform.rotation = startRotation;




        //IgnoreLayout(from, false);

        yield return new WaitForSeconds(CantAttackDuration);




    }



    //�U�����[�V�����ł�.�����̈ʒu(from)���瑊��̈ʒu(target)�֍s���Ė߂�܂�

    public IEnumerator AttackAnim(Transform from,Transform target)
    {
        
        float elapsedTime = 0f;


        IgnoreLayout(from,true);

        Vector3 startPosition = from.GetComponent<Transform>().position;
        Vector3 endPosition = target.GetComponent<Transform>().position;

        Debug.Log($"startPosition:{startPosition},endPosition:{endPosition}");


        //identityEuler(0, 0, -45)
        Quaternion startRotation = Quaternion.identity;//����͖���]
        Quaternion endRotation = Quaternion.Euler(0,0,-45);

        while (elapsedTime <AttackDurationGO)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / AttackDurationGO;

            //�ʒu�Ɖ�]��⊮
            rectTransform.position = Vector3.Lerp(startPosition,endPosition,t);
            rectTransform.rotation = Quaternion.Lerp(startRotation, endRotation, t);


            yield return null;

        }


        elapsedTime = 0f;

        while (elapsedTime < AttackDurationBack)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / AttackDurationBack;

            //�ʒu�Ɖ�]��⊮
            rectTransform.position = Vector3.Lerp(endPosition, startPosition, t);
            rectTransform.rotation = Quaternion.Lerp(endRotation, startRotation, t);


            yield return null;

        }



        rectTransform.position = startPosition;
        rectTransform.rotation = startRotation;

        IgnoreLayout(from,false);

        yield return new WaitForSeconds(AttackDurationGO+AttackDurationBack);


    }
}
