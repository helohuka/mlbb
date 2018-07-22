using UnityEngine;
using System.Collections;

public class MessageBlock{

    private const int ALIGN = 1024;

    public MessageBlock(int size)
    {
        size = size < 0 ? ALIGN : size;
        size = ((size + ALIGN - 1) & (~(ALIGN - 1)));
        
        MemoryBlock_ = new byte[size];

        RdPtr_ = 0;
        WrPtr_ = 0;

    }

    public int Size
    {
        get
        {
            //lock (MemoryBlock_)
            //{
                return MemoryBlock_.Length;
            //}
        }
    }
    
    public int Space
    {
        get
        {
            return MemoryBlock_.Length - WrPtr_;
        }
    }

    public int Length
    {
        get
        {
            return WrPtr_ - RdPtr_;
            
        }
    }

    public void Write(ref byte[] b, int offset, int length)
    {
        if (length > Space)
        {///BUUFER 越界
            return;
        }
      
        System.Array.Copy(b, offset, MemoryBlock_, WrPtr_, length);
        WrPtr_ += length;
       
    }

    public int Read(ref byte[] r, int offset, int length)
    {
        if( 0 == Length)
            return 0;
        if(Length > length)
            return 0;
        length = Length;
        
        System.Array.Copy(MemoryBlock_, RdPtr_, r, offset, length);
        RdPtr_ += length;
        return length;
      
    }

    public void Crunch()
    {
        
        if(0 == RdPtr_)
            return ;
        int i = 0;
        int l = WrPtr_ - RdPtr_;
        while (i < l)
        {
            MemoryBlock_[i] = MemoryBlock_[RdPtr_ + i++];
        }

        RdPtr_ = 0;
        WrPtr_ = l;
    }

    public byte[] Memory
    {
        get
        {
            return MemoryBlock_;
        }
    }

    public void RdPtr(int offset)
    {
         RdPtr_ += offset;
    }

    public int RdPtr()
    {
        return RdPtr_;
    }

    public void WrPtr(int offset)
    {
        
            WrPtr_ += offset;
       
    }

    public int WrPtr()
    {
        return WrPtr_;
    }
    public void Reset()
    {
        lock (MemoryBlock_)
        {
            RdPtr_ = WrPtr_ = 0;
        }
    }
    
    int RdPtr_;
    int WrPtr_;
    byte[] MemoryBlock_;
}
