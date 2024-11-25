using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//これは他のアニメーションにも使えそう
public class CardHighlightController : MonoBehaviour
{
    public GameObject whiteHighlightPrefab;
    public GameObject redHighlightPrefab;

    private GameObject currentHighlight;

    public void ShowWhiteHighlight()
    {
        RemoveHighlight();
        currentHighlight = Instantiate(whiteHighlightPrefab,transform);
        currentHighlight.transform.localPosition = Vector3.zero;//中心に配置
    }

    public void ShowRedHighlight()
    {
        RemoveHighlight();
        currentHighlight = Instantiate(redHighlightPrefab, transform);
        currentHighlight.transform.localPosition = Vector3.zero;//中心に配置
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
