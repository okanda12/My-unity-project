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
    {//ターンの初めにマナをリセットする

        
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

            //アニメータを取得しトリガーを設定
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

        //比較論理
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
            animator.SetBool("create",true);//自動的にcreateされるからいらなったわ
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


    //麻耶をつかったときに呼び出すスクリプトです
    public void AnimationManaUse(Transform costFrame,int cost)
    {
        int i = 0;
        int tempcost = 1;//これがコストを越えるまで
        //子オブジェクトの数を確認
        int childCount = costFrame.childCount;


        
        
        if (childCount<cost)
        {
            Debug.Log("Not enough Manacrystals to consume!");
            return;

        }

        while(tempcost <= cost)//末尾から探索
        {
            Transform manaCrystal = costFrame.GetChild(childCount - i - 1);
            Animator animator = manaCrystal.GetComponent<Animator>();

            //Debug.Log($"getbool {animator.GetBool("create")}");
            //Debug.Log($"childcountis{childCount}");

            //アニメータを取得しトリガーを設定




            if (animator != null　&& animator.GetBool("create")==true)
            {
                tempcost += 1;
                animator.SetBool("used",true);
                animator.SetBool("create", false);
                //animator.SetBool("shining", false);//使えないマナとなる.


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
