using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

//�J�[�h�̓����������ǂ�X�N���v�g�ɂȂ�܂�

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //�h���b�O�\���ǂ����̃t���O


    
    //�����炭�ŏ���Hand�ɂȂ��Ă�H
    public Transform defaultParent;
    public CardModel cardModel;
    CardController cardcon;
    public CardCastAnimation castAnim;

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private int originalSortingOrder;

    private bool isDraggable;


    public void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        cardcon = this.GetComponent<CardController>();

        if (canvas != null)
        {
            //���̃\�[�g�����L�^
            originalSortingOrder = canvas.sortingOrder;

        }
    }

    //��D����t�B�[���h�ɃJ�[�h�̈ʒu��ύX����.�h���b�O�Ȃ�
    public void SetCardTransform(Transform parentTransform)
    {
        
        defaultParent = parentTransform;
        transform.SetParent(defaultParent, false);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {

        isDraggable = (transform.parent == BattleManager.Instance.PlayerHandTransform);
        //�e�I�u�W�F�N�gparent��ϐ��ɕۑ�
        if (!isDraggable) return;
        defaultParent = transform.parent;
        //�e�I�u�W�F�N�g��c����ɂ���@false=���[�J�����W���g�p
        transform.SetParent(defaultParent.parent, false);
        //false=�I�u�W�F�N�g�h���b�O���ɑ���UI���N���b�N�\��
        
        //�`�揇�����őO�ʂɂ���
        if(canvas != null)
        {
            canvas.sortingOrder = 1000; //�őO�ʂɕ\�����邽�߂̃\�[�g��
        }

        //�h���b�O���ɑ���UI���N���b�N�\�ɂȂ�
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;//����������
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;
        

        Vector3 cardPos = Camera.main.ScreenToWorldPoint(eventData.position);
        cardPos.z = 0;
        transform.position = cardPos;
        //�J�[�h�������������Ƃ��ɍs������
        //transform.position = eventData.position;
        

        ///////////////////////////////
        //�t�B�[���h�𔭌������܂�
        foreach (Transform daiza in BattleManager.Instance.PlayerFieldTransform)
        {
            Animator animator= daiza.GetComponent<Animator>();

            animator.SetBool("chose", true);
        }

        ////////////////////////////
       




    }




    public void OnEndDrag(PointerEventData eventData)
    {//DropPlace����defaultParent��Dropplace�����Ă���I�u�W�F�N�g�ɂȂ��ĕԂ��Ă���
        if (!isDraggable) return;
        CardCastAnimation castAnim = this.GetComponent<CardCastAnimation>();


        ////�t�B�[���h�̌��������܂�
        foreach (Transform daiza in BattleManager.Instance.PlayerFieldTransform)
        {
            Animator animator = daiza.GetComponent<Animator>();

            animator.SetBool("chose", false);
        }



        if (BattleManager.Instance.isPlayerTurn == false) //��������^�[���Ȃ���Ȃ�.
        {
            StartCoroutine(castAnim.cantMinionCast(defaultParent));
            Debug.Log("youcan't play in Enemy Turn!!!");
        }
        else
        {
            //�v���C���[�̃J�[�h�łȂ���΃h���b�O�s��



            if (cardModel.cost > BattleManager.Instance.Player_Mana || defaultParent ==BattleManager.Instance.PlayerHandTransform)
            {�@//�o���Ȃ�
                Debug.Log("youcan't play this card!!");
                StartCoroutine(castAnim.cantMinionCast(defaultParent));
            
            
                
            }
            else//�J�[�h���o����Ƃ�
            {
                //defaultParent�������炭Daiza�ɂȂ��Ă܂�
                if (defaultParent.childCount != 0)//���ɐe���q�������Ă�����
                {
                    Debug.Log("youcan't play this card!!");
                    StartCoroutine(castAnim.cantMinionCast(BattleManager.Instance.PlayerHandTransform));
                }
                else
                {
                    

                    

                    

                    if (cardModel.cardType == "Magic")
                    {//source target

                        StartCoroutine(castAnim.MagicCast(defaultParent));
                        
                        
                        
                    }
                    else
                    {
                        StartCoroutine(castAnim.MinionCast(defaultParent));

                        



                    }

                    BattleManager.Instance.manasys.UseMana(cardModel.cost);


                    cardcon.ARMA(cardModel.cost);//�A���}�f�o�C�X�ɃR�X�g���ǉ�����


                }

            }


        }


        //�J�[�h��b�������ɍs�������@���̐e�ɖ߂�
        //transform.SetParent(defaultParent, false);

        //�`�揇�������ɖ߂�
        if (canvas != null)
        {
            canvas.sortingOrder = originalSortingOrder;
        }

        //Raycast�u���b�N�����ɖ߂�
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

    }






}
