using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockFilter : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int blockSize ;

    private bool isAnimating = false;//�A�j���[�V���������ǂ���


    private static BlockFilter instance;

    void Awake()
    {
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

        for (int y=0;y<rows;y++)
        {
            for (int x=0;x<cols;x++)
            {
                Vector2 position = new Vector2(x * blockSize-600, -y * blockSize +360);
                SpawnBlock(position);
                yield return new WaitForSeconds(spawnInterval);

            }

        }
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

    private IEnumerator FadeOutBlock(SpriteRenderer renderer)
    {
        Debug.Log("Fadeout");
        Color originalColor = renderer.color;
        float timer = 0;

        while (timer <fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b,alpha);

            
            yield return null;

        }

        Destroy(renderer.gameObject);
    }
















}
