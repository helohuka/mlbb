using System;
using System.Collections.Generic;
using UnityEngine;
namespace bin
{
    public class MemWriter : bin.IWriter
    {
        public MemWriter()
        {
            buffer_ = new List<byte>();
        }
        public byte[] buffer { get { return buffer_.ToArray(); } }
    	public void write(byte[] data)
        {
            for (int i = 0; i < data.Length; ++i )
                buffer_.Add(data[i]);
        }
        private List<byte> buffer_;
    }
}