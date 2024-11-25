using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class HeartAnimation : MonoBehaviour
{
    public GameObject heartPrefab; //�n�[�g�̃v���t�@�u
    public int heartCount ;//��΂��n�[�g�̐�
    public float animationDuration ; //�A�j���[�V�����̌p������
    public float radius ;//�n�[�g����Ԕ��a

    public GameObject hearttextprefab;//�n�[�g�ʒu�ɂۂ��[��ĂȂ���
    public float hearttextDuration;//�ۂ��[�񎞊�

    //�񕜃A�j���[�V�����ł���܂�

    public void PlayHealingAnimation(Transform targetTransform,int healAmount)
    {
        StartCoroutine(HealingAnimation(targetTransform,healAmount));
    }

    private IEnumerator HealingAnimation(Transform targetTransform, int healAmount)
    {
        
        float elapsedTime = 0f;
        Transform HeartPosition = transform.Find("Heart");//�n�[�g�̈ʒu�����΂�
        
        Vector3 startPosition = HeartPosition.position;
        Vector3 endPosition = startPosition + new Vector3(0, 0.1f, 0); 

        TextMeshProUGUI hearttext = hearttextprefab.GetComponentInChildren<TextMeshProUGUI>();
        //�q�I�u�W�F�N�g����T��

        hearttext.text = healAmount.ToString();

        GameObject hearttextobject=Instantiate(hearttextprefab,HeartPosition,false);




        while (elapsedTime<hearttextDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / hearttextDuration;
            hearttextobject.transform.position = Vector3.Lerp(startPosition, endPosition, t * t);

            yield return null;
        }

        Destroy(hearttextobject);



        //GameObject heart1 = Instantiate(heartPrefab, targetTransform.position, Quaternion.identity,targetTransform);
        for (int i=0;i<heartCount;i++)
        {
            //�n�[�g�𐶐�
            //�Ȃ񂩍Ō�̈����Őe��ݒ肵�Ȃ��Ƃ��܂������Ȃ��݂���
            GameObject heart = Instantiate(heartPrefab,targetTransform.position,Quaternion.identity, targetTransform);

            //�����_���ȕ����ɔ�΂�
            Vector2 randomDirection = Random.insideUnitCircle.normalized * radius;
            Vector3 targetPosition = targetTransform.position + new Vector3(randomDirection.x, randomDirection.y, 0);

            //�n�[�g�𓮂���
            StartCoroutine(MoveHeart(heart.transform, targetPosition, animationDuration));

            yield return new WaitForSeconds(0.1f);
            //������Ƒ҂��Ď��̃n�[�g�𓮂���
        
        }


        
    }


    //�n�[�g�ЂƂЂƂɓ�����^����
    private IEnumerator MoveHeart(Transform heart,Vector3 targetPosition,float duration)
    {
        Vector3 startPosition = heart.position;
        float elapsedTime = 0f;

        //Debug.Log($"Heart Position: {startPosition}, Target Position: {targetPosition}");



        while (elapsedTime<duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            //�ɂ₩�ɓ���
            heart.position = Vector3.Lerp(startPosition,targetPosition,t*t);
            //Debug.Log(heart.position);

            //Debug.Log($"radius: {Vector3.Distance(startPosition, heart.position)}");


            yield return null;
        }
        

        Destroy(heart.gameObject);


    }



    
}
