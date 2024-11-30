using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Turndisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button turnButton;       // ターン切り替えボタン
    [SerializeField] private Image buttonImage;       // ボタンの画像（暗くするため）

    private Color activeColor = Color.white;          // ボタンが有効なときの色
    private Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // ボタンが無効なときの色

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            // isPlayerTurn の状態に応じて Text を更新
            if (GameManager.Instance.isPlayerTurn)
            {
                turnText.text = "Your Turn";
                EnableButton(true);
            }
            else
            {
                turnText.text = "Enemy Turn";
                EnableButton(false);
            }
        }
    }


    private void EnableButton(bool isEnabled)
    {
        turnButton.interactable = isEnabled;         // ボタンのクリック有効/無効
        buttonImage.color = isEnabled ? activeColor : inactiveColor; // 色の変更
    }
}
