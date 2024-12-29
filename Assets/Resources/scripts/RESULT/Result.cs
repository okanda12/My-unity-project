using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI WinnerName;

    // Start is called before the first frame update
    private void Start()
    {
        
        WinnerName.text=$"{BattleManager.Instance.Winner}";



    }


}
