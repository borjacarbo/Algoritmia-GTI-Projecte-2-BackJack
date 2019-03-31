using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();
//        Test_InitCardValues();
    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        
        /* DONE */
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */

        for (int i = 0; i < 51; i++)
        {
            if (i % 13 == 0)
            {
                values[i] = 11;
            }
            else
            {
                if ((i % 13) >= 10)
                {
                    values[i] = 10;
                }
                else
                {
                    values[i] = (i % 13) + 1;
                }
            }  
        }
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */       
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();


            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }


        // Test_PlayerGameWithoutShuffle_Loose(1);
        // Test_PlayerGameWithoutShuffle_Stand(1);
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();

    }

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer cuando se plantar el jugador)
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        // Test_PlayerGameWithoutShuffle_Loose(2);
        // Test_PlayerGameWithoutShuffle_Stand(2);

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */                
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }

    /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    Testing functionalities
    ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/



    private void Test_InitCardValues()
    {

        for (int i = 0; i < 51; i++)
        {
            Debug.Log("Carta: " + i.ToString() + " has value " + values[i].ToString());
        }
    }


    private void Test_PlayerGameWithoutShuffle_Loose(int init)
    {

        int playerPoints = player.GetComponent<CardHand>().points;

        if (init == 1)
        {
            Debug.Log("Initially Player has Points: " + playerPoints.ToString());
            Hit();
            Debug.Log("Player has Points: " + playerPoints.ToString());

        }
        else
        {
            Debug.Log("Player has Points: " + playerPoints.ToString());

            if (playerPoints < 21)
            {
                Hit();
            }
            else
            {
                if (playerPoints == 21)
                {
                    Debug.Log("------ Player has won  ---------------- ");
                }
                else
                {
                    Debug.Log("------ Player has lost with " + playerPoints.ToString() + " ---------------- ");

                }
            }

        }
    }


    private List<GameObject> cardList;

    private void Test_PlayerGameWithoutShuffle_Stand(int init)
    {

        int playerPoints = player.GetComponent<CardHand>().points;

        if (init == 1)
        {
            Debug.Log("Initially Player has Points: " + playerPoints.ToString());
            Hit();
            Debug.Log("Player has Points: " + playerPoints.ToString());

        }
        else
        {
            Debug.Log("Player has Points: " + playerPoints.ToString());

            Debug.Log("going to calculate future points");

            //Calculamos la puntuación previa de nuestra mano


            Debug.Log("Next card will be  : " + values[cardIndex].ToString());
            int valFutur = 0;
            int aces = 0;
            cardList = player.GetComponent<CardHand>().cards;
            foreach (GameObject f in cardList)
            {

                if (f.GetComponent<CardModel>().value != 11)
                    valFutur += f.GetComponent<CardModel>().value;
                else
                    aces++;
            }

            if (values[cardIndex] == 11)
            {
                aces++;
            }
            else
            {
                valFutur = valFutur + values[cardIndex];
            }

            for (int i = 0; i < aces; i++)
            {
                if (valFutur + 11 <= 21)
                {
                    valFutur = valFutur + 11;
                }
                else
                {
                    valFutur = valFutur + 1;
                }
            }

            Debug.Log("Next points will be  : " + valFutur.ToString());

            if (valFutur < 21)
            {
                Hit();
            }
            else
            {
                Debug.Log("------ Player stands after card : " + values[cardIndex -1]);

                Debug.Log("------ Player stands with : " + playerPoints.ToString() + " points  ---------------- ");
                Stand();
            }

        }

    }
}

