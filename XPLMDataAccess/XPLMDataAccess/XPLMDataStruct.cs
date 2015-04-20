using System.Runtime.InteropServices;

namespace DotNetDataRefConnector
{

    /// <summary>
    /// XPLMDataStruct
    /// created by A.Eckers
    /// 
    /// Defines the struct for data exchange between DotNetDataRefConnector plugin an DotNetDataRefConnector .net dll
    /// </summary>
    public class XPLMDataStruct
    {
        /// <summary>
        /// Typelist; defines the datatype in data buffer
        /// </summary>
        public enum XPLMDataTypeList : byte
        {
            dtype_int = 0x00,
            dtype_float = 0x01,
            dtype_double = 0x02,
            dtype_handle = 0x03,
            dtype_float_array = 0x04,
            dtype_int_array = 0x05,
            dtype_byte_array = 0x06
        }


        /// <summary>
        /// Layout for databuffer for exchange data
        /// This data are read/write
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct XPLMData
        {
            // count of bytes filled with data
            public int DataLength;

            // shows the datatype represeted with bytes in buffer
            public XPLMDataTypeList DataType;

            // databuffer max. 65565 bytes
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65536)]
            public byte[] Data;
        }
    }
}
