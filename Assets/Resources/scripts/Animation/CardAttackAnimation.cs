using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//�J�[�h���U������A�j���[�V�����ł�.
public class CardAttackAnimation : MonoBehaviour
{

    public CardView View;
    public CardModel Model;
    public GameObject banish;//���ʂƂ��ɂ���큁��ƂȂ���

  
    //��A�̓���
    //from�J�[�h���U������target�Ɍ�����,from�ɖ߂�܂�.

    public float AttackDurationGO;//�s���̎���
    public float AttackDurationBack;//�߂�̎���
    public float CantAttackDuration;//�U���ł��Ȃ�

    public float DieDuration;//����ł����Ƃ��̃A�j���[�V��������

    private Transform rectTransform;//�A�^�b�`����Ă���J�[�h��transform�ł�

    public Transform HeartPosition;

    public GameObject Attackedtext;//�U�����ꂽ�Ƃ�,�����Ƃ��ɂۂ��`��ƂȂ���
    public float AttackedDuration;//�̎���
    public float bletime;

    [SerializeField] private Animator HEROdamageAnim;//�q�[��[���_���[�W���󂯂��Ƃ��̃A�j���[�V����


    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
       


    }

    //�_���[�W���󂯂��Ƃ��ɂ�����ƂԂ��A�j���[�V�����ł�
    public IEnumerator ble()
    {
        Vector3 startPosition = this.transform.position;
        
        float elapsedTime = 0f;

        while (elapsedTime < bletime)
        {

            elapsedTime += Time.deltaTime;
            this.transform.position = startPosition + new Vector3(0.03f*Random.Range(-1f,1f),0.03f* Random.Range(-1f, 1f), 0);

            yield return null;
        }


        this.transform.position = startPosition;
    }


    //�n�[�g�����ł����A�j���[�V����
    public IEnumerator HeartDamage(int damage)
    {

        Vector3 startPosition = HeartPosition.position;
        Vector3 endPosition = startPosition + new Vector3(0, 0.05f, 0);


        TextMeshProUGUI Damagetext = Attackedtext.GetComponentInChildren<TextMeshProUGUI>();


        Damagetext.text = damage.ToString();

        GameObject Damagetextobject = Instantiate(Attackedtext, HeartPosition, false);







        float elapsedTime = 0f;


        while (elapsedTime < AttackedDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / AttackedDuration;

            if (Damagetextobject != null)
            {
                Damagetextobject.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            }

            yield return null;


        }

        Destroy(Damagetextobject);//����

        if (this != null)
        {
            Model = GetComponent<CardController>().model;
            View = GetComponent<CardController>().view;


            if (Model.cardType=="Hero")
            {
                HEROdamageAnim.SetTrigger("HeroDamage");
            }


            //�_���[�W���󂯂܂�
            Model.hp -= damage;
            if (Model.hp <= 0)
            {
                StartCoroutine(Die());

            }

            
            View.Show(Model);
        }

    }

    //�_���[�W���󂯂��Ƃ��̃A�j���[�V����
    public IEnumerator GetDamage(int damage)
    {


        StartCoroutine(ble());
        //Transform HeartPosition = transform.Find("Heart");//�n�[�g�̈ʒu�����΂�
        yield return StartCoroutine(HeartDamage(damage));

    }


    //���ʂƂ��ł�.����(banisher)���Ď��ɂ܂�.
  
    public IEnumerator Die()
    {


        GameObject banisher=Instantiate(banish, transform);


        yield return new WaitForSeconds(0.5f);

        Destroy(banisher);

        Destroy(gameObject);

    }
    //}


    //�J�[�h�ɓ��������邽��,Horizontal Layout����u���������ɂ��܂�.
    //Horizontal Layout ��t���Ă����position�̕ύX���ł��܂���....
    public void IgnoreLayout(Transform from,  bool ignore)
    {
        LayoutElement layoutElement = from.GetComponent<LayoutElement>();

        if (layoutElement != null)//����Ȃ�
        {
            layoutElement.ignoreLayout = ignore;//���C�A�E�g�𖳎����邩�ݒ�
        }
    }

    //�U���ł��Ȃ��Ƃ�,������Ɖ�]���Ė߂�܂�.�͂ɂ�H�������Ȋ���
    public IEnumerator CantAttack(Transform from)
    {
        float elapsedTime = 0f;

        //IgnoreLayout(from,true);�܂�holizontal�͉�]�𐧖񂵂Ȃ��̂ł��Ȃ��Ă�������


        Quaternion startRotation = from.GetComponent<Transform>().localRotation;



        while (elapsedTime < CantAttackDuration)
        {

            
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / CantAttackDuration;
            float rotationOffset = 4* Mathf.Sin(t);
            //�ʒu�Ɖ�]��⊮
            //Debug.Log(rotationOffset);

            rectTransform.rotation = Quaternion.Euler(0, 0,rotationOffset);


            yield return null;

        }

        rectTransform.rotation = startRotation;




        //IgnoreLayout(from, false);

        yield return new WaitForSeconds(CantAttackDuration);




    }



    //�U�����[�V�����ł�.�����̈ʒu(from)���瑊��̈ʒu(target)�֍s���Ė߂�܂�
    public IEnumerator AttackAnim(CardController fromCard,CardController targetCard)
    {



        


        Transform from = this.transform;

        
        Transform target = targetCard.transform;

        Transform defaultparent = this.transform.parent;

        //���������Ԃ����ɕ\������


        float elapsedTime = 0f;


        IgnoreLayout(from,true);//��U���C�A�E�g�𖳎����܂�.

        Vector3 startPosition = from.GetComponent<Transform>().position;
        Vector3 endPosition = target.GetComponent<Transform>().position;



        Debug.Log($"startPosition:{startPosition},endPosition:{endPosition}");


        //identityEuler(0, 0, -45)
        Quaternion startRotation = Quaternion.identity;//����͖���]
        Quaternion endRotation = Quaternion.Euler(0,0,-45);


        this.transform.SetParent(BattleManager.Instance.canvas2, true);//�����炭�����Őݒ肷��̂��ǂ�����

        //////////////////
        ///�A�^�b�N�s��
        while (elapsedTime <AttackDurationGO - 0.2*AttackDurationGO)//���S�ɏd�Ȃ钼�O�Ŏ~�܂�
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / AttackDurationGO;

            //�ʒu�Ɖ�]��⊮
            rectTransform.position = Vector3.Lerp(startPosition,endPosition,t);
            rectTransform.rotation = Quaternion.Lerp(startRotation, endRotation, t);


            yield return null;

        }
        endPosition = rectTransform.position;//���݈ʒu
        endRotation = rectTransform.rotation;//���݈ʒu
        ////////////////////////
        //�A�^�b�N�r��

        CardAttackAnimation targetAnim = targetCard.GetComponent<CardAttackAnimation>();//������莝���Ă��Ă�
        
        
        StartCoroutine(targetAnim.GetDamage(fromCard.model.at));//����Ɏ󂯂�������
        StartCoroutine(GetDamage(targetCard.model.at));//�������󂯂�

        yield return new WaitForSeconds(0.2f);//�Ȃ񂩂��ꂶ�イ�悤���


        /////////////////////////////////
        //�A�^�b�N�A��
        elapsedTime = 0f;

        while (elapsedTime < AttackDurationBack)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / AttackDurationBack;

            //�ʒu�Ɖ�]��⊮
            rectTransform.position = Vector3.Lerp(endPosition, startPosition, t);
            rectTransform.rotation = Quaternion.Lerp(endRotation, startRotation, t);


            yield return null;

        }



        rectTransform.position = startPosition;
        rectTransform.rotation = startRotation;

        IgnoreLayout(from,false);
        this.transform.SetParent(defaultparent, true);
        Debug.Log($"defaultParent{defaultparent}");

        //yield return new WaitForSeconds(AttackDurationGO+AttackDurationBack);
        fromCard.canAttack = false;

    }
}
