namespace bin 
{
    /** Abstract interface for writing binary data. */
    public interface IWriter
    {
        /** Write some binary data. */
        void write(byte[] data);
    }
}
