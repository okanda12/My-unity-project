using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerClickHandler
{
    //MVC�p�^�[��

    public bool canAttack = true;


    //�J�[�h�f�[�^��\������
    public CardView view;
    //�J�[�h�f�[�^�Ɋւ��邱�Ƃ𑀍�
    public CardModel model;

    //�J�[�h�̈ړ��𑀍�
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
        movement.cardModel = model;//CardMovement��Cardmodel��n��
 
    
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        

        Debug.Log(transform.parent);

        if (transform.parent == GameManager.Instance.PlayerFieldTransform || transform.parent == GameManager.Instance.PlayerHEROfield)
        {
            GameManager.Instance.SelectCardToAttack(this,"Attacker");//�U���҂�ݒ肷��
        }else if (transform.parent == GameManager.Instance.EnemyFieldTransform || transform.parent == GameManager.Instance.EnemyHEROfield)
        {
            GameManager.Instance.SelectCardToAttack(this, "Defender");//�h��҂�ݒ肷��.
        }


        //�N���b�N���ꂽ��
        //��D�̃J�[�h�͍U���o���Ȃ�
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
