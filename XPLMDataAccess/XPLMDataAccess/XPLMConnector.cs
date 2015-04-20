using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace DotNetDataRefConnector
{
    /// <summary>
    /// XPLMConnector
    /// created by A.Eckers
    /// 
    /// This class handles the read/write communication to DotNetDataRefConnector through shared memory
    /// </summary>
    internal class XPLMConnector
    {
        // shared memory map names
        private const string COMMAND_MAP_NAME = "DNDRC_COMMAND";
        private const string DATA_MAP_NAME = "DNDRC_DATA";

        // memory change event names
        private const string COMMAND_EVENT_NAME = "DNDRC_COMMAND_EVENT";
        private const string DATA_EVENT_NAME = "DNDRC_DATA_EVENT";

        // memory lock names
        private const string COMMAND_LOCK_NAME = "COMMAND_LOCK";
        private const string DATA_LOCK_NAME = "DATA_LOCK";

        // structure size in bytes
        private int command_struct_size = 0;
        private int data_struct_size = 0;

        // wait handles
        private readonly EventWaitHandle command_eventwaithandle;
        private readonly EventWaitHandle data_eventwaithandle;

        // handles for command exchange
        private MemoryMappedFile mmf_command;
        private MemoryMappedViewAccessor mmfa_command;
        private Mutex lock_command;
        private bool command_locked;

        // handles for data exchange
        private MemoryMappedFile mmf_data;
        private MemoryMappedViewAccessor mmfa_data;
        private Mutex lock_data;
        private bool data_locked;

        // flags
        private bool initialized = false;
        private bool opened = false;

        // timeout for waiting of reading data in seconds
        private int timeout = 5;

        /// <summary>
        /// Konstructor
        /// Initialization of handles
        /// Exception on error
        /// </summary>
        public XPLMConnector()
        {
            try{
                // get size of structures
                command_struct_size = Marshal.SizeOf(typeof(XPLMCommandStruct.XPLMCommand));
                data_struct_size = Marshal.SizeOf(typeof(XPLMDataStruct.XPLMData));

                // create wait handles
                command_eventwaithandle = new EventWaitHandle(false, EventResetMode.ManualReset, COMMAND_EVENT_NAME);
                data_eventwaithandle = new EventWaitHandle(false, EventResetMode.ManualReset, DATA_EVENT_NAME);

                // set flag
                initialized = true;
            }
            catch (Exception ex)
            {
                // everything goes wrong
                throw new Exception(ex.Message);
            }
        } // XPLMConnector

        /// <summary>
        /// set reat timeout in seconds
        /// </summary>
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        } // Timeout

        /// <summary>
        /// open connection to DotNetDataRefConnector plugin over shared memory
        /// </summary>
        /// <returns>true when ok; exception on error</returns>
        public bool Open()
        {
            try
            {
                // Create connection to shared memory for command exchange
                mmf_command = MemoryMappedFile.CreateOrOpen(COMMAND_MAP_NAME, command_struct_size, MemoryMappedFileAccess.ReadWrite);
                mmfa_command = mmf_command.CreateViewAccessor(0, command_struct_size, MemoryMappedFileAccess.ReadWrite);
                lock_command = new Mutex(true, COMMAND_LOCK_NAME, out command_locked);

                // create connectio to shared memory for data exchange
                mmf_data = MemoryMappedFile.CreateOrOpen(DATA_MAP_NAME, data_struct_size, MemoryMappedFileAccess.ReadWrite);
                mmfa_data = mmf_data.CreateViewAccessor(0, data_struct_size, MemoryMappedFileAccess.ReadWrite);
                lock_data = new Mutex(true, DATA_LOCK_NAME, out data_locked);

                // set flag
                opened = true;

                return true;
            }
            catch (Exception ex)
            {
                // something goes wrong
                throw new Exception(ex.Message);
            }
        } // open


        /// <summary>
        /// writes a command to shared memory
        /// XPLMDataRef or XPLMDataRefHandle can be used.
        /// If both will given then only XPLMDataRefHandle is used.
        /// </summary>
        /// <param name="cmd">command</param>
        /// <param name="XPLMDataRef">data ref name string</param>
        /// <param name="XPLMDataRefHandle">data ref handle</param>
        /// <param name="data">data for command i.g. write values</param>
        /// <returns>struct with return data from command</returns>
        public XPLMDataStruct.XPLMData Write(XPLMCommandStruct.XPLMCommandList cmd, string XPLMDataRef,  uint XPLMDataRefHandle,  XPLMDataStruct.XPLMData data)
        {
            try{
                // connector should be initialized and open
                if(!initialized)
                    throw new Exception("XLMPConnetor is not initialized!");

                if (!opened)
                    throw new Exception("XLMPConnetor is not opened!");



                XPLMCommandStruct.XPLMCommand newCommand = new XPLMCommandStruct.XPLMCommand();
                // create command
                newCommand.Command = cmd;
                if (XPLMDataRefHandle > 0)
                {
                    newCommand.XPLMDataRefName = "";
                    newCommand.XPLMDataRefHandle = XPLMDataRefHandle;
                }
                else
                {
                    newCommand.XPLMDataRefName = XPLMDataRef;
                    newCommand.XPLMDataRefHandle = 0;
                }
                // convert struct command to byte array
                byte[] byteArray = ConvertStructToByteArray(newCommand);

                // data available?
                if (data.DataLength >0)
                {

                    // convert struct data to array byte
                    byte[] dataByteArray = ConvertStructToByteArray(data);

                    // write data to shared memory
                    lock_data.WaitOne();
                    mmfa_data.WriteArray(0, dataByteArray, 0, dataByteArray.Length);
                    lock_data.ReleaseMutex();
                }
                else {
                    data = new XPLMDataStruct.XPLMData();
                }

                // write command to shared memory
                lock_command.WaitOne();
                mmfa_command.WriteArray(0, byteArray, 0, byteArray.Length);
                lock_command.ReleaseMutex();

                // set event to indicate the plugin that a command is send
                command_eventwaithandle.Set();

                // no reply by set commands
                if (data.DataLength == 0)
                {
                    // wait for reply until timeout
                    int timeout_index = 0;
                    while (timeout_index < Timeout)
                    {
                        // ask if reply event is set from plugin
                        var isNewDataAvailable = data_eventwaithandle.WaitOne(1000);
                        // data available?
                        if (isNewDataAvailable)
                        {
                            // read data
                            data = this.ReadData();
                            // reset event
                            data_eventwaithandle.Reset();
                            // break while
                            break;
                        }
                        // raise timeout index
                        timeout_index++;

                    }
                }

                // return data
                return data;
            }
            catch
            {
                // on error null
                return new XPLMDataStruct.XPLMData();
            }
        } // Write



        /// <summary>
        /// reads the data from shared memory an copy it to struct
        /// </summary>
        /// <returns>struct with data</returns>
        private XPLMDataStruct.XPLMData ReadData()
        {
            try{
                // define buffer
                byte[] byteArray = new byte[Marshal.SizeOf(typeof(XPLMDataStruct.XPLMData))];
                // read daza from shared memory 
                mmfa_data.ReadArray<byte>(0, byteArray, 0, Marshal.SizeOf(typeof(XPLMDataStruct.XPLMData)));
                // get handle of buffer
                GCHandle handle = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
                // copy buffer to struct
                XPLMDataStruct.XPLMData data = (XPLMDataStruct.XPLMData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(XPLMDataStruct.XPLMData));
                // free handle
                handle.Free();
                // return data
                return data;
            }
            catch
            {
                // on error null
                return new XPLMDataStruct.XPLMData();
            }
        } // ReadData


        /// <summary>
        /// converts a data structure to byte[] array
        /// </summary>
        /// <param name="theStruct">structure to convert to byte array</param>
        /// <returns>byte[] array; on error null</returns>
        private static byte[] ConvertStructToByteArray(object theStruct)
        {
            // define array size
            byte[] datatemp = new byte[Marshal.SizeOf(theStruct)];
            try
            {   
                // alloc memory block
                var p = Marshal.AllocHGlobal(Marshal.SizeOf(theStruct));
                // copy struct to memory block
                Marshal.StructureToPtr(theStruct, p, false);
                // copy memory block to buffer
                Marshal.Copy(p, datatemp, 0, datatemp.Length);
                // release memory block
                Marshal.FreeHGlobal(p);
                // return data
                return datatemp;
            }catch
            {
                // on error null
                return null;
            }

        } // ConvertStructToByteArray


        /// <summary>
        /// Close and dispose all
        /// </summary>
        /// <returns>true then ok; exception on error</returns>
        public bool Close()
        {
            try
            {
                // set flag
                opened = false;
                // dispose and close
                mmfa_command.Dispose();
                mmf_command.Dispose();
                lock_command.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        } // close

    } // class
}
