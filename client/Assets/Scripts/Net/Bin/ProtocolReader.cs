using System;
using System.Text;

namespace bin
{
    /** This class can read basic types by using a bin.IReader object. */
    public static class ProtocolReader
    {
        public static bool readType(bin.IReader r, out byte[] v, uint len)
        {
            v = new byte[len];
            byte[] data; int startId;
            if (!r.read(len, out data, out startId))
                return false;
            Array.Copy(data, startId, v, 0, len);
            return true;
        }
    	public static bool readType(bin.IReader r, out long v)
    	{
            v = 0;
            byte[] data; int startId;
            if (!r.read(8, out data, out startId))
                return false;
            v = BitConverter.ToInt64(data, startId);
            return true;
    	}
    	public static bool readType(bin.IReader r, out ulong v)
    	{
            v = 0;
            byte[] data; int startId;
            if (!r.read(8, out data, out startId))
                return false;
            v = BitConverter.ToUInt64(data, startId);
            return true;
    	}
    	public static bool readType(bin.IReader r, out double v)
    	{
            v = 0.0;
            byte[] data; int startId;
            if (!r.read(8, out data, out startId))
                return false;
            v = BitConverter.ToDouble(data, startId);
            return true;
    	}
    	public static bool readType(bin.IReader r, out float v)
    	{
            v = 0.0f;
            byte[] data; int startId;
            if (!r.read(4, out data, out startId))
                return false;
            v = BitConverter.ToSingle(data, startId);
            return true;
    	}
    	public static bool readType(bin.IReader r, out int v)
    	{
            v = 0;
            byte[] data; int startId;
            if (!r.read(4, out data, out startId))
                return false;
            v = BitConverter.ToInt32(data, startId);
            return true;
    	}
    	public static bool readType(bin.IReader r, out uint v)
    	{
            v = 0;
            byte[] data; int startId;
            if (!r.read(4, out data, out startId))
                return false;
            v = BitConverter.ToUInt32(data, startId);
            return true;
    	}
    	public static bool readType(bin.IReader r, out short v)
    	{
            v = 0;
            byte[] data; int startId;
            if (!r.read(2, out data, out startId))
                return false;
            v = BitConverter.ToInt16(data, startId);
            return true;
    	}
    	public static bool readType(bin.IReader r, out ushort v)
    	{
            v = 0;
            byte[] data; int startId;
            if (!r.read(2, out data, out startId))
                return false;
            v = BitConverter.ToUInt16(data, startId);
            return true;
    	}
    	public static bool readType(bin.IReader r, out sbyte v)
    	{
            v = 0;
            byte[] data; int startId;
            if (!r.read(1, out data, out startId))
                return false;
            v = (sbyte)data[startId];
            return true;
    	}
    	public static bool readType(bin.IReader r, out byte v)
    	{
            v = 0;
            byte[] data; int startId;
            if (!r.read(1, out data, out startId))
                return false;
            v = data[startId];
            return true;
    	}
    	public static bool readType(bin.IReader r, out bool v)
    	{
            v = false;
            byte[] data; int startId;
            if (!r.read(1, out data, out startId))
                return false;
            v = (data[startId] == 0)?false:true;
            return true;
    	}
    	public static bool readType(bin.IReader r, out string v, uint maxlen)
    	{
            v = "";
            uint len;
            if (!readDynSize(r, out len))
                return false;
            if (len > maxlen)
                return false;
            if (len == 0)
                return true;
            byte[] data; int startId;
            if(!r.read(len, out data, out startId))
                return false;
            v = Encoding.UTF8.GetString(data, startId, (int)len);
            return true;
    	}
        public static bool readDynSize(bin.IReader r, out uint s)
        {
            s = 0;
            byte b;
            if (!readType(r, out b))
                return false;
            uint n = (uint)((b & 0XC0) >> 6);
            s = (uint)(b & 0X3F);
            for (int i = 0; i < n; i++)
            {
                if (!readType(r, out b))
                    return false;
                s = (s << 8) | b;
            }
            return true;
        }
    }
}
