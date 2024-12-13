using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�J�[�h�ɂ��Ă܂��D
//�n�C���C�g�𐶐�����X�N���v�g�ł�.
public class CardHighlightController : MonoBehaviour
{
    public GameObject whiteHighlightPrefab;
    public GameObject redHighlightPrefab;

    private GameObject currentHighlight;

    public void ShowWhiteHighlight()
    {
        RemoveHighlight();
        currentHighlight = Instantiate(whiteHighlightPrefab,transform);
        currentHighlight.transform.localPosition = Vector3.zero;//���S�ɔz�u
    }

    public void ShowRedHighlight()
    {
        RemoveHighlight();
        currentHighlight = Instantiate(redHighlightPrefab, transform);
        currentHighlight.transform.localPosition = Vector3.zero;//���S�ɔz�u
    }


    public void RemoveHighlight()
    {
        if(currentHighlight != null)
        {
            Destroy(currentHighlight);
            currentHighlight = null;
        }
    }
}
