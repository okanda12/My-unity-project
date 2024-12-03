using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{


    //カードのプレファブを入れる
    [SerializeField] CardController cardPrefab;
    //ヒーローのプレファブを入れる
    // [SerializeField] heroPrefab;

    //手札のTransformを入れる
    public Transform PlayerHandTransform, EnemyHandTransform;
    public Transform PlayerFieldTransform, EnemyFieldTransform;
    public Transform PlayerHEROfield, EnemyHEROfield;
    public Transform PlayerDeckTransform, EnemyDeckTransform;
    public Transform PlayerCostframe, EnemyCostframe;
    public Transform canvas2; //攻撃するときにカードが一番上に表示されるように移動するキャンバス
    public Transform MagiCasPlace;//魔法をキャストする場所


    private Deck playerDeck;
    private Deck enemyDeck;

    //プレイヤーのターンかの判定
    public bool isPlayerTurn;//Turndisplayerに出力

    public string PlayerHERO = "ARMA";
    public string EnemyHERO = "ZERO2";


    public int Player_Mana;
    public int Player_maxMana;

    public int Enemy_Mana;
    public int Enemy_maxMana;

    //manasystemとかの方が良くない？
    public Manasystem manasys;



    //特殊デバイス
    private RAIKAdevice RAIKAdevice;


    public CardController selectedCard;//現在選択されているカード
    public List<CardController> enemyCards;//敵カードのリスト

    public CardController PlayerHerocon;
    public CardController EnemyHerocon;


    //敵のターン,以下2つのフラッグがtrueになったらターン終了
    private bool EMcastflag = false;//キャストできるカードがないとtrue;
    private bool EMatackflag = false; //アタックできるカードが無いとtrue;




    //シングルトンにするための呪文
    public static BattleManager Instance { get; private set; }

    private void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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


        StartGame();



        isPlayerTurn = true;
        TurnCalc();
    }

    private void StartGame()
    {

        HeroAppears();





        StartCoroutine(SettingHand());
        manasys.InitializeMana(0);

        RAIKAdevice = FindObjectOfType<RAIKAdevice>();
    }


    public void EnemyFieldHighlight(bool YES)
    {//敵の情報を取得.そいつらにハイライトをつける

        //ちな孫も追えます
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



    //カードが選ばれたときの処理
    //cardには攻撃元カードが入ってくる
    public void SelectCardToAttack(CardController card, string isAttacker)
    {

        if (isAttacker == "Attacker")
        {
            //最初にカードを選ぶ
            selectedCard = card;
            if (selectedCard.canAttack == true)
            {

                Debug.Log("攻撃者カード選択:" + selectedCard.model.name);//お名前を取得
                EnemyFieldHighlight(true);//ハイライトを付けます.
            }
            else
            {
                Debug.Log(selectedCard.model.name + "can't Attack!");

                CardAttackAnimation cantAttackAnim = card.GetComponent<CardAttackAnimation>();

                StartCoroutine(cantAttackAnim.CantAttack(card.transform));




            }


        }
        else if (isAttacker == "Defender" && selectedCard != null)//ここに条件を追加
        {

            if (selectedCard.canAttack == true)
            {
                //既にカードが選ばれている場合はターゲット選択
                AttackTarget(selectedCard, card);//攻撃者　守備者の順
                Debug.Log("被攻撃者カード選択:" + card.model.name);

                selectedCard = null;//攻撃後は選択を解除

                EnemyFieldHighlight(false);//ハイライトを消します
            }
            else
            {
                Debug.Log(selectedCard.model.name + "can't Attack!");

                //防御者側にはいらなかったわ
                //CardAttackAnimation cantAttackAnim = card.GetComponent<CardAttackAnimation>();

                //StartCoroutine(cantAttackAnim.CantAttack(card.transform));

            }


        }

    }

    public void AttackTarget(CardController attacker, CardController target)
    {
        //Transform targetparent = target.transform.parent;
        //ターゲットが敵カードであれば攻撃する
        //if (targetparent == EnemyFieldTransform || targetparent == EnemyHEROfield)
        //{
        attacker.Attack(target);//攻撃メソッド呼び出し

        Debug.Log($"{attacker.model.name}が{target.model.name}を攻撃しました");
        // }

    }





    private void HeroAppears()
    {



        ICard Enemycard = LoadHeroCard(2);//02
        ICard Playercard = LoadHeroCard(1);//ARMA

        CreateHero(PlayerHEROfield, Playercard);
        CreateHero(EnemyHEROfield, Enemycard);




        playerDeck = CreateDeck("ARMA");
        enemyDeck = CreateDeck("ZERO2");

    }

    private Deck CreateDeck(string hero)
    {
        Deck deck = new Deck();

        //minionとmagicのカードをデッキに追加
        for (int i = 1; i <= 3; i++)//3枚のミニオンを追加
        {
            ICard card = LoadCard(hero, "minions", i);
            if (card != null) deck.AddCard(card);
        }
        for (int i = 1; i <= 3; i++)//3枚のmagicを追加
        {
            ICard card = LoadCard(hero, "magics", i);
            if (card != null) deck.AddCard(card);
        }
        deck.Shuffle();//シャッフル

        Debug.Log(deck.DeckCount);
        deck.PrintCardNames();//デバック用関数

        return deck;

    }

    void TurnCalc()
    {
        //ターンの進行の処理をする.
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
        //プレイヤーターンの処理
        Debug.Log("プレイヤーのターンです");
        manasys.AddMana(1);
        manasys.AddmaxMana(1);
        manasys.RestoreMana(PlayerCostframe);

        CardController[] PlayerFieldcardList = PlayerFieldTransform.GetComponentsInChildren<CardController>();


        //攻撃フラッグを初期化する
        foreach (var card in PlayerFieldcardList)
        {
            card.canAttack = true;

        }

    }







    //敵のキャストセクション
    public IEnumerator EnemyCast()
    {
        Debug.Log("Enemycast!");

        CardController[] EMhandCC = EnemyHandTransform.GetComponentsInChildren<CardController>();//ハンドのカードコントローラ
        CardController[] EMfieldCC = EnemyFieldTransform.GetComponentsInChildren<CardController>();//フィールドのコントローラー


        //もし手札に何もなかったら終了

        if (EMhandCC.Length == 0)
        {
            EMcastflag = true;
            Debug.Log("EMhandCC is empty");
            yield return new WaitForSeconds(0.5f);
            yield break; // コルーチンを終了
        }

        int maxIterations = EMhandCC.Length; // 無限ループ防止
        int iterationCount = 0;


        //maxIterationまで以下を繰り返す.相手がちょっと考えるような動作をとるときはこれが原因

        while (iterationCount < maxIterations)
        {


            bool cardCasted = false;




            foreach (CardController card in EMhandCC)//ハンドを全てみて　
            {
                if (card.model.cost <= Enemy_Mana && EMfieldCC.Length < 5)//手札のカード一枚を見ています
                {
                    
                    //フィールドの状態を見ている
                    List<Transform> NocardDaiza = new List<Transform>();//ここにオブジェクトを持たない台座を追加していく


                    foreach (Transform child in EnemyFieldTransform)//子オブジェクトを持たないdaizaを取得していく
                    {
                        if (child.childCount == 0) // 子オブジェクトを持たないなら葉ノード
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
                    
                    cardCasted = true;


                    yield return new WaitForSeconds(0.5f);
                    break; // foreach を抜ける


                }
            }

            // 手札・フィールドを更新
            EMhandCC = EnemyHandTransform.GetComponentsInChildren<CardController>();
            EMfieldCC = EnemyFieldTransform.GetComponentsInChildren<CardController>();

            // 出せるカードがなくなったら終了
            if (!cardCasted) break;

            iterationCount++;
        }
        yield return new WaitForSeconds(0.1f);
        EMcastflag = true;



    }


    //敵の攻撃命令です
    public IEnumerator EnemyAttack()
    {
        Debug.Log("EnemyAttack!");
        bool canatEMfield = true;//何か攻撃できるカードがいるとき


        while (canatEMfield != false)
        {
            canatEMfield = false;

            CardController[] EMfieldCC = EnemyFieldTransform.GetComponentsInChildren<CardController>();//フィールドのコントローラー
            CardController[] PLfieldCC = PlayerFieldTransform.GetComponentsInChildren<CardController>();//プレイヤーフィールド


            if (EMfieldCC.Length == 0) yield break;


            if (EMfieldCC != null)
            {
                foreach (CardController card in EMfieldCC)
                {
                    EMfieldCC = EnemyFieldTransform.GetComponentsInChildren<CardController>();//フィールドのコントローラー
                    PLfieldCC = PlayerFieldTransform.GetComponentsInChildren<CardController>();//プレイヤーフィールド


                    if (card.canAttack == true)
                    {
                        Debug.Log($"PLfieldCC{PLfieldCC},PLfieldCC.length{PLfieldCC.Length}");
                        if (PLfieldCC.Length > 0)
                        {
                            CardController EnemyCard = PLfieldCC[0];//いちばん端っこの敵(味方)を攻撃
                            AttackTarget(card, EnemyCard);

                            yield return new WaitForSeconds(0.5f);


                        }
                        else//攻撃できない
                        {
                            CardController player = PlayerHEROfield.GetComponentInChildren<CardController>();//ヒーろーを攻撃
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


        //自分のカードを攻撃できなくする処理
        foreach (var card in PlayerFieldcardList)
        {
            card.canAttack = false;

        }


        //エネミーターンの処理
        Debug.Log("敵のターンです");
        manasys.AddMana(1);
        manasys.AddmaxMana(1);
        manasys.RestoreMana(EnemyCostframe);

        yield return new WaitForSeconds(1f);
        //EnemyHandtransform からcardcontrollerを持つ子オブジェクトを検索


        CardController[] EnemyFieldcardList = EnemyFieldTransform.GetComponentsInChildren<CardController>();


        //攻撃フラッグを初期化する
        foreach (var card in EnemyFieldcardList)
        {
            card.canAttack = true;

        }


        int maxiteration = 5;
        int iteration = 0;
        // EMatackflag =false //攻撃不可能ならtrueに
        // EMcastflag =false　//キャスト不可能ならtrueに
        while (iteration < maxiteration)
        {

            yield return StartCoroutine(EnemyCast());//キャスト出来るカードをキャストする処理
            yield return StartCoroutine(EnemyAttack());//攻撃できるカードで攻撃する処理
            iteration += 1;
            Debug.Log($"iteration{iteration}");
        }

        EMatackflag = false;//全て初期の状態に戻す現状使ってないけど,後で使うかも
        EMcastflag = false;







        //ターンを終了する
        yield return new WaitForSeconds(1f);


        ChangeTurn();



    }

    public void ChangeTurn()
    {
        //ターンの切り替え処理
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
            //ドローする

            //ライカデバイス発動
            RAIKAdevice.OnTurnStart();



        }
        else
        {
            //ドローする

            if (enemyDeck.DeckCount != 0)
            {
                ICard enemyCardEntity = enemyDeck.DrawCard();
                CreateHand(EnemyHandTransform, enemyCardEntity);
            }
            else
            {
                Debug.Log("Enemy cant draw!!");
            }
            //ライカデバイス発動



        }
        TurnCalc();
    }









    void CreateHand(Transform hand, ICard cardEntity)
    {
        CardController card;
        //一時的に親をデッキにする.
        //Transform deckTransform = BattleManager.Instance.PlayerDeckTransform;

        if (hand == PlayerHandTransform)//もしプレイヤーなら
        {
            card = Instantiate(cardPrefab, PlayerDeckTransform, false);


        }
        else//敵なら
        {
            card = Instantiate(cardPrefab, EnemyDeckTransform, false);
        }




        //()にカードIDを入れる
        card.Init(cardEntity);
        //controllerはMonobehaviorを継承しているのでgetできる
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

        if (herofield == PlayerHEROfield)//もしプレイヤーなら
        {
            card = Instantiate(cardPrefab, PlayerHEROfield, false);

            PlayerHerocon = card;

        }
        else//敵なら
        {
            card = Instantiate(cardPrefab, EnemyHEROfield, false);
            EnemyHerocon = card;
        }

        //カード初期化
        card.Init(cardEntity);

    }








    public IEnumerator SettingHand()
    {
        //それぞれの手札に3枚カードを配る
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

        //以下を簡略化するためのコード
        //ICard EnemyCardEntity = Resources.Load<ZERO2minion>("CardEntityList/ZERO2/minions/minion " + 1);
        return Resources.Load<ScriptableObject>($"CardEntityList/{hero}/{cardType}/{cardType} {cardID}") as ICard;
    }


    private ICard LoadHeroCard(int cardID)
    {

        //PlayerHEROfield, EnemyHEROfield;
        return Resources.Load<ScriptableObject>($"CardEntityList/Heros/Heros {cardID}") as ICard;
    }

}
