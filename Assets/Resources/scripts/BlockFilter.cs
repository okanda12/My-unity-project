using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


//�V�[����؂�ւ���Ƃ��̃t�F�[�h�A�j���[�V�����ł�
public class BlockFilter : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;//�t�F�[�h����Ƃ��̃u���b�Nprefab�ł�.
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float spawnInterval;//�u���b�N���������鎞�Ԃł�.
    [SerializeField] private int blockSize ;//�u���b�N��size100*100??
    [SerializeField] private GameObject cutinobject;//���[�h���ɏo�Ă���02�A�C�R����͋[���Ă��܂�


    [SerializeField] private float cutinDuration;
    private Transform maskTrans;//getcomponent�Ŏ���Ă��܂�,�ǂ�����ăv���t�@�u�̎q�I�u�W�F�N�g�������Ă���H�H


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
    }


    void ActiveSceneChanged(Scene thisScene, Scene nextScene)
    {//�V�[�����؂�ւ�������Ɏ��s����܂�.
        Debug.Log(thisScene.name);
        Debug.Log(nextScene.name);
        nextsceneflag = true;
    }



    public void StartFillingAnimation()//�O������Q�Əo����A�j���[�V����
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

        float screenWidth = canvasRect.rect.width;
        float screenHeight = canvasRect.rect.height;

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

        GameManager.Instance.sceneGO = true;
        isAnimating = false;

    }



    private void SpawnBlock(Vector2 position)//�u���b�N���|�W�V������
    {
        Debug.Log("spawn");
        GameObject block = Instantiate(blockPrefab, transform);
        Transform blockTransform = block.transform;

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

            Debug.Log($"startPosition{startPosition},endPosition{endPosition},maskTrans.localPosition{maskTrans.localPosition}");
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);

        //���̃V�[������������܂őҋ@
        while (nextsceneflag==false)
        {
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
