using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Turndisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            // isPlayerTurn の状態に応じて Text を更新
            if (GameManager.Instance.isPlayerTurn)
            {
                turnText.text = "Your Turn";
            }
            else
            {
                turnText.text = "Enemy Turn";
            }
        }
    }
}
