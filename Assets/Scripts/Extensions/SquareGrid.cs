using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public class SquareGrid
    {
        private readonly int m_rows; // Número de filas
        private int m_columns;  // Número de columnas
        private Vector2 m_spacing; // Espacio entre cartas
        private Vector2 m_startPosition;  // Posición inicial de la cuadrícula
        private Vector2 m_offSet;
        public List<Vector2> Positions = new List<Vector2>(); 
        public SquareGrid(int rows, int columns, Vector2 spacing, Vector2 p_startPosition, Vector2 p_offSet = default)
        {
            m_rows = rows;
            m_columns = columns;
            m_spacing = spacing;
            m_offSet = p_offSet;
            // Calcula la posición inicial para centrar la cuadrícula
            m_startPosition = p_startPosition;
        }

        // Genera la lista de posiciones para la cuadrícula
        public List<Vector2> GenerateGridPositions()
        {
            Positions = new List<Vector2>();
            for (int row = 0; row < m_rows; row++)
            {
                for (int col = 0; col < m_columns; col++)
                {
                    // Calcula la posición para cada celda en la cuadrícula
                    Vector2 position = new Vector2(m_startPosition.x + col * m_spacing.x, m_startPosition.y - row * m_spacing.y) + m_offSet;
                    Positions.Add(position);
                }
            }

            return Positions;
        }
        
        public static List<Vector2> CreateGridPositions(int p_rows, int p_colums, Vector2 p_spacing, Vector2 p_startPosition)
        {
            List<Vector2> positions = new List<Vector2>();

            for (int row = 0; row < p_rows; row++)
            {
                for (int col = 0; col < p_colums; col++)
                {
                    // Calcula la posición para cada celda en la cuadrícula
                    Vector2 position = new Vector2(p_startPosition.x + col * p_spacing.x, p_startPosition.y - row * p_spacing.y);
                    positions.Add(position);
                }
            }

            return positions;
        }
    }
}