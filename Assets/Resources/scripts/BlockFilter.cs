using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


//シーンを切り替えるときのフェードアニメーションです
public class BlockFilter : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;//フェードするときのブロックprefabです.
   
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float spawnInterval;//ブロックが発生する時間です.
    [SerializeField] private int blockSize ;//ブロックのsize100*100??
    [SerializeField] private GameObject cutinobject;//ロード中に出てくる02アイコンを模擬しています


    [SerializeField] private float cutinDuration;

    [SerializeField] GameObject RAIKAhand;
    [SerializeField] GameObject ARMAhand;
    [SerializeField] GameObject AnimationMask;
    [SerializeField] GameObject Woodpivot;
    [SerializeField] GameObject WoodPrefab;


    private List<GameObject> woodInstances = new List<GameObject>();







    private Transform maskTrans;//getcomponentで取ってきます,どうやってプレファブの子オブジェクトを持ってくる？？
    private RectTransform canvasRect;//取ってきます.

    private bool isAnimating = false;//アニメーション中かどうか
    private bool nextsceneflag = false;

    private static BlockFilter instance;

    void Awake()
    {

        SceneManager.activeSceneChanged += ActiveSceneChanged;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 既にインスタンスが存在している場合は破棄
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        canvasRect=canvas.GetComponent<RectTransform>();

    }

    void ActiveSceneChanged(Scene thisScene, Scene nextScene)
    {//シーンが切り替わった時に実行されます.
        Debug.Log(thisScene.name);
        Debug.Log(nextScene.name);
        nextsceneflag = true;


    }


    private void Update()
    {
        Debug.Log(nextsceneflag);
    }


    /// <summary>
    /// /////////////////////////////
    /// </summary>
    //GameManager内でStartCoroutine()するとバグるのでここで行う．
    public void StartLoadScene1(string scene)
    {
        nextsceneflag = false;


        Debug.Log("loadTownScene");
        StartUniteAnimation(); // アニメーション開始
        //StartFillingAnimation()
        gameObject.SetActive(true);
        StartCoroutine(StartLoadScene2(scene));
        



    }


    public IEnumerator StartLoadScene2(string scene)
    {
        // アニメーションの完了まで待機 
        while (GameManager.Instance.sceneGO == false)
        {
            yield return null;
        }

        SceneManager.LoadScene(scene);


        //次のアニメーション用にリセット
        GameManager.Instance.sceneGO = false;
    }
    /// <summary>
    /// ///////////////////////////////
    /// </summary>


    //結束アニメーション
    public void StartUniteAnimation()
    {

        if (!isAnimating)
        {
            StartCoroutine(UniteAnimation());
        }



    }

    public IEnumerator UniteAnimation()
    {
        DontDestroyOnLoad(gameObject);
        nextsceneflag = false;
        isAnimating = true;
        bool handflag = false;

        float Duration = 0.5f;
        float elapsedTime = 0f;
        float t;
        Vector3 StartScale = new Vector3(1.1f,1.1f,1.1f);
        Vector3 endScale = new Vector3(0f, 0f, 0f);

        while (elapsedTime<Duration)
        {
            t = elapsedTime / Duration;
            elapsedTime += Time.deltaTime;
            AnimationMask.transform.localScale = Vector3.Lerp(StartScale, endScale, t);
            yield return null;

            //途中から手や木を発生させる
            if (elapsedTime>Duration/2f && handflag==false)
            {
                handflag = true;
                StartCoroutine(kirotation());
                //StartCoroutine(handrotation());
            }

        }
        AnimationMask.transform.localScale = endScale;
        GameManager.Instance.sceneGO = true;//これがtrueになるとsceneが進む
        yield return new WaitForSeconds(0.5f);



        while (nextsceneflag == false)
        {
            yield return null;
        }


        elapsedTime = 0f;
        Duration = 0.3f;
        
        while (elapsedTime < Duration)
        {
            t = elapsedTime / Duration;
            elapsedTime += Time.deltaTime;
            AnimationMask.transform.localScale = Vector3.Lerp(endScale, StartScale, t);
            yield return null;

        }
        AnimationMask.transform.localScale = StartScale;









        isAnimating = false;



    }

    //木が円形に配置され,回転
    public IEnumerator kirotation()
    {
        
        //円形に配置
        int woodNumber = 20;
        float r_wood = 900f;
       


        for (int i = 0; i < woodNumber; i++)
        {
            float angle = i * Mathf.PI * 2f / woodNumber;
            float x = Mathf.Cos(angle) * r_wood;
            float y = Mathf.Sin(angle) * r_wood;

            Vector3 localPosition = new Vector3(x, y, 0f);

            GameObject woodInstance = Instantiate(WoodPrefab,Woodpivot.transform);
            woodInstance.transform.localPosition = localPosition;


            // 中心（pivot）から木までの方向を計算
            Vector3 direction = (localPosition - Vector3.zero).normalized;

            // Z軸回転を計算して設定
            float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            woodInstance.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);



            //後で一気に破壊するため,リストに追加
            woodInstances.Add(woodInstance);
        }

        float elapsedTime = 0f;
        float Duration = 0.1f;
        float t;
        Vector3 startScale = Woodpivot.transform.localScale;
        Vector3 endScale = startScale*0.75f;

        while (elapsedTime<Duration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / Duration;

            Woodpivot.transform.localScale = Vector3.Lerp(startScale,endScale,t);
            yield return null;
        }
        Woodpivot.transform.localScale = endScale;



        float rotationSpeed = 3f;

        while (isAnimating == true)//アニメーション中は以下を続ける
        {

            Woodpivot.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

            yield return null;


        }


        foreach (GameObject woodInstance in woodInstances)
        {
            Destroy(woodInstance);
        }
        Woodpivot.transform.localScale = startScale;




    }

























    //ブロックを生成するフェードアニメーション

    public void StartFillingAnimation()
    {
        if (!isAnimating)//アニメーションが既に実行中でない場合のみ開始
        {
            StartCoroutine(FillScreenWithBlocks());
        }

       

    }


    private IEnumerator FillScreenWithBlocks()
    {

        Debug.Log("fillscreen");
        isAnimating = true;

        float screenWidth = 1280f;
       //anvasRect.rect.width;
        float screenHeight = 720f;

        //最小のせいすうを返す
        int rows = Mathf.CeilToInt(screenHeight / blockSize);
        int cols = Mathf.CeilToInt(screenWidth / blockSize);

        bool cutintag = false;
        



        for (int y=0;y<rows;y++)
        {

            if (y>rows/2 && cutintag==false)
            {
                StartCoroutine(cutin02());
                cutintag = true;
                Debug.Log("coutintag");
            }
            for (int x=0;x<cols;x++)
            {
                Vector2 position = new Vector2(x * blockSize-600, -y * blockSize +360);
                SpawnBlock(position);
                yield return new WaitForSeconds(spawnInterval);

    
            }

        }


        //これがfalseだとgamemanagerのロードされない．
        GameManager.Instance.sceneGO = true;
        isAnimating = false;

    }


    private int madara = 0;
    private void SpawnBlock(Vector2 position)//ブロックをポジションに
    {

        Debug.Log("spawn");
        GameObject block = Instantiate(blockPrefab, transform);
        Transform blockTransform = block.transform;

        if (madara%2 ==0)
        {
            SpriteRenderer spriteRenderer = block.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(0.03f, 0.7f, 0.9f);
           
            
        }
        madara += 1;



        blockTransform.localPosition = new Vector3(position.x- 0.5f, position.y+0.5f, 0);

        // サイズの設定
        blockTransform.localScale = new Vector3(blockSize, blockSize, 1);

        DontDestroyOnLoad(gameObject);

        StartCoroutine(FadeOutBlock(block.GetComponent<SpriteRenderer>()));

    }


    //ロード途中でカットインします
    private IEnumerator cutin02()
    {

        //prefabであるcutinの子オブジェクトにmaskがあります.

        GameObject cutin = Instantiate(cutinobject, transform);
        DontDestroyOnLoad(cutin);
        maskTrans =cutin.transform.Find("Sprite Mask");//マスクのTransformを使う

        
        Vector3 startPosition = maskTrans.localPosition;

        //ただ下に落ちていく.
        Vector3 endPosition= new Vector3(maskTrans.localPosition.x, 0, maskTrans.localPosition.z);
        //親オブジェクトをシーン中に破壊しなければ良い？


        //カットインの入りです

        float elapsedTime = 0f;
        float t;

        while (elapsedTime < cutinDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / cutinDuration;

            maskTrans.localPosition= Vector3.Lerp(startPosition,endPosition , t);

            //Debug.Log($"startPosition{startPosition},endPosition{endPosition},maskTrans.localPosition{maskTrans.localPosition}");
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);

        //次のシーンが発動するまで待機
        while (nextsceneflag==false)
        {
            Debug.Log("No");
            yield return null;
        }




        //カットインの帰りです

        elapsedTime = 0f;

        while (elapsedTime < cutinDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / cutinDuration;

            maskTrans.localPosition = Vector3.Lerp(endPosition, startPosition, t);
            Debug.Log($"startPosition{startPosition},endPosition{endPosition},maskTrans.localPosition{maskTrans.localPosition}");

            yield return null;
        }

        //Dontdestroyonload解除
        //SceneManager.MoveGameObjectToScene(cutin, SceneManager.GetActiveScene());
        //Debug.Log("Destroying cutin...");
        Destroy(cutin);
       


    }

    //粒がフェードアウトするやつです
    private IEnumerator FadeOutBlock(SpriteRenderer renderer)
    {
        Debug.Log("Fadeout");
        Color originalColor = renderer.color;
        float timer = 0;

        while (timer <fadeDuration)
        {
            timer += Time.deltaTime;
            //float alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            //renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b,alpha);

            
            yield return null;

        }

        Destroy(renderer.gameObject);


       


    }
















}
