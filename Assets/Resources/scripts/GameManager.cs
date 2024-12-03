using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{


    [SerializeField] private BlockFilter blockFilter;//�t�F�[�h�̃X�N���v�g

    //�V���O���g���ɂ��邽�߂̎���
    public static GameManager Instance { get; private set; }

    private void Awake()
    {

        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }
    //

    public void ToSelectionPhase()
    {
        if (blockFilter != null)
        {
            blockFilter.StartFillingAnimation();
            StartCoroutine(PlayAnimationAndLoadScene());
        }
        else
        {
            Debug.Log("cantfind clockfilter ");
        }

       // SceneManager.LoadScene("BattleScene");

        
    }


    private IEnumerator PlayAnimationAndLoadScene()
    {
        blockFilter.StartFillingAnimation(); // �A�j���[�V�����J�n

        // �A�j���[�V�����̊����܂őҋ@ (�K�v�ɉ����ĕb����ύX)
        yield return new WaitForSeconds(1f);

        // �V�[���J��
        SceneManager.LoadScene("BattleScene");
    }



}
