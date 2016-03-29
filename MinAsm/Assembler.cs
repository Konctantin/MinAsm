using System;
using System.Collections;
using System.Collections.Generic;
using MinAsm.Encoding;

namespace MinAsm
{
    public abstract partial class Assembler
        : IEnumerable<Instruction>
    {
        public Context Context { get; }

        readonly List<Instruction> m_instructions = new List<Instruction>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="architecture"></param>
        /// <param name="offset"></param>
        protected Assembler(Architectures architecture, long offset)
        {
            Context = new Context(architecture, offset);
            m_instructions.Clear();
        }

        void Add(Instruction instruction) => m_instructions.Add(instruction);

        public void Bind()
        {
            throw new NotImplementedException();
        }

        public void Reset() => m_instructions.Clear();

        IEnumerator IEnumerable.GetEnumerator() => m_instructions.GetEnumerator();

        IEnumerator<Instruction> IEnumerable<Instruction>.GetEnumerator() => m_instructions.GetEnumerator();
    }
}
