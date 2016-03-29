namespace MinAsm.Encoding
{
    public enum Prefix : byte
    {
        None = 0,
        RexW = 0x48,
        P66  = 0x66,
    }
}
