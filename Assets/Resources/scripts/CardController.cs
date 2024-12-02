using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerClickHandler
{
    //MVC�p�^�[��

    public bool canAttack = true;
    private ARMAdevice ARMAdevice;

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
        ARMAdevice = FindObjectOfType<ARMAdevice>();

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

        if (transform.parent == BattleManager.Instance.PlayerFieldTransform || transform.parent == BattleManager.Instance.PlayerHEROfield)
        {
            BattleManager.Instance.SelectCardToAttack(this,"Attacker");//�U���҂�ݒ肷��
        }else if (transform.parent == BattleManager.Instance.EnemyFieldTransform || transform.parent == BattleManager.Instance.EnemyHEROfield)
        {
            BattleManager.Instance.SelectCardToAttack(this, "Defender");//�h��҂�ݒ肷��.
        }


        //�N���b�N���ꂽ��
        //��D�̃J�[�h�͍U���o���Ȃ�
        //if (transform.parent != BattleManager.Instance.PlayerHandTransform)
        //{
        
        //    return;
        //}
       
    }



    public void Attack(CardController targetCard)
    {
        Transform from = this.transform;
        Transform target = targetCard.transform;

        //int hi = targetCard.model.hp;

        CardAttackAnimation cardAnim = this.GetComponent<CardAttackAnimation>();
        if (targetCard !=null)
        {
            
            StartCoroutine(cardAnim.AttackAnim(this,targetCard));



            
            //targetCard.TakeDamage(model.at);//���̃J�[�h�̍U���͂�^����
            

        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }

    public void ARMA(int cost)
    {
        if (ARMAdevice != null)
        {
            ARMAdevice.AddCost(cost);
        }




    }










}
