using System.Collections.Generic;
using MemoTest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MemoTest
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> menuObjects = new List<GameObject>();
        [SerializeField] MemoTestController controller;

        [SerializeField] private Button easyButton;
        [SerializeField] private Button mediumButton;
        [SerializeField] private Button expertButton;
        [SerializeField] private GameObject winGameObject;
        [SerializeField] private Animation winAnimation;

        private float m_animTimer;
        private int m_cardsAmount = 6;
        private int m_columsAmount = 2;
        private float m_cameraSize = 20f;

        private void Start()
        {
            easyButton.Select();
        }


        private void Update()
        {
            if(m_animTimer <= 0f)
                return;


            m_animTimer -= Time.deltaTime;

            if (m_animTimer < 0.2f)
            {
                winGameObject.SetActive(false);
                easyButton.Select();
                m_cardsAmount = 6;
                m_columsAmount = 2;
                m_cameraSize = 20f;

                for (int i = 0; i < menuObjects.Count; i++)
                {
                    menuObjects[i].SetActive(true);
                }
            }
        }
        public void StartGame()
        {
            for (int i = 0; i < menuObjects.Count; i++)
            {
                menuObjects[i].SetActive(false);
            }

            controller.StartGame(m_cardsAmount, m_columsAmount);
            Camera.main.orthographicSize = m_cameraSize;
        }

        public void ToggleEasyButton()
        {
        }
        public void ChangeCardAmount(int p_cardAmmount)
        {
            m_cardsAmount = p_cardAmmount;
        }

        public void ChangeColumnsAmount(int p_columsAmount)
        {
            m_columsAmount = p_columsAmount;
        }

        public void ChangeCameraSize(float p_f) => m_cameraSize = p_f;

        public void ShowWinAnim()
        {
            winGameObject.SetActive(true);
            winAnimation.Play();
            m_animTimer = 3f;
        }
    }
}
