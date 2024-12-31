using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardHoverZoom : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{


    public float zoomScale = 2f;//�g�嗦
    public float zoomDuration = 0.2f;//�g��ɂ����鎞��

    

    private Vector3 originalScale;//���̃X�P�[�����L�^

    private void Start()
    {
        //�����X�P�[����ۑ�;
        originalScale = transform.localScale;

    }

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.transform.parent ==BattleManager.Instance.PlayerHandTransform )
        {
            //IgnoreLayout(true);
            //�������Ă����ƌ�X���Q���肻������
            //StopAllCoroutines();
            StartCoroutine(ZoomCard(originalScale * zoomScale));
            //Debug.Log("cardZoomEnter");

        }


    }


    public void OnPointerExit(PointerEventData eventData)
    {
        //IgnoreLayout(false);
        //StopAllCoroutines();
        StartCoroutine(ZoomCard(originalScale));
       //Debug.Log("cardZoomExit");
    }



    private System.Collections.IEnumerator ZoomCard(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;


        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / zoomDuration);
            //Debug.Log(transform.localScale);
            yield return null;

        }

        transform.localScale = targetScale;
    }

}
