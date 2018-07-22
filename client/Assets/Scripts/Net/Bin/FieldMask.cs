namespace bin
{
    /** FieldMask is an object that is used to indicate whether an bin field 
        is present in an bin protocol diagram. The purpose of using this object
        is to keep protocol diagram small as possible.
     */
    public struct FieldMask
    {
        /** Constructor. */
        public FieldMask(byte[] m)
        {
            masks_ = m;
            pos_ = 0;
        }

        /** Write to next bit. */
        public void writeBit(bool b)
        {
            if (b)
                masks_[pos_ >> 3] |= (byte)(128 >> (int)(pos_ & 0X00000007));
            pos_++;
        }

        /** Read from next bit. */
        public bool readBit()
        {
            bool r = ((masks_[pos_ >> 3] & (byte)(128 >> (int)(pos_ & 0X00000007))) != 0)? true : false;
            pos_++;
            return r;
        }

        /** Get internal mask bits. */
        public byte[] getBits() { return masks_;  }

        private byte[] masks_;
        private uint pos_;
    }
}
