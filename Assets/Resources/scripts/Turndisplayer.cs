using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Turndisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button turnButton;       // �^�[���؂�ւ��{�^��
    [SerializeField] private Image buttonImage;       // �{�^���̉摜�i�Â����邽�߁j

    private Color activeColor = Color.white;          // �{�^�����L���ȂƂ��̐F
    private Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // �{�^���������ȂƂ��̐F

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            // isPlayerTurn �̏�Ԃɉ����� Text ���X�V
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
        turnButton.interactable = isEnabled;         // �{�^���̃N���b�N�L��/����
        buttonImage.color = isEnabled ? activeColor : inactiveColor; // �F�̕ύX
    }
}
