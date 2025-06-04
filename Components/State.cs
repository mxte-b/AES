using Crypto.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Components
{
    public class State : IEquatable<State>
    {
        // Nb is defined here too, just for clarity.
        // It is also present in the KeyConstants class.
        // For AES, the number of blocks in the state is always 4.
        private const int Nb = 4;

        private readonly byte[,] buffer = new byte[Nb, 4];

        public byte this[int row, int col]
        {
            get => buffer[row, col];
            set => buffer[row, col] = value;
        }

        /// <summary>
        /// Gets the column as a Word at a specified index.
        /// </summary>
        /// <param name="col">The index of the column</param>
        /// <returns>The bytes of the column as a Word instance.</returns>
        public Word GetColumn(int col)
        {
            return new Word(buffer[0, col], buffer[1, col], buffer[2, col], buffer[3, col]);
        }

        /// <summary>
        /// Sets the internal buffer's column from a word at a specified index.
        /// </summary>
        /// <param name="col">The index of the column that will be set</param>
        /// <param name="value">The value of the column</param>
        public void SetColumn(int col, Word value)
        {
            for (int i = 0; i < Nb; i++)
            {
                buffer[i, col] = value[i];
            }
        }

        /// <summary>
        /// Gets the row as a Word at a specified index.
        /// </summary>
        /// <param name="row">The index of the row</param>
        /// <returns>The bytes of the row as a Word instance.</returns>
        public Word GetRow(int row)
        {
            return new Word(buffer[row, 0], buffer[row, 1], buffer[row, 2], buffer[row, 3]);
        }

        public int GetLength(int dimension) => buffer.GetLength(dimension);

        public byte[,] GetBuffer() => (byte[,])buffer.Clone();

        public byte[] ToArray()
        {
            byte[] bytes = new byte[4 * Nb];

            for (int col = 0; col < 4; col++) 
            {
                for (int row = 0; row < Nb; row++)
                {
                    bytes[col * 4 + row] = buffer[row, col];
                }
            }

            return bytes;
        }

        public State(byte[,] buffer)
        {
            if (buffer.GetLength(0) != Nb || buffer.GetLength(1) != 4)
            {
                throw new ArgumentException($"The dimensions of the buffer must be {Nb}x4");
            }

            this.buffer = (byte[,])buffer.Clone();
        }

        public State()
        {
            buffer = new byte[Nb, 4];
        }

        public State(byte[] bytes)
        {
            if (bytes.Length != 16)
            {
                throw new ArgumentException("The number of bytes must equal 16");
            }

            byte[,] buffer = new byte[Nb, 4];

            for (int col = 0; col < Nb; col++)
            {
                for (int row = 0; row < 4; row++)
                {
                    buffer[row, col] = bytes[col * 4 + row];
                }
            }

            this.buffer = buffer;
        }

        public State Clone() => new State((byte[,])buffer.Clone());

        public bool Equals(State? other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            for (int row = 0; row < 4; row++)
            {
                if (!GetRow(row).Equals(other.GetRow(row)))
                {
                    return false;
                }
            }
            return true;
        }
        public override bool Equals(object? obj) => Equals(obj as State);

        public override int GetHashCode() => Enumerable.Range(0, 4).Select(i => GetRow(i).GetHashCode()).Aggregate((h1, h2) => HashCode.Combine(h1, h2));

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            for (int row = 0; row < 4; row++)
            {
                sb.AppendLine(GetRow(row).ToString());
            }

            return sb.ToString();
        }
    }
}
