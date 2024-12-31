using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{


    //�J�[�h�̃v���t�@�u������
    [SerializeField] CardController cardPrefab;
    [SerializeField] GameObject ARMAdevpre;
    [SerializeField] GameObject RAIKAdevpre;
    [SerializeField] GameObject Result_Prefab;
    
    //�q�[���[�̃v���t�@�u������
    // [SerializeField] heroPrefab;


    public Transform PlayerHandTransform, EnemyHandTransform;
    public Transform PlayerFieldTransform, EnemyFieldTransform;
    public Transform PlayerHEROfield, EnemyHEROfield;
    public Transform PlayerDeckTransform, EnemyDeckTransform;
    public Transform PlayerCostframe, EnemyCostframe;
    public Transform canvas2; //�U������Ƃ��ɃJ�[�h����ԏ�ɕ\�������悤�Ɉړ�����L�����o�X
    public Transform MagiCasPlace;//���@���L���X�g����ꏊ
    public Transform EnemyDevice, PlayerDevice;//�f�o�C�X��u���ꏊ


    public Deck playerDeck;
    public Deck enemyDeck;

    //�v���C���[�̃^�[�����̔���
    public bool isPlayerTurn;//Turndisplayer�ɏo��

    public string PlayerHERO = "ARMA";
    public string EnemyHERO = "RAIKA";


    public int Player_Mana;
    public int Player_maxMana;

    public int Enemy_Mana;
    public int Enemy_maxMana;

    //�}�i���Ǘ�����
    public Manasystem manasys;



    public string Winner = "nobody";
    public string Loser = "nobody";

    //����f�o�C�X
    private RAIKAdevice RAIKAdevice;


    public CardController selectedCard;//���ݑI������Ă���J�[�h
    public List<CardController> enemyCards;//�G�J�[�h�̃��X�g

    public CardController PlayerHerocon;
    public CardController EnemyHerocon;


    //�G�̃^�[��,�ȉ�2�̃t���b�O��true�ɂȂ�����^�[���I��
    private bool EMcastflag = false;//�L���X�g�ł���J�[�h���Ȃ���true;
    private bool EMatackflag = false; //�A�^�b�N�ł���J�[�h��������true;

    public bool isAnimating = false;//�A�j���[�V���������ǂ������f


    private bool EnemyCasted = false;//�G���J�[�h���g����������


    //�V���O���g���ɂ��邽�߂̎���
    public static BattleManager Instance { get; private set; }

    private void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }
    //

    void Start()
    {

        
        

        if (manasys == null)
        {
            manasys = new Manasystem();
        }


        StartCoroutine(StartGame());



        
    }

    private IEnumerator StartGame()
    {

        //�q�[��[�o�����[�V��������ꂽ��

        yield return new WaitForSeconds(1.5f);
        
        //�q�[��]�f�o�C�X���[�V��������ꂽ��


        HeroAppears();

       



        StartCoroutine(SettingHand());
        manasys.InitializeMana(0);
        isPlayerTurn = true;
        TurnCalc();

        RAIKAdevice = FindObjectOfType<RAIKAdevice>();
    }



    public void Result()
    {

        GameObject canvas = GameObject.Find("Canvas");
        GameObject result = Instantiate(Result_Prefab, canvas.transform);





    }


    public void EnemyFieldHighlight(bool YES)
    {//�G�̏����擾.������Ƀn�C���C�g������

        //���ȑ����ǂ��܂�
        CardController[] EnemyFieldcardList = EnemyFieldTransform.GetComponentsInChildren<CardController>();

        if (YES == true)
        {

            foreach (var card in EnemyFieldcardList)
            {
                CardHighlightController cardhighlight = card.GetComponent<CardHighlightController>();
                cardhighlight.ShowRedHighlight();
                //Debug.Log($"Card Name: {card.name}, ATK: {card.model.at}, HP: {card.model.hp}");
            }

        }
        else
        {

            foreach (var card in EnemyFieldcardList)
            {
                CardHighlightController cardhighlight = card.GetComponent<CardHighlightController>();
                cardhighlight.RemoveHighlight();
                //Debug.Log($"Card Name: {card.name}, ATK: {card.model.at}, HP: {card.model.hp}");
            }

        }

    }



    //�J�[�h���I�΂ꂽ�Ƃ��̏���
    //card�ɂ͍U�����J�[�h�������Ă���
    public void SelectCardToAttack(CardController card, string isAttacker)
    {

        if (isAttacker == "Attacker")//�U���҂Ȃ�
        {
            //�I�����Ă���J�[�h
            selectedCard = card;
            if (selectedCard.canAttack == true)//�U���ł���Ȃ�
            {

                Debug.Log("�U���҃J�[�h�I��:" + selectedCard.model.name);//�����O���擾
                EnemyFieldHighlight(true);//�G�̃t�B�[���h�̃~�j�I���Ƀn�C���C�g��t���܂�.


            }
            else
            {
                Debug.Log(selectedCard.model.name + "can't Attack!");

                CardAttackAnimation cantAttackAnim = card.GetComponent<CardAttackAnimation>();

                StartCoroutine(cantAttackAnim.CantAttack(card.transform));




            }


        }
        else if (isAttacker == "Defender" && selectedCard != null)//�h��҂Ȃ�
        {

            if (selectedCard.canAttack == true)
            {
                //���ɃJ�[�h���I�΂�Ă���ꍇ�̓^�[�Q�b�g�I��
                AttackTarget(selectedCard, card);//�U���ҁ@����҂̏�
                Debug.Log("��U���҃J�[�h�I��:" + card.model.name);

                selectedCard = null;//�U����͑I��������

                EnemyFieldHighlight(false);//�n�C���C�g�������܂�
            }
            else
            {
                Debug.Log(selectedCard.model.name + "can't Attack!");

                //�h��ґ��ɂ͂���Ȃ�������
                //CardAttackAnimation cantAttackAnim = card.GetComponent<CardAttackAnimation>();

                //StartCoroutine(cantAttackAnim.CantAttack(card.transform));

            }


        }

    }

    public void AttackTarget(CardController attacker, CardController target)
    {
        //Transform targetparent = target.transform.parent;
        //�^�[�Q�b�g���G�J�[�h�ł���΍U������
        //if (targetparent == EnemyFieldTransform || targetparent == EnemyHEROfield)
        //{
        attacker.Attack(target);//�U�����\�b�h�Ăяo��

        Debug.Log($"{attacker.model.name}��{target.model.name}���U�����܂���");
        // }

    }




    //�q�[���[���o�ꂷ�镔���ł�
    private void HeroAppears()
    {
        //���������Ɗȑf���o���邩��

        ICard Playercard=LoadHeroCard(1);
        ICard Enemycard=LoadHeroCard(1);

        switch (PlayerHERO) {
            case "ARMA":
                Playercard= LoadHeroCard(1);
                playerDeck = CreateDeck("ARMA");
                Instantiate(ARMAdevpre, PlayerDevice, false);
                break;
            case "RAIKA":
                Playercard = LoadHeroCard(2);
                playerDeck = CreateDeck("RAIKA");
                Instantiate(RAIKAdevpre, PlayerDevice, false);
                break;

        }

        switch (EnemyHERO)
        {
            case "ARMA":
                Enemycard = LoadHeroCard(1);
                enemyDeck = CreateDeck("ARMA");
                Instantiate(ARMAdevpre, EnemyDevice, false);
                break;
            case "RAIKA":
                Enemycard = LoadHeroCard(2);
                enemyDeck = CreateDeck("RAIKA");
                Instantiate(RAIKAdevpre, EnemyDevice, false);
                break;

        }
       

        CreateHero(PlayerHEROfield, Playercard);
        CreateHero(EnemyHEROfield, Enemycard);


        

    }

    private Deck CreateDeck(string hero)
    {
        Deck deck = new Deck();
        ICard card;

        //minion��magic�̃J�[�h���f�b�L�ɒǉ�
        for (int i = 1; i <= 5; i++)//3���̃~�j�I����ǉ�
        {
            card = LoadCard(hero, "minions", i);
            if (card != null) deck.AddCard(card);
        }

        //�T�����ς����ꂽ���Ƃ����肢
        card = LoadCard(hero, "minions", 1);
        if (card != null) deck.AddCard(card);
        card = LoadCard(hero, "minions", 1);
        if (card != null) deck.AddCard(card);
        card = LoadCard(hero, "minions", 1);
        if (card != null) deck.AddCard(card);
        for (int i = 1; i <= 5; i++)//3����magic��ǉ�
        {
            card = LoadCard(hero, "magics", i);
            if (card != null) deck.AddCard(card);
        }
        deck.Shuffle();//�V���b�t��

        Debug.Log(deck.DeckCount);
        deck.PrintCardNames();//�f�o�b�N�p�֐�

        return deck;

    }

    void TurnCalc()
    {
        //�^�[���̐i�s�̏���������.
        if (isPlayerTurn)
        {
            PlayerTurn();

        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    void PlayerTurn()
    {
        //�v���C���[�^�[���̏���
        Debug.Log("�v���C���[�̃^�[���ł�");



        manasys.AddMana(1);
        //manasys.AddEmptyMana(1);
        manasys.RestoreMana(PlayerCostframe);

        CardController[] PlayerFieldcardList = PlayerFieldTransform.GetComponentsInChildren<CardController>();


        //�U���t���b�O������������
        foreach (var card in PlayerFieldcardList)
        {
            card.canAttack = true;

        }

    }







    //�G�̃L���X�g�Z�N�V����
    public IEnumerator EnemyCast()
    {
        EnemyCasted = false;//�J�[�h���g���������܂�
        
        Debug.Log("Enemycast!");

        CardController[] EMhandCC = EnemyHandTransform.GetComponentsInChildren<CardController>();//�n���h�̃J�[�h�R���g���[��
        CardController[] EMfieldCC = EnemyFieldTransform.GetComponentsInChildren<CardController>();//�t�B�[���h�̃R���g���[���[


        //������D�ɉ����Ȃ�������I��

        if (EMhandCC.Length == 0)
        {
            EMcastflag = true;
            Debug.Log("EMhandCC is empty");
            yield return new WaitForSeconds(0.5f);
            yield break; // �R���[�`�����I��
        }

        int maxIterations = EMhandCC.Length; // �������[�v�h�~
        int iterationCount = 0;


        //maxIteration�܂ňȉ����J��Ԃ�.���肪������ƍl����悤�ȓ�����Ƃ�Ƃ��͂��ꂪ����

        while (iterationCount < maxIterations)
        {


            bool cardCasted = false;




            foreach (CardController card in EMhandCC)//�n���h��S�Ă݂ā@
            {
                if (card.model.cost <= Enemy_Mana && EMfieldCC.Length < 5)//��D�̃J�[�h�ꖇ�����Ă��܂�
                {
                    
                    //�t�B�[���h�̏�Ԃ����Ă���
                    List<Transform> NocardDaiza = new List<Transform>();//�����ɃI�u�W�F�N�g�������Ȃ������ǉ����Ă���


                    foreach (Transform child in EnemyFieldTransform)//�q�I�u�W�F�N�g�������Ȃ�daiza���擾���Ă���
                    {
                        if (child.childCount == 0) // �q�I�u�W�F�N�g�������Ȃ��Ȃ�t�m�[�h
                        {
                            NocardDaiza.Add(child);
                        }

                    }


                    CardCastAnimation castAnim = card.GetComponent<CardCastAnimation>();
                    BattleManager.Instance.manasys.UseMana(card.model.cost);

                    if (card.model.cardType=="Magic")
                    {
                        StartCoroutine(castAnim.MagicCast(NocardDaiza[0]));
                        
                    }
                    else
                    {
                        StartCoroutine(castAnim.MinionCast(NocardDaiza[0]));
                    }


                    EnemyCasted = true;


                    cardCasted = true;


                    //yield return new WaitForSeconds(0.5f);
                    break; // foreach �𔲂���


                }
            }

            // ��D�E�t�B�[���h���X�V
            EMhandCC = EnemyHandTransform.GetComponentsInChildren<CardController>();
            EMfieldCC = EnemyFieldTransform.GetComponentsInChildren<CardController>();

            // �o����J�[�h���Ȃ��Ȃ�����I��
            if (!cardCasted) break;

            iterationCount++;
        }
        yield return new WaitForSeconds(0.1f);
        EMcastflag = true;



    }


    //�G�̍U�����߂ł�
    public IEnumerator EnemyAttack()
    {
        Debug.Log("EnemyAttack!");
        bool canatEMfield = true;//�����U���ł���J�[�h������Ƃ�


        while (canatEMfield != false)
        {
            canatEMfield = false;

            CardController[] EMfieldCC = EnemyFieldTransform.GetComponentsInChildren<CardController>();//�t�B�[���h�̃R���g���[���[
            CardController[] PLfieldCC = PlayerFieldTransform.GetComponentsInChildren<CardController>();//�v���C���[�t�B�[���h


            if (EMfieldCC.Length == 0) yield break;


            if (EMfieldCC != null)
            {
                foreach (CardController card in EMfieldCC)
                {
                    EMfieldCC = EnemyFieldTransform.GetComponentsInChildren<CardController>();//�t�B�[���h�̃R���g���[���[
                    PLfieldCC = PlayerFieldTransform.GetComponentsInChildren<CardController>();//�v���C���[�t�B�[���h


                    if (card.canAttack == true)
                    {
                        //Debug.Log($"PLfieldCC{PLfieldCC},PLfieldCC.length{PLfieldCC.Length}");
                        if (PLfieldCC.Length > 0)
                        {
                            CardController EnemyCard = PLfieldCC[0];//�����΂�[�����̓G(����)���U��
                            AttackTarget(card, EnemyCard);

                            yield return new WaitForSeconds(0.5f);


                        }
                        else//�U���ł��Ȃ�
                        {
                            CardController player = PlayerHEROfield.GetComponentInChildren<CardController>();//�q�[��[���U��
                            AttackTarget(card, player);

                           
                        }
                        break;
                    }


                }

            }
            canatEMfield = false;

            yield return new WaitForSeconds(0.2f);

        }

        //yield return new WaitForSeconds(0.5f);

        EMatackflag = true;

    }



    public IEnumerator EnemyTurn()
    {
        CardController[] PlayerFieldcardList = PlayerFieldTransform.GetComponentsInChildren<CardController>();


        //�����̃J�[�h���U���ł��Ȃ����鏈��
        foreach (var card in PlayerFieldcardList)
        {
            card.canAttack = false;

        }


        //�G�l�~�[�^�[���̏���
        Debug.Log("�G�̃^�[���ł�");
        manasys.AddMana(1);
        //manasys.AddEmptyMana(1);
        manasys.RestoreMana(EnemyCostframe);

        yield return new WaitForSeconds(1f);
        //EnemyHandtransform ����cardcontroller�����q�I�u�W�F�N�g������


        CardController[] EnemyFieldcardList = EnemyFieldTransform.GetComponentsInChildren<CardController>();


        //�U���t���b�O������������
        foreach (var card in EnemyFieldcardList)
        {
            card.canAttack = true;

        }


        int maxiteration = 5;
        int iteration = 0;
        // EMatackflag =false //�U���s�\�Ȃ�true��
        // EMcastflag =false�@//�L���X�g�s�\�Ȃ�true��
        while (iteration < maxiteration)
        {


            while (isAnimating)//�A�j���[�V���������ǂ������f
            {
                yield return null;
            }
            yield return StartCoroutine(EnemyCast());//�L���X�g�o����J�[�h���L���X�g���鏈��


            if (EnemyCasted)//��D����J�[�h���o�����ꍇ,
            {
                yield return new WaitForSeconds(1f);
            }


            while (isAnimating)
            {
                yield return null;
            }
            yield return StartCoroutine(EnemyAttack());//�U���ł���J�[�h�ōU�����鏈��

            if (EnemyCasted)//��D����J�[�h���o�����ꍇ,
            {
                yield return new WaitForSeconds(1f);
            }


            iteration += 1;
            Debug.Log($"iteration{iteration}");

        }

        EMatackflag = false;//�S�ď����̏�Ԃɖ߂�����g���ĂȂ�����,��Ŏg������
        EMcastflag = false;




        //�^�[�����I������
        yield return new WaitForSeconds(1f);


        ChangeTurn();



    }

    public void ChangeTurn()
    {
        //�^�[���̐؂�ւ�����
        isPlayerTurn = !isPlayerTurn;

        if (isPlayerTurn)
        {

            if (playerDeck.DeckCount != 0)
            {
                ICard playerCardEntity = playerDeck.DrawCard();
                CreateHand(PlayerHandTransform, playerCardEntity);


            }
            else
            {
                Debug.Log("player cant draw!!");
            }
            //�h���[����

            //���C�J�f�o�C�X����
            if (PlayerHERO == "RAIKA")
            {
                RAIKAdevice.OnTurnStart();
            }
           



        }
        else//����̃^�[��
        {
            //�h���[����

            if (enemyDeck.DeckCount != 0)
            {
                ICard enemyCardEntity = enemyDeck.DrawCard();
                CreateHand(EnemyHandTransform, enemyCardEntity);
            }
            else
            {
                Debug.Log("Enemy cant draw!!");
            }
            //���C�J�f�o�C�X����

            Debug.Log($"EnemyHERO{EnemyHERO}");

            if (EnemyHERO == "RAIKA")
            {
                RAIKAdevice.OnTurnStart();

                Debug.Log("MoonTurnStart!");
            }

        }
        TurnCalc();
    }









    public void CreateHand(Transform hand, ICard cardEntity)
    {
        CardController card;
        //�ꎞ�I�ɐe���f�b�L�ɂ���.
        //Transform deckTransform = BattleManager.Instance.PlayerDeckTransform;

        if (hand == PlayerHandTransform)//�����v���C���[�Ȃ�
        {
            card = Instantiate(cardPrefab, PlayerDeckTransform, false);


        }
        else//�G�Ȃ�
        {
            card = Instantiate(cardPrefab, EnemyDeckTransform, false);
        }




        //()�ɃJ�[�hID������
        card.Init(cardEntity);
        //controller��Monobehavior���p�����Ă���̂�get�ł���
        CardDrawAnimation cardAnim = card.GetComponent<CardDrawAnimation>();
        if (cardAnim != null)
        {
            //cardAnim.transform.SetParent(hand);
            StartCoroutine(cardAnim.Drawsetparent(cardAnim, hand));
        }






    }





    void CreateHero(Transform herofield, ICard cardEntity)
    {
        CardController card;

        if (herofield == PlayerHEROfield)//�����v���C���[�Ȃ�
        {
            card = Instantiate(cardPrefab, PlayerHEROfield, false);

            PlayerHerocon = card;

        }
        else//�G�Ȃ�
        {
            card = Instantiate(cardPrefab, EnemyHEROfield, false);
            EnemyHerocon = card;
        }

        //�J�[�h������
        card.Init(cardEntity);

    }








    public IEnumerator SettingHand()
    {
        //���ꂼ��̎�D��3���J�[�h��z��
        for (int i = 0; i < 3; i++)
        {


            ICard playerCardEntity = playerDeck.DrawCard();
            ICard enemyCardEntity = enemyDeck.DrawCard();
            CreateHand(PlayerHandTransform, playerCardEntity);
            CreateHand(EnemyHandTransform, enemyCardEntity);
            yield return new WaitForSeconds(0.2f);
        }

    }

    private ICard LoadCard(string hero, string cardType, int cardID)
    {

        //�ȉ����ȗ������邽�߂̃R�[�h
        //ICard EnemyCardEntity = Resources.Load<ZERO2minion>("CardEntityList/ZERO2/minions/minion " + 1);
        return Resources.Load<ScriptableObject>($"CardEntityList/{hero}/{cardType}/{cardType} {cardID}") as ICard;
    }


    private ICard LoadHeroCard(int cardID)
    {

        //PlayerHEROfield, EnemyHEROfield;
        return Resources.Load<ScriptableObject>($"CardEntityList/Heros/Heros {cardID}") as ICard;
    }

}
