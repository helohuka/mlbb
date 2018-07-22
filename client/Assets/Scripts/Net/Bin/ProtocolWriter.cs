using System;
using System.Text;

namespace bin
{
    /** This class can write basic types by using a bin.IWriter object. */
    public static class ProtocolWriter
    {
        public static void writeType(bin.IWriter w, byte[] d) { w.write(d); }
    	public static void writeType(bin.IWriter w, long v)   { w.write(BitConverter.GetBytes(v)); }
    	public static void writeType(bin.IWriter w, ulong v)  { w.write(BitConverter.GetBytes(v)); }
    	public static void writeType(bin.IWriter w, double v) { w.write(BitConverter.GetBytes(v)); }
    	public static void writeType(bin.IWriter w, float v)  { w.write(BitConverter.GetBytes(v)); }
    	public static void writeType(bin.IWriter w, int v)    { w.write(BitConverter.GetBytes(v)); }
    	public static void writeType(bin.IWriter w, uint v)   { w.write(BitConverter.GetBytes(v)); }
    	public static void writeType(bin.IWriter w, short v)  { w.write(BitConverter.GetBytes(v)); }
    	public static void writeType(bin.IWriter w, ushort v) { w.write(BitConverter.GetBytes(v)); }
    	public static void writeType(bin.IWriter w, sbyte v)  { w.write(new byte[1]{(byte)v}); }
    	public static void writeType(bin.IWriter w, byte v)   { w.write(new byte[1]{v}); }
    	public static void writeType(bin.IWriter w, bool v)   { writeType(w, (byte)(v?1:0)); }
    	public static void writeType(bin.IWriter w, string v)
    	{
            if (v == null || v.Length == 0)
                writeDynSize(w, 0);
            else
            {
                byte[] str = Encoding.UTF8.GetBytes(v);
                uint len = (uint)str.Length;
                writeDynSize(w, len);
                if (len > 0)
                    w.write(str);
            }
    	}
        public static void writeDynSize(bin.IWriter w, uint s)
        {
            byte[] b = BitConverter.GetBytes(s);
            int n = 0;
            if (s <= 0X3F)
                n = 0;
            else if (s <= 0X3FFF)
                n = 1;
    		else if(s <= 0X3FFFFF)
    			n = 2;
    		else if(s <= 0X3FFFFFFF)
    			n = 3;
            b[n] |= (byte)(n << 6);
    		for(int i = n; i >= 0; i--)
    			writeType(w, b[i]);
        }
    }
}
