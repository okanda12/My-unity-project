using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{


    [SerializeField] private BlockFilter blockFilter;//フェードのスクリプト

    //シングルトンにするための呪文
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
        blockFilter.StartFillingAnimation(); // アニメーション開始

        // アニメーションの完了まで待機 (必要に応じて秒数を変更)
        yield return new WaitForSeconds(1f);

        // シーン遷移
        SceneManager.LoadScene("BattleScene");
    }



}
