using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.MemoTest;
using Extensions;
using Extensions.IENumerableExtensions;
using Extensions.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MemoTest
{
    public class MemoTestController : MonoBehaviour
    {
        [SerializeField] private MenuController menuController;
        [Header("MemoTest")]
        [SerializeField] private MemoCard memoCardPrefab;
        [SerializeField] private List<Sprite> levelSprites = new List<Sprite>();
        [SerializeField] private Vector2 cardSize;
        [SerializeField] private LayerMask cardLayerMask;
        [Header("Grid")] 
        [SerializeField] private Vector2 gridCenterPos;
        [Header("Sounds")] 
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip touchSound;
        [SerializeField] private AudioClip matchSound;
        [SerializeField] private AudioClip winSound;
        [Header("Particles")]
        [SerializeField] private ParticleSystem matchParticles;
        [SerializeField] private ParticleSystem winParticles;

        private bool m_isPlayingAnim;
        private int m_pairsLeft;
        private List<MemoCard> m_allCards = new List<MemoCard>();
        private SquareGrid m_grid;
        
        private Action<MemoCard> m_onCardSelected;
        private Action m_onMatchCards;

        public void StartGame(int p_totalCards, int p_maxColumns)
        {
            Create(p_totalCards, p_maxColumns);
            ShuffleCards();
            SubscribeInputs();
            SubscribeEvents();
        }

        public void EndGame()
        {
            UnsubscribeInputs();
            UnSubscribeEvents();

            m_isPlayingAnim =false;
            m_pairsLeft = 0;
            m_allCards.Clear();
            m_grid = null;
        }

        private void OnDisable()
        {
            UnsubscribeInputs();
            UnSubscribeEvents();
        }

        private void SubscribeInputs()
        {
            var l_ins = ExtensionsInputManager.Instance;
            l_ins.SubscribeInput("OnTouchScreen", OnTouchScreenPerformed );
            l_ins.SubscribeInput("OnLeftClick", OnLeftClickPerformed);
        }

        private void UnsubscribeInputs()
        {
            var l_ins = ExtensionsInputManager.Instance;
            l_ins.UnsubscribeInput("OnTouchScreen", OnTouchScreenPerformed );
            l_ins.UnsubscribeInput("OnLeftClick", OnLeftClickPerformed);
        }

        private void SubscribeEvents()
        {
            m_onMatchCards += MatchCardsParticles;
            m_onMatchCards += CheckWinCondition;
            m_onCardSelected += OnSelectedCard;
        }

        private void UnSubscribeEvents()
        {
            m_onMatchCards -= MatchCardsParticles;
            m_onMatchCards -= CheckWinCondition;
            m_onCardSelected -= OnSelectedCard;
        }

        private void Update()
        {
            if (m_isPlayingAnim)
            {
                var p_dirOne = (m_middlePoint - m_cardOne.transform.position).normalized;
                m_cardOne.transform.position += p_dirOne * Time.deltaTime * cardSpeed;

                var p_dirTwo = (m_middlePoint - m_cardTwo.transform.position).normalized;
                m_cardTwo.transform.position += p_dirTwo * Time.deltaTime * cardSpeed;

                if (Vector2.Distance(m_cardOne.transform.position, m_cardTwo.transform.position) <= 0.2)
                {
                    //play anim de la carta
                    m_cardOne.Expand();
                    Destroy(m_cardTwo.gameObject);
                    m_isPlayingAnim = false;
                    SubscribeInputs();
                    m_onMatchCards.Invoke();
                }
            }
        }
        private void OnLeftClickPerformed(InputAction.CallbackContext p_obj)
        {
            var l_mousePos = Mouse.current.position.ReadValue();
            RayToPos(l_mousePos);

        }

        private void OnTouchScreenPerformed(InputAction.CallbackContext p_obj)
        {
            var l_touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            RayToPos(l_touchPos);
        }

        private void RayToPos(Vector2 p_pos)
        {
            Ray l_ray = Camera.main.ScreenPointToRay(p_pos);  // Crea un rayo desde la cámara
            RaycastHit l_hit;
            
            if (Physics.Raycast(l_ray, out l_hit,10, cardLayerMask))
            {
                MemoCard l_selectedCard = l_hit.transform.GetComponent<MemoCard>();  
                
                
                if(l_selectedCard.IsDiscovered)
                    return;
                
                l_selectedCard.DiscoverCard();
                m_onCardSelected.Invoke(l_selectedCard);
            }
        }

        private MemoCard m_selectedCard = default;
        private void OnSelectedCard(MemoCard p_card)
        {
            audioSource.PlayOneShot(touchSound);
            if (m_selectedCard == default)
            {
                m_selectedCard = p_card;
                return;
            }

            if (m_selectedCard.CardId == p_card.CardId) //Esto significa que las cartas son las mismas
            {
                //Agregar los puntos que sean necesarios
                m_pairsLeft--;
                AnimCards(m_selectedCard, p_card);
                m_selectedCard = default;
                
                
            }
            else
            {
                UnsubscribeInputs();
                StartCoroutine(ShowAndHideCards(m_selectedCard, p_card));
                m_selectedCard = default;
            }
        }
        
        private IEnumerator ShowAndHideCards(MemoCard card1, MemoCard card2)
        {
            yield return new WaitForSeconds(1f);
            SubscribeInputs();
            card1.HideCard();
            card2.HideCard();
        }

        [SerializeField] private float cardSpeed;
        private Vector3 m_middlePoint;
        private MemoCard m_cardOne;
        private MemoCard m_cardTwo;

        private void AnimCards(MemoCard p_carOne, MemoCard p_cardTwo)
        {
            m_cardOne = p_carOne;
            m_cardTwo = p_cardTwo;

            m_cardOne.GetRenderer().sortingOrder = 10;
            m_cardTwo.GetRenderer().sortingOrder = 10;
            m_middlePoint = (p_cardTwo.transform.position + p_carOne.transform.position)/2;
            m_isPlayingAnim = true;
            UnsubscribeInputs();
        }

        private void MatchCardsParticles()
        {
            audioSource.PlayOneShot(matchSound);
        }

        private void CheckWinCondition()
        {
            Debug.Log(m_pairsLeft);
            if(m_pairsLeft > 0)
                return;

            audioSource.PlayOneShot(winSound);

            Debug.Log("YOU WIN");
            menuController.ShowWinAnim();
            EndGame();

        }

        [ContextMenu("Shuffle")]
        private void ShuffleCards()
        {
            m_allCards.Shuffle();
            for (int l_i = 0; l_i < m_allCards.Count; l_i++)
            {
                m_allCards[l_i].transform.position = m_grid.Positions[l_i];
            }
        }

        [ContextMenu("Create")]
        private void Create(int p_totalCards, int p_maxColums)
        {
            m_pairsLeft = p_totalCards / 2;
            m_allCards = new List<MemoCard>();
            var l_currentRows = Mathf.CeilToInt(p_totalCards / p_maxColums); //Veo cuantas filas necesito y las redondeo hacia arriba


            for (int l_i = 0; l_i < m_pairsLeft; l_i++)
            {
                var l_card1 = Instantiate(memoCardPrefab);
                l_card1.Initialize(levelSprites[l_i], l_i); // Inicializa la carta con el sprite
                m_allCards.Add(l_card1);

                // Segunda carta (pareja)
                var l_card2 = Instantiate(memoCardPrefab);
                l_card2.Initialize(levelSprites[l_i], l_i);
                m_allCards.Add(l_card2);
            }
            
            var l_supLeftCorner = gridCenterPos + 
                                  ((Vector2.left * p_maxColums * cardSize.x) + (Vector2.up * l_currentRows * cardSize.y))/2;

            
            m_grid = new SquareGrid(l_currentRows, p_maxColums, cardSize, l_supLeftCorner, new Vector2(cardSize.x/2, cardSize.y/2) + Vector2.down*cardSize.y);
            
            
            var l_grid = m_grid.GenerateGridPositions();
            
            for (int l_i = 0; l_i < m_allCards.Count; l_i++)
            {
                m_allCards[l_i].transform.position = l_grid[l_i];
                m_allCards[l_i].HideCard();
            }

            
        }
    }
}
