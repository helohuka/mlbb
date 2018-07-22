namespace bin 
{
    /** Abstract interface for reading binary data. */
    public interface IReader
    {
        /** 
         * @param size data size want to read.
         * @param[out] data buffer.
         * @param[out] data buffer start id.
         */
        bool read(uint size, out byte[] data, out int startId);
    }
}
