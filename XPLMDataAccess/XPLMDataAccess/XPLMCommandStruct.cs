using System.Runtime.InteropServices;

namespace DotNetDataRefConnector
{
    /// <summary>
    /// XPLMCommandStruct
    /// created by A.Eckers
    ///
    /// Defines the struct for command exchange between DotNetDataRefConnector plugin an DotNetDataRefConnector .net dll
    /// </summary>
    public class XPLMCommandStruct
    {
        /// <summary>
        /// Typelist; defines the commandlist used for XPLMDataAccess
        /// only this commands are implemeted
        /// </summary>
        public enum XPLMCommandList : byte
        {
            CMD_XPLMCanWriteDataRef = 0x00,
            CMD_XPLMGetDataRefTypes = 0x01,
            CMD_XPLMGetDatai = 0x02,
            CMD_XPLMSetDatai = 0x03,
            CMD_XPLMGetDataf = 0x04,
            CMD_XPLMSetDataf = 0x05,
            CMD_XPLMGetDatad = 0x06,
            CMD_XPLMSetDatad = 0x07,
            CMD_XPLMGetDatavi = 0x08,
            CMD_XPLMSetDatavi = 0x09,
            CMD_XPLMGetDatavf = 0x0a,
            CMD_XPLMSetDatavf = 0x0b,
            CMD_XPLMGetDatab = 0x0c,
            CMD_XPLMSetDatab = 0x0d,
            CMD_XPLMFindDataRef = 0x0e
        }

        /// <summary>
        /// Layout for command exchange
        /// only read
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct XPLMCommand
        {
            // command name
            public XPLMCommandList Command;

            // DataRef name
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string XPLMDataRefName;

            // Handle of DataRef in uint32
            public uint XPLMDataRefHandle;
        }
    }
}
