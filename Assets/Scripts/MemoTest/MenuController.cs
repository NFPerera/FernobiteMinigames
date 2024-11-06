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
        public void StartGame()
        {
            for (int i = 0; i < menuObjects.Count; i++)
            {
                menuObjects[i].SetActive(false);
            }

            controller.StartGame(m_cardsAmount, m_columsAmount);
        }


        public void ChangeCardAmount(int p_cardAmmount)
        {
            m_cardsAmount = p_cardAmmount;
        }

        public void ChangeColumnsAmount(int p_columsAmount)
        {
            m_columsAmount = p_columsAmount;
        }
    }
}
