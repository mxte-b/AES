using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AES.Components
{
    public class Word : IEquatable<Word>
    {
        private readonly byte[] buffer = new byte[4];

        public byte this[int index]
        {
           get => buffer[index];
           set => buffer[index] = value;
        }

        public static Word operator ^(Word a, Word b)
        {
            return new Word(
                (byte)(a[0] ^ b[0]),
                (byte)(a[1] ^ b[1]),
                (byte)(a[2] ^ b[2]),
                (byte)(a[3] ^ b[3])
            );
        }

        public byte[] GetBuffer() => (byte[])buffer.Clone();
        public byte[] ToArray() => (byte[])buffer.Clone();

        public int Length
        {
            get
            {
                return buffer.Length;
            }
        }

        public Word(byte b0, byte b1, byte b2, byte b3)
        {
            buffer = [b0, b1, b2, b3];
        }

        public Word(byte[] buffer)
        {
            if (buffer.Length != 4)
            {
                throw new ArgumentException("Word buffer must exactly be 4 bytes");
            }
            Array.Copy(buffer, this.buffer, 4);
        }

        public Word()
        {
            buffer = new byte[4];
        }

        public override string ToString()
        {
            return $"[{String.Join(",", buffer.Select(x => x.ToString("X2")))}]";
        }

        public bool Equals(Word? other)
        {
            if (other == null)
            {
                return false;
            }

            return buffer.SequenceEqual(other.buffer);
        }

        public override bool Equals(object? obj) => Equals(obj as Word);

        public override int GetHashCode() => HashCode.Combine(buffer[0], buffer[1], buffer[2], buffer[3]);
    }
}
