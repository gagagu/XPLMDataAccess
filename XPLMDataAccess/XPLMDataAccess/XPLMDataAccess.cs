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
    /// This class is the main interface for dll
    /// </summary>
    public class XPLMDataAccess
    {

        private XPLMConnector connector;

        #region "main"

        /// <summary>
        /// Konstructor
        /// Init handles
        /// </summary>
        public XPLMDataAccess()
        {
            try
            {
                connector = new XPLMConnector();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } // XPLMDataAccess

        /// <summary>
        /// Opens the connection to shared memory
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            try
            {
                connector.Open();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } // Open


        /// <summary>
        /// Timeout for waiting of reply data
        /// </summary>
        public int Timeout
        {
            get
            {
                return connector.Timeout;
            }
            set
            {
                connector.Timeout = value;
            }
        }

        /// <summary>
        /// close all connections an release handles
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            try
            {
                connector.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region "XPLMFindDataRef"
        /// <summary>
        /// This Routines return 1 if you can set data to given DataRef
        /// </summary>
        /// <param name="XPLMDataRef">DataRefName</param>
        /// <returns>0 = false; 1 = true</returns>
        public uint XPLMFindDataRef(string XPLMDataRefName)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return 0;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMFindDataRef, XPLMDataRefName, 0, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_handle))
            {

                UInt32 i = BitConverter.ToUInt32(data.Data, 0);
                return i;
            }

            return 0;
        }
        #endregion

        #region "XPLMCanWriteDataRef"
        /// <summary>
        /// This Routines return 1 if you can set data to given DataRef
        /// </summary>
        /// <param name="XPLMDataRef">DataRefName</param>
        /// <returns>0 = false; 1 = true</returns>
        public int XPLMCanWriteDataRef(string XPLMDataRefName)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return 0;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMCanWriteDataRef, XPLMDataRefName, 0, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength>0) && (data.DataType==XPLMDataStruct.XPLMDataTypeList.dtype_int))
            {

                Int16 i = BitConverter.ToInt16(data.Data, 0);
                return i;
            }

            return 0;
        }

        /// <summary>
        /// This Routines return 1 if you can set data to given DataRef
        /// </summary>
        /// <param name="XPLMDataRefHandle">DataRef Handle</param>
        /// <returns>0 = false; 1 = true</returns>
        public int XPLMCanWriteDataRef(uint XPLMDataRefHandle)
        {
            if (XPLMDataRefHandle == 0)
                return 0;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMCanWriteDataRef, "", XPLMDataRefHandle, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_int))
            {

                Int16 i = BitConverter.ToInt16(data.Data, 0);
                return i;
            }

            return 0;
        }
        #endregion

        #region "XPLMGetDataRefTypes"
        /// <summary>
        /// Returns the types of the data ref
        /// </summary>
        /// <param name="XPLMDataRef">DataRef Name</param>
        /// <returns>datetype id</returns>
        public int XPLMGetDataRefTypes(string XPLMDataRefName)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return 0;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDataRefTypes, XPLMDataRefName, 0, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_int))
            {

                Int16 i = BitConverter.ToInt16(data.Data, 0);
                return i;
            }

            return 0;
        }

        /// <summary>
        /// Returns the types of the data ref
        /// </summary>
        /// <param name="XPLMDataRefHandle">DataRef Handle</param>
        /// <returns>datetype id</returns>
        public int XPLMGetDataRefTypes(uint XPLMDataRefHandle)
        {
            if (XPLMDataRefHandle == 0)
                return 0;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDataRefTypes, "", XPLMDataRefHandle, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_int))
            {

                Int16 i = BitConverter.ToInt16(data.Data, 0);
                return i;
            }

            return 0;
        }
        #endregion

        #region "XPLMGetDatai"
        /// <summary>
        /// This Routines return 1 if you can set data to given DataRef
        /// </summary>
        /// <param name="XPLMDataRef">DataRefName</param>
        /// <returns>0 = false; 1 = true</returns>
        public int XPLMGetDatai(string XPLMDataRefName)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return 0;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDatai, XPLMDataRefName, 0, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_int))
            {

                Int16 i = BitConverter.ToInt16(data.Data, 0);
                return i;
            }

            return 0;
        }

        /// <summary>
        /// This Routines return 1 if you can set data to given DataRef
        /// </summary>
        /// <param name="XPLMDataRefHandle">DataRef Handle</param>
        /// <returns>0 = false; 1 = true</returns>
        public int XPLMGetDatai(uint XPLMDataRefHandle)
        {
            if (XPLMDataRefHandle==0)
                return 0;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDatai, "", XPLMDataRefHandle, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_int))
            {

                Int16 i = BitConverter.ToInt16(data.Data, 0);
                return i;
            }

            return 0;
        }
        #endregion

        #region "XPLMGetDataf"
        /// <summary>
        /// This Routines return 1 if you can set data to given DataRef
        /// </summary>
        /// <param name="XPLMDataRef">DataRefName</param>
        /// <returns>0 = false; 1 = true</returns>
        public float XPLMGetDataf(string XPLMDataRefName)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return 0;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDataf, XPLMDataRefName, 0, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_float))
            {

                float i = BitConverter.ToSingle(data.Data, 0);
                return i;
            }

            return 0;
        }

        /// <summary>
        /// This Routines return 1 if you can set data to given DataRef
        /// </summary>
        /// <param name="XPLMDataRefHandle">DataRef Handle</param>
        /// <returns>0 = false; 1 = true</returns>
        public float XPLMGetDataf(uint XPLMDataRefHandle)
        {
            if (XPLMDataRefHandle == 0)
                return 0;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDataf, "", XPLMDataRefHandle, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_float))
            {

                float i = BitConverter.ToSingle(data.Data, 0);
                return i;
            }

            return 0;
        }
        #endregion

        #region "XPLMSetDatai"
        /// <summary>
        /// Write a new int value to dataref
        /// </summary>
        /// <param name="XPLMDataRef">DataRef Name</param>
        /// <param name="value">value to write</param>
        public void XPLMSetDatai(string XPLMDataRefName, Int16 value)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return;

            XPLMDataStruct.XPLMData data = new XPLMDataStruct.XPLMData();
            data.DataLength = 2;
            data.DataType = XPLMDataStruct.XPLMDataTypeList.dtype_int;
            data.Data = new byte[65565];
            
            byte[] intval = BitConverter.GetBytes(value);
     
            Buffer.BlockCopy(intval, 0, data.Data, 0, 2);
            connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMSetDatai, XPLMDataRefName, 0, data);
        }

        /// <summary>
        /// Write a new int value to dataref
        /// </summary>
        /// <param name="XPLMDataRefHandle">DataRef Handle</param>
        /// <param name="value">value to write</param>
        public void XPLMSetDatai(uint XPLMDataRefHandle, Int16 value)
        {
            if (XPLMDataRefHandle == 0)
                return;

            XPLMDataStruct.XPLMData data = new XPLMDataStruct.XPLMData();
            data.DataLength = 2;
            data.DataType = XPLMDataStruct.XPLMDataTypeList.dtype_int;
            data.Data = new byte[65565];

            byte[] intval = BitConverter.GetBytes(value);

            Buffer.BlockCopy(intval, 0, data.Data, 0, 2);
            connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMSetDatai, "", XPLMDataRefHandle, data);
        }
        #endregion

        #region "XPLMSetDataf"
        public void XPLMSetDataf(string XPLMDataRefName, Single value)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return;

            XPLMDataStruct.XPLMData data = new XPLMDataStruct.XPLMData();
            data.DataLength = 4;
            data.DataType = XPLMDataStruct.XPLMDataTypeList.dtype_float;
            data.Data = new byte[65565];

            byte[] floatval = BitConverter.GetBytes(value);
            Buffer.BlockCopy(floatval, 0, data.Data, 0, 4);
            connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMSetDataf, XPLMDataRefName, 0, data);
        }

        public void XPLMSetDataf(uint XPLMDataRefHandle, Single value)
        {
            if (XPLMDataRefHandle == 0)
                return;

            XPLMDataStruct.XPLMData data = new XPLMDataStruct.XPLMData();
            data.DataLength = 4;
            data.DataType = XPLMDataStruct.XPLMDataTypeList.dtype_float;
            data.Data = new byte[65565];

            byte[] floatval = BitConverter.GetBytes(value);
            Buffer.BlockCopy(floatval, 0, data.Data, 0, 4);
            connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMSetDataf, "", XPLMDataRefHandle, data);
        }
        #endregion

        #region "XPLMGetDatavf"
        public float[] XPLMGetDatavf(string XPLMDataRefName)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return null;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDatavf, XPLMDataRefName, 0, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_float_array))
            {
                float []i= new float[data.DataLength/4];
                for(int x=0; x<data.DataLength/4;x++)
                {
                    i[x] = BitConverter.ToSingle(data.Data, x*4);
                }
                return i;
            }

            return null;
        }

        public float[] XPLMGetDatavf(uint XPLMDataRefHandle)
        {
            if (XPLMDataRefHandle == 0)
                return null;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDatavf, "", XPLMDataRefHandle, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_float_array))
            {
                float[] i = new float[data.DataLength / 4];
                for (int x = 0; x < data.DataLength / 4; x++)
                {
                    i[x] = BitConverter.ToSingle(data.Data, x * 4);
                }
                return i;
            }

            return null;
        }
        #endregion

        #region "XPLMGetDatavi"
        public Int16[] XPLMGetDatavi(string XPLMDataRefName)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return null;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDatavi, XPLMDataRefName, 0, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_int_array))
            {
                Int16[] i = new Int16[data.DataLength / 2];
                for (int x = 0; x < data.DataLength / 2; x++)
                {
                    i[x] = BitConverter.ToInt16(data.Data, x * 2);
                }
                return i;
            }

            return null;
        }

        public Int16[] XPLMGetDatavi(uint XPLMDataRefHandle)
        {
            if (XPLMDataRefHandle == 0)
                return null;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDatavi, "", XPLMDataRefHandle, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_int_array))
            {
                Int16[] i = new Int16[data.DataLength / 2];
                for (int x = 0; x < data.DataLength / 2; x++)
                {
                    i[x] = BitConverter.ToInt16(data.Data, x *2);
                }
                return i;
            }

            return null;
        }
        #endregion

        #region "XPLMGetDatab"
        public byte[] XPLMGetDatab(string XPLMDataRefName)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return null;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDatab, XPLMDataRefName, 0, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_byte_array))
            {
                return data.Data.Take(data.DataLength).ToArray();
            }

            return null;
        }

        public byte[] XPLMGetDatab(uint XPLMDataRefHandle)
        {
            if (XPLMDataRefHandle == 0)
                return null;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDatab, "", XPLMDataRefHandle, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_byte_array))
            {
                return data.Data.Take(data.DataLength).ToArray();
            }

            return null;
        }
        #endregion

        #region "XPLMSetDatavf"
        public void XPLMSetDatavf(string XPLMDataRefName, Single[] value)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return;

            XPLMDataStruct.XPLMData data = new XPLMDataStruct.XPLMData();
            data.DataLength = 4 * value.Count(); 
            data.DataType = XPLMDataStruct.XPLMDataTypeList.dtype_float_array;
            data.Data = new byte[65565];

            for (int x = 0; x < value.Count(); x++)
            {
                byte[] floatval = BitConverter.GetBytes(value[x]);
                Buffer.BlockCopy(floatval, 0, data.Data, x*4, 4);
            }
            connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMSetDatavf, XPLMDataRefName, 0, data);
        }

        public void XPLMSetDatavf(uint XPLMDataRefHandle, Single[] value)
        {
            if (XPLMDataRefHandle == 0)
                return;

            XPLMDataStruct.XPLMData data = new XPLMDataStruct.XPLMData();
            data.DataLength = 4 * value.Count();
            data.DataType = XPLMDataStruct.XPLMDataTypeList.dtype_float_array;
            data.Data = new byte[65565];

            for (int x = 0; x < value.Count(); x++)
            {
                byte[] floatval = BitConverter.GetBytes(value[x]);
                Buffer.BlockCopy(floatval, 0, data.Data, x * 4, 4);
            }
            connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMSetDatavf, "", XPLMDataRefHandle, data);
        }
        #endregion

        #region "XPLMSetDatavi"
        public void XPLMSetDatavi(string XPLMDataRefName, Int16[] value)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return;

            XPLMDataStruct.XPLMData data = new XPLMDataStruct.XPLMData();
            data.DataLength = 2 * value.Count();
            data.DataType = XPLMDataStruct.XPLMDataTypeList.dtype_int_array;
            data.Data = new byte[65565];

            for (int x = 0; x < value.Count(); x++)
            {
                byte[] intval = BitConverter.GetBytes(value[x]);
                Buffer.BlockCopy(intval, 0, data.Data, x * 2, 2);
            }
            connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMSetDatavi, XPLMDataRefName, 0, data);
        }

        public void XPLMSetDatavi(uint XPLMDataRefHandle, Int16[] value)
        {
            if (XPLMDataRefHandle == 0)
                return;

            XPLMDataStruct.XPLMData data = new XPLMDataStruct.XPLMData();
            data.DataLength = 2 * value.Count();
            data.DataType = XPLMDataStruct.XPLMDataTypeList.dtype_int_array;
            data.Data = new byte[65565];

            for (int x = 0; x < value.Count(); x++)
            {
                byte[] intval = BitConverter.GetBytes(value[x]);
                Buffer.BlockCopy(intval, 0, data.Data, x * 2, 2);
            }
            connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMSetDatavi, "", XPLMDataRefHandle, data);
        }
        #endregion

        #region "XPLMSetDatab"
        public void XPLMSetDatab(string XPLMDataRefName, byte[] value)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return;

            XPLMDataStruct.XPLMData data = new XPLMDataStruct.XPLMData();
            data.DataLength = value.Count();
            data.DataType = XPLMDataStruct.XPLMDataTypeList.dtype_byte_array;
            data.Data = new byte[65565];

            Buffer.BlockCopy(value, 0, data.Data, 0, value.Count());

            connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMSetDatavi, XPLMDataRefName, 0, data);
        }

        public void XPLMSetDatab(uint XPLMDataRefHandle, byte[] value)
        {
            if (XPLMDataRefHandle == 0)
                return;

            XPLMDataStruct.XPLMData data = new XPLMDataStruct.XPLMData();
            data.DataLength = value.Count();
            data.DataType = XPLMDataStruct.XPLMDataTypeList.dtype_byte_array;
            data.Data = new byte[65565];

            Buffer.BlockCopy(value, 0, data.Data, 0, value.Count());

            connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMSetDatavi, "", XPLMDataRefHandle, data);
        }
        #endregion

        #region "XPLMSetDatad"
        public void XPLMSetDatad(string XPLMDataRefName, double value)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return;

            XPLMDataStruct.XPLMData data = new XPLMDataStruct.XPLMData();
            data.DataLength = 8;
            data.DataType = XPLMDataStruct.XPLMDataTypeList.dtype_double;
            data.Data = new byte[65565];

            byte[] doubleval = BitConverter.GetBytes(value);
            Buffer.BlockCopy(doubleval, 0, data.Data, 0, 8);
            connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMSetDatad, XPLMDataRefName, 0, data);
        }

        public void XPLMSetDatad(uint XPLMDataRefHandle, double value)
        {
            if (XPLMDataRefHandle == 0)
                return;

            XPLMDataStruct.XPLMData data = new XPLMDataStruct.XPLMData();
            data.DataLength = 8;
            data.DataType = XPLMDataStruct.XPLMDataTypeList.dtype_double;
            data.Data = new byte[65565];

            byte[] doubleval = BitConverter.GetBytes(value);
            Buffer.BlockCopy(doubleval, 0, data.Data, 0, 8);
            connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMSetDatad, "", XPLMDataRefHandle, data);
        }
        #endregion

        #region "XPLMGetDatad"

        public double XPLMGetDatad(string XPLMDataRefName)
        {
            if (string.IsNullOrEmpty(XPLMDataRefName))
                return 0;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDatad, XPLMDataRefName, 0, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_float))
            {

                double i = BitConverter.ToDouble(data.Data, 0);
                return i;
            }

            return 0;
        }


        public double XPLMGetDatad(uint XPLMDataRefHandle)
        {
            if (XPLMDataRefHandle == 0)
                return 0;

            XPLMDataStruct.XPLMData data = connector.Write(XPLMCommandStruct.XPLMCommandList.CMD_XPLMGetDataf, "", XPLMDataRefHandle, new XPLMDataStruct.XPLMData());
            if ((data.Data != null) && (data.DataLength > 0) && (data.DataType == XPLMDataStruct.XPLMDataTypeList.dtype_float))
            {

                double i = BitConverter.ToDouble(data.Data, 0);
                return i;
            }

            return 0;
        }
        #endregion
    } // class
}
