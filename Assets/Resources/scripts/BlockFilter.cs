using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


//シーンを切り替えるときのフェードアニメーションです
public class BlockFilter : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;//フェードするときのブロックprefabです.
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float spawnInterval;//ブロックが発生する時間です.
    [SerializeField] private int blockSize ;//ブロックのsize100*100??
    [SerializeField] private GameObject cutinobject;//ロード中に出てくる02アイコンを模擬しています


    [SerializeField] private float cutinDuration;
    private Transform maskTrans;//getcomponentで取ってきます,どうやってプレファブの子オブジェクトを持ってくる？？


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
    }


    void ActiveSceneChanged(Scene thisScene, Scene nextScene)
    {//シーンが切り替わった時に実行されます.
        Debug.Log(thisScene.name);
        Debug.Log(nextScene.name);
        nextsceneflag = true;
    }



    public void StartFillingAnimation()//外部から参照出来るアニメーション
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

        float screenWidth = canvasRect.rect.width;
        float screenHeight = canvasRect.rect.height;

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

        GameManager.Instance.sceneGO = true;
        isAnimating = false;

    }



    private void SpawnBlock(Vector2 position)//ブロックをポジションに
    {
        Debug.Log("spawn");
        GameObject block = Instantiate(blockPrefab, transform);
        Transform blockTransform = block.transform;

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

            Debug.Log($"startPosition{startPosition},endPosition{endPosition},maskTrans.localPosition{maskTrans.localPosition}");
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);

        //次のシーンが発動するまで待機
        while (nextsceneflag==false)
        {
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
