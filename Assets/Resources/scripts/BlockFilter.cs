using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


//�V�[����؂�ւ���Ƃ��̃t�F�[�h�A�j���[�V�����ł�
public class BlockFilter : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;//�t�F�[�h����Ƃ��̃u���b�Nprefab�ł�.
   
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float spawnInterval;//�u���b�N���������鎞�Ԃł�.
    [SerializeField] private int blockSize ;//�u���b�N��size100*100??
    [SerializeField] private GameObject cutinobject;//���[�h���ɏo�Ă���02�A�C�R����͋[���Ă��܂�


    [SerializeField] private float cutinDuration;

    [SerializeField] GameObject RAIKAhand;
    [SerializeField] GameObject ARMAhand;
    [SerializeField] GameObject AnimationMask;
    [SerializeField] GameObject Woodpivot;
    [SerializeField] GameObject WoodPrefab;


    private List<GameObject> woodInstances = new List<GameObject>();







    private Transform maskTrans;//getcomponent�Ŏ���Ă��܂�,�ǂ�����ăv���t�@�u�̎q�I�u�W�F�N�g�������Ă���H�H
    private RectTransform canvasRect;//����Ă��܂�.

    private bool isAnimating = false;//�A�j���[�V���������ǂ���
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
            Destroy(gameObject); // ���ɃC���X�^���X�����݂��Ă���ꍇ�͔j��
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        canvasRect=canvas.GetComponent<RectTransform>();

    }

    void ActiveSceneChanged(Scene thisScene, Scene nextScene)
    {//�V�[�����؂�ւ�������Ɏ��s����܂�.
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
    //GameManager����StartCoroutine()����ƃo�O��̂ł����ōs���D
    public void StartLoadScene1(string scene)
    {
        nextsceneflag = false;


        Debug.Log("loadTownScene");
        StartUniteAnimation(); // �A�j���[�V�����J�n
        //StartFillingAnimation()
        gameObject.SetActive(true);
        StartCoroutine(StartLoadScene2(scene));
        



    }


    public IEnumerator StartLoadScene2(string scene)
    {
        // �A�j���[�V�����̊����܂őҋ@ 
        while (GameManager.Instance.sceneGO == false)
        {
            yield return null;
        }

        SceneManager.LoadScene(scene);


        //���̃A�j���[�V�����p�Ƀ��Z�b�g
        GameManager.Instance.sceneGO = false;
    }
    /// <summary>
    /// ///////////////////////////////
    /// </summary>


    //�����A�j���[�V����
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

            //�r��������؂𔭐�������
            if (elapsedTime>Duration/2f && handflag==false)
            {
                handflag = true;
                StartCoroutine(kirotation());
                //StartCoroutine(handrotation());
            }

        }
        AnimationMask.transform.localScale = endScale;
        GameManager.Instance.sceneGO = true;//���ꂪtrue�ɂȂ��scene���i��
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

    //�؂��~�`�ɔz�u����,��]
    public IEnumerator kirotation()
    {
        
        //�~�`�ɔz�u
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


            // ���S�ipivot�j����؂܂ł̕������v�Z
            Vector3 direction = (localPosition - Vector3.zero).normalized;

            // Z����]���v�Z���Đݒ�
            float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            woodInstance.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);



            //��ň�C�ɔj�󂷂邽��,���X�g�ɒǉ�
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

        while (isAnimating == true)//�A�j���[�V�������͈ȉ��𑱂���
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

























    //�u���b�N�𐶐�����t�F�[�h�A�j���[�V����

    public void StartFillingAnimation()
    {
        if (!isAnimating)//�A�j���[�V���������Ɏ��s���łȂ��ꍇ�̂݊J�n
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

        //�ŏ��̂���������Ԃ�
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


        //���ꂪfalse����gamemanager�̃��[�h����Ȃ��D
        GameManager.Instance.sceneGO = true;
        isAnimating = false;

    }


    private int madara = 0;
    private void SpawnBlock(Vector2 position)//�u���b�N���|�W�V������
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

        // �T�C�Y�̐ݒ�
        blockTransform.localScale = new Vector3(blockSize, blockSize, 1);

        DontDestroyOnLoad(gameObject);

        StartCoroutine(FadeOutBlock(block.GetComponent<SpriteRenderer>()));

    }


    //���[�h�r���ŃJ�b�g�C�����܂�
    private IEnumerator cutin02()
    {

        //prefab�ł���cutin�̎q�I�u�W�F�N�g��mask������܂�.

        GameObject cutin = Instantiate(cutinobject, transform);
        DontDestroyOnLoad(cutin);
        maskTrans =cutin.transform.Find("Sprite Mask");//�}�X�N��Transform���g��

        
        Vector3 startPosition = maskTrans.localPosition;

        //�������ɗ����Ă���.
        Vector3 endPosition= new Vector3(maskTrans.localPosition.x, 0, maskTrans.localPosition.z);
        //�e�I�u�W�F�N�g���V�[�����ɔj�󂵂Ȃ���Ηǂ��H


        //�J�b�g�C���̓���ł�

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

        //���̃V�[������������܂őҋ@
        while (nextsceneflag==false)
        {
            Debug.Log("No");
            yield return null;
        }




        //�J�b�g�C���̋A��ł�

        elapsedTime = 0f;

        while (elapsedTime < cutinDuration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / cutinDuration;

            maskTrans.localPosition = Vector3.Lerp(endPosition, startPosition, t);
            Debug.Log($"startPosition{startPosition},endPosition{endPosition},maskTrans.localPosition{maskTrans.localPosition}");

            yield return null;
        }

        //Dontdestroyonload����
        //SceneManager.MoveGameObjectToScene(cutin, SceneManager.GetActiveScene());
        //Debug.Log("Destroying cutin...");
        Destroy(cutin);
       


    }

    //�����t�F�[�h�A�E�g�����ł�
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
