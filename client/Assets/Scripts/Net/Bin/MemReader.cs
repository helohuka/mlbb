using System;

namespace bin
{
    public class MemReader : bin.IReader
    {
        private MemReader() {}
    	public MemReader(byte[] b, uint l)
    	{
            buffer_ = b;
            length_ = l;
            rdptr_ = 0;
        }

    	public bool read(uint len, out byte[] data, out int startId)
        {
            data = buffer_;
            startId = (int)rdptr_;
    		if(length_ < rdptr_ + len)
    			return false;
    		rdptr_ += len;
            return true;
        }

        private byte[] buffer_;
        private uint length_ = 0;
        private uint rdptr_ = 0;
    }
}