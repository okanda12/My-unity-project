using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{


    //[SerializeField] private GameObject Fade;//フェードのプレファブ
    private GameObject FadeObject;
    private BlockFilter FadeObject_script;


    //[SerializeField] private BlockFilter blockFilter;//フェードのスクリプト

    //シングルトンにするための呪文
    public static GameManager Instance { get; private set; }

    public bool sceneGO = false;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //gameObject.SetActive(true);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("Duplicate GameManager detected, destroying this instance.");
            Destroy(gameObject); // 重複するインスタンスを破棄
        }
        
        
    }

    private void Start()
    {

        FadeObject = GameObject.Find("Fade");
        FadeObject_script = FadeObject.GetComponent<BlockFilter>();


        if (FadeObject_script)
        {
            Debug.Log("fadeobject finded!");
            //StartCoroutine(PlayAnimation_LoadTownScene());

        }
        else if(!FadeObject_script)
        {
            Debug.Log("cantfind fadeobject");
        }


    }

    private void Update()
    {
        








    }


    //街画面へ移る
    public void ToTown()
    {
        FadeObject = GameObject.Find("Fade");
        //FadeObject = Fade;
        FadeObject_script = FadeObject.GetComponent<BlockFilter>();

       

        if (FadeObject_script)
        {
            Debug.Log("Starting Coroutine...");
            FadeObject_script.StartLoadScene1("TownScene");

        }
        else
        {
            Debug.LogError("FadeObject_script not found or null.");
        }
    

    }



    //バトル画面へ移る
    public void ToBattle()
    {
        FadeObject = GameObject.Find("Fade"); 
        //FadeObject = Fade;
        FadeObject_script = FadeObject.GetComponent<BlockFilter>();


        if (FadeObject_script)
        {
            Debug.Log("Starting Coroutine...");
            FadeObject_script.StartLoadScene1("BattleScene");

        }
        else
        {
            Debug.LogError("FadeObject_script not found or null.");
        }


    }



   


}
