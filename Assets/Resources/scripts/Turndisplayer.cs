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
            // isPlayerTurn ‚Ìó‘Ô‚É‰‚¶‚Ä Text ‚ğXV
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
