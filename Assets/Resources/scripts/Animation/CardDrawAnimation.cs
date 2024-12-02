using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardDrawAnimation : MonoBehaviour
{
    private Transform PlayerDeckTransform;//�f�b�L�̈ʒu
    private Transform PlayerHandTransform;//��D�̈ʒu
    private Transform EnemyDeckTransform;//�f�b�L�̈ʒu
    private Transform EnemyHandTransform;//��D�̈ʒu
    public float drawDuration = 0.1f;//�h���[�A�j���[�V�����̎���

    private RectTransform rectTransform;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        PlayerDeckTransform = BattleManager.Instance.PlayerDeckTransform;
        PlayerHandTransform = BattleManager.Instance.PlayerHandTransform;
        EnemyDeckTransform = BattleManager.Instance.EnemyDeckTransform;
        EnemyHandTransform = BattleManager.Instance.EnemyHandTransform;
    }
    
    public void DrawCard(Transform hand)
    {
        StartCoroutine(DrawCardCoroutine(hand));
    
    }

    public IEnumerator Drawsetparent(CardDrawAnimation cardAnim,Transform hand)
    {
        //�J�[�h�̃h���[�A�j���[�V���������s
        cardAnim.DrawCard(hand);

        //�A�j���[�V��������������܂ő��
        yield return new WaitForSeconds(cardAnim.drawDuration);

        //yield return StartCoroutine();
        //�A�j���[�V�����I�����,�J�[�h�̐e����D�ɕύX
        cardAnim.transform.SetParent(hand, false);



       
    }

    public IEnumerator DrawCardCoroutine(Transform hand)
    {
        float elapsedTime = 0f;
        //�J�[�h�̈ʒu���f�b�L�̈ʒu�ɐݒ�
        Vector3 startPosition = PlayerDeckTransform.position;
        Vector3 endPosition = PlayerHandTransform.position;

        if (hand == PlayerHandTransform)
        {
            startPosition = PlayerDeckTransform.position;
            endPosition = PlayerHandTransform.position;

        }
        else
        {
            startPosition = EnemyDeckTransform.position;
            endPosition = EnemyHandTransform.position;
        }


        

        Quaternion startRotation = Quaternion.Euler(0, 0, -45);
        Quaternion endRotation = Quaternion.identity;//����͖���]

        while (elapsedTime < drawDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / drawDuration;

            //�ʒu�Ɖ�]��⊮
            rectTransform.position = Vector3.Lerp(startPosition, endPosition, t);
            rectTransform.rotation = Quaternion.Lerp(startRotation, endRotation, t);

            yield return null;//1�t���[����~������.�b���w�肷��ɂ� yield return new wait forseconds
        }

        rectTransform.position = endPosition;
        rectTransform.rotation = endRotation;

        //Debug.Log("Card Draw Coroutine");




    }


}
