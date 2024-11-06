using System.Collections.Generic;
using MemoTest;
using UnityEngine;

namespace Assets.Scripts.MemoTest
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> menuObjects = new List<GameObject>();
        [SerializeField] MemoTestController controller;

        private int m_cardsAmount = 6;
        private int m_columsAmount = 2;
        private float m_cameraSize = 20f;
        public void StartGame()
        {
            for (int i = 0; i < menuObjects.Count; i++)
            {
                menuObjects[i].SetActive(false);
            }

            controller.StartGame(m_cardsAmount, m_columsAmount);
            Camera.main.orthographicSize = m_cameraSize;
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
    }
}
