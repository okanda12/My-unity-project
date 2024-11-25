using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manasystem : MonoBehaviour
{

    GameObject manaPrefab = Resources.Load<GameObject>("prefabs/Manadot");
    

    

        


    public int Player_Mana
    {
        get => GameManager.Instance.Player_Mana;
        set => GameManager.Instance.Player_Mana = value;
    }

    public int Player_maxMana
    {
        get => GameManager.Instance.Player_maxMana;
        set => GameManager.Instance.Player_maxMana = value;
    }

    public int Enemy_Mana
    {
        get => GameManager.Instance.Enemy_Mana;
        set => GameManager.Instance.Enemy_Mana = value;
    }

    public int Enemy_maxMana
    {
        get => GameManager.Instance.Enemy_maxMana;
        set => GameManager.Instance.Enemy_maxMana = value;
    }


    public void InitializeMana(int startMana)
    {
        Player_Mana = startMana;
        Player_maxMana = startMana;
        Enemy_Mana = startMana;
        Enemy_maxMana = startMana;
    }

    public void RestoreMana(Transform costFrame)
    {//�^�[���̏��߂Ƀ}�i�����Z�b�g����

        
        if (GameManager.Instance.isPlayerTurn == true)
        {
            Player_Mana = Player_maxMana;
        }
        else
        {
            Enemy_Mana = Enemy_maxMana;
        }


        int childCount = costFrame.childCount;

        //Debug.Log($"childCount{childCount}");
        for (int i = 0; i < childCount; i++)
        {
            Transform manaCrystal = costFrame.GetChild(i);

            //�A�j���[�^���擾���g���K�[��ݒ�
            Animator animator = manaCrystal.GetComponent<Animator>();

            if (animator != null)
            {
                animator.SetBool("create",true);
                animator.SetBool("used", false);
                //animator.SetBool("shining", true);
                Debug.LogWarning($"MANA create trigger! ");
            }
            else
            {
                Debug.LogWarning($"Animator not found on ManaCrystal: {manaCrystal.name}");
            }




        }




        

        
    }

    public bool CanPlayCard(int cardCost)
    {

        if (GameManager.Instance.isPlayerTurn == true)
        {
            return Player_Mana >= cardCost;
        }
        else
        {
            return Enemy_Mana >= cardCost;
        }

        //��r�_��
    }

    public void UseMana(int cost)
    {
        if (GameManager.Instance.isPlayerTurn == true)
        {
            Player_Mana -= cost;

            AnimationManaUse(GameManager.Instance.PlayerCostframe, cost);
        }
        else
        {
            Enemy_Mana -= cost;
            AnimationManaUse(GameManager.Instance.EnemyCostframe, cost);
        }
        
    }

    public void AddMana(int cost)
    {
        Animator animator = manaPrefab.GetComponent<Animator>();

        if (GameManager.Instance.isPlayerTurn == true)
        {
            Player_Mana += cost;
            Instantiate(manaPrefab,GameManager.Instance.PlayerCostframe, false);
            animator.SetBool("create",true);//�����I��create����邩�炢��Ȃ�����
            //animator.SetBool("shining", true);

        }
        else
        {
            Enemy_Mana += cost;
            Instantiate(manaPrefab, GameManager.Instance.EnemyCostframe, false);
            //animator.SetTrigger("Create");
        }

    }
    public void AddmaxMana(int cost)
    {
        if (GameManager.Instance.isPlayerTurn == true)
        {
            Player_maxMana += cost;
        }
        else
        {
            Enemy_maxMana += cost;
        }

    }


    //������������Ƃ��ɌĂяo���X�N���v�g�ł�
    public void AnimationManaUse(Transform costFrame,int cost)
    {
        int i = 0;
        int tempcost = 1;//���ꂪ�R�X�g���z����܂�
        //�q�I�u�W�F�N�g�̐����m�F
        int childCount = costFrame.childCount;


        
        
        if (childCount<cost)
        {
            Debug.Log("Not enough Manacrystals to consume!");
            return;

        }

        while(tempcost <= cost)//��������T��
        {
            Transform manaCrystal = costFrame.GetChild(childCount - i - 1);
            Animator animator = manaCrystal.GetComponent<Animator>();

            //Debug.Log($"getbool {animator.GetBool("create")}");
            //Debug.Log($"childcountis{childCount}");

            //�A�j���[�^���擾���g���K�[��ݒ�




            if (animator != null�@&& animator.GetBool("create")==true)
            {
                tempcost += 1;
                animator.SetBool("used",true);
                animator.SetBool("create", false);
                //animator.SetBool("shining", false);//�g���Ȃ��}�i�ƂȂ�.


                //Debug.LogWarning($"used trigger! ");
            }
            else
            {
                //Debug.Log($"Animator not found on ManaCrystal: {manaCrystal.name}");
            }

            i++;



        }




    }




}
