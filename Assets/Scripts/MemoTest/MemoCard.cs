using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace MemoTest
{
    public class MemoCard : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private Sprite backCard;
        [SerializeField] private Vector3 maxScale;
        [SerializeField] private float scaleSpeed = 1;
        [SerializeField] private float fadeSpeed = 1;
        
        public int CardId { get; private set; }
        public bool IsDiscovered { get; private set; }

        private Sprite m_realCard;

        public SpriteRenderer GetRenderer() => renderer;
        
        public void Initialize(Vector2 p_pos, Sprite p_sprite, int p_CardId)
        {
            transform.position = p_pos;
            m_realCard = p_sprite;
            CardId = p_CardId;
        }

        public void Initialize(Sprite p_sprite, int p_CardId)
        {
            m_realCard = p_sprite;
            CardId = p_CardId;
        }

        private bool m_playExpansion;
        private void Update()
        {
            if (!m_playExpansion)
                return;

            transform.localScale *= 1.01f * scaleSpeed;

            if (transform.localScale.magnitude > maxScale.magnitude * 0.7f)
            {
                var prevColor = renderer.color;
                prevColor.a *= 0.9f * fadeSpeed;
                renderer.color = prevColor;
            }

            if(renderer.color.a <= 0.05)
                Destroy(gameObject);
        }

        public void Expand()
        {
            m_playExpansion = true;
        }

        public void DiscoverCard()
        {
            renderer.sprite = m_realCard;
            IsDiscovered = true;
        }

        public void HideCard()
        {
            renderer.sprite = backCard;
            IsDiscovered = false;
        }
    }
}