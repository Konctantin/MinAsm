namespace MinAsm
{
    public class Context
    {
        public Architectures Architecture { get; set; }
        public long Offset { get; }

        internal Context(Architectures architecture = Architectures.X86, long offset = 0L)
        {
            Architecture = architecture;
            Offset = offset;
        }
    }
}
