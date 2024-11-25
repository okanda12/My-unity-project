using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerClickHandler
{
    //MVCパターン

    public bool canAttack = true;


    //カードデータを表示する
    public CardView view;
    //カードデータに関することを操作
    public CardModel model;

    //カードの移動を操作
    public CardMovement movement;

    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
    }
    public void Init(ICard cardEntity)
    {
        model = new CardModel(cardEntity, cardEntity.minioneffect);
        view.Show(model);
        movement.cardModel = model;//CardMovementにCardmodelを渡す
 
    
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        

        Debug.Log(transform.parent);

        if (transform.parent == GameManager.Instance.PlayerFieldTransform || transform.parent == GameManager.Instance.PlayerHEROfield)
        {
            GameManager.Instance.SelectCardToAttack(this,"Attacker");//攻撃者を設定する
        }else if (transform.parent == GameManager.Instance.EnemyFieldTransform || transform.parent == GameManager.Instance.EnemyHEROfield)
        {
            GameManager.Instance.SelectCardToAttack(this, "Defender");//防御者を設定する.
        }


        //クリックされたら
        //手札のカードは攻撃出来ない
        //if (transform.parent != GameManager.Instance.PlayerHandTransform)
        //{
        
        //    return;
        //}
       
    }



    public void Attack(CardController targetCard)
    {
        Transform from = this.GetComponent<Transform>();
        Transform target = targetCard.GetComponent<Transform>();


        CardAttackAnimation cardAnim = this.GetComponent<CardAttackAnimation>();
        if (targetCard !=null)
        {
            
            StartCoroutine(cardAnim.AttackAnim(from,target));

            targetCard.TakeDamage(model.at);
            

        }
    }
    public void TakeDamage(int damage)
    {
        model.hp -= damage;
        if (model.hp<=0)
        {
            Die();

        }
        view.Show(model);

    }



    public void Die()
    {
        Destroy(gameObject);
    }

    



    
}
