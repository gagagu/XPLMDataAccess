using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNetDataRefConnector;
using System.Globalization;

namespace TestApp
{
    public partial class Mainform : Form
    {
        // define connector
        private DotNetDataRefConnector.XPLMDataAccess da = new DotNetDataRefConnector.XPLMDataAccess();

        public Mainform()
        {
            InitializeComponent();
            // open connection to shared memory
            da.Open();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            da.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region "functions by DataRef name"
        /// <summary>
        /// Find Data Ref by String Name, result ist handle as uint32 value
        /// This value can used as handle for further commands to this dataref
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btXPLMFindDataRef_Click(object sender, EventArgs e)
        {
            UInt32 res = da.XPLMFindDataRef(tbDataRef.Text);
            this.textBox1.Text = res.ToString();
        }

        /// <summary>
        /// aks if dataref is writeable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btXPLMCanWriteDataRef_Click(object sender, EventArgs e)
        {
            int res = da.XPLMCanWriteDataRef(tbDataRef.Text);
            this.textBox1.Text = res.ToString();
        }

        private void btXPLMGetDataRefTypes_Click(object sender, EventArgs e)
        {
            int res = da.XPLMGetDataRefTypes(tbDataRef.Text);
            this.textBox1.Text = res.ToString();
        }

        private void btXPLMGetDatai_Click(object sender, EventArgs e)
        {
            int res = da.XPLMGetDatai(tbDataRef.Text);
            this.textBox1.Text = res.ToString();
        }

        private void btXPLMSetDatai_Click(object sender, EventArgs e)
        {
            da.XPLMSetDatai(tbDataRef.Text,Convert.ToInt16(tbValue.Text));
            this.textBox1.Text = "send";
        }

        private void btXPLMGetDataf_Click(object sender, EventArgs e)
        {
            float res = da.XPLMGetDataf(tbDataRef.Text);
            this.textBox1.Text = res.ToString();
        }

        private void btXPLMSetDataf_Click(object sender, EventArgs e)
        {
            da.XPLMSetDataf(tbDataRef.Text, Convert.ToSingle(tbValue.Text));
            this.textBox1.Text = "send";
        }

        private void btXPLMGetDatavf_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            float []res = da.XPLMGetDatavf(tbDataRef.Text);
            if (res != null)
            {
                foreach (float x in res)
                {
                    this.textBox1.Text += x.ToString(CultureInfo.CreateSpecificCulture("de-DE")) + "\r\n";
                }
            }
        }

        private void btXPLMGetDatavi_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            Int16[] res = da.XPLMGetDatavi(tbDataRef.Text);
            if (res != null)
            {
                foreach (Int16 x in res)
                {
                    this.textBox1.Text += x.ToString() + "\r\n";
                }
            }
        }


        private void btXPLMGetDatab_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            byte[] res = da.XPLMGetDatab(tbDataRef.Text);
            if (res != null)
            {
                //foreach (byte x in res)
                //{
                //    this.textBox1.Text += x.ToString() + "\r\n";
                //}
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                this.textBox1.Text += enc.GetString(res);
            }
        }

        private void btXPLMSetDatavf_Click(object sender, EventArgs e)
        {
            // only 8 for test
            Single[] array= new Single[8];
            array[0] = Convert.ToSingle(tbValue.Text);

            da.XPLMSetDatavf(tbDataRef.Text, array);
            this.textBox1.Text = tbValue.Text + " is send";
        }

        private void btXPLMSetDatavi_Click(object sender, EventArgs e)
        {
            // only 8 for test
            Int16[] array = new Int16[8];
            array[0] = Convert.ToInt16(tbValue.Text);

            da.XPLMSetDatavi(tbDataRef.Text, array);
            this.textBox1.Text = tbValue.Text + " is send";
        }

        private void btXPLMSetDatab_Click(object sender, EventArgs e)
        {
            // only 8 for test
            byte[] array = new byte[8];
            array[0] = Convert.ToByte(tbValue.Text);

            da.XPLMSetDatab(tbDataRef.Text, array);
            this.textBox1.Text = tbValue.Text + " is send";
        }

        private void btXPLMGetDatad_Click(object sender, EventArgs e)
        {
            double res = da.XPLMGetDatad(tbDataRef.Text);
            this.textBox1.Text = res.ToString();
        }

        private void btXPLMSetDatad_Click(object sender, EventArgs e)
        {
            da.XPLMSetDatad(tbDataRef.Text, Convert.ToDouble(tbValue.Text));
            this.textBox1.Text = "send";
        }

        #endregion


        #region "functions by DataRef handle"
        
        private void btXPLMGetDataiHandle_Click(object sender, EventArgs e)
        {
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            int res = da.XPLMGetDatai(RefHandle);
            this.textBox1.Text = res.ToString();
        }


        private void btXPLMCanWriteDataRefHandle_Click(object sender, EventArgs e)
        {
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            int res = da.XPLMCanWriteDataRef(RefHandle);
            this.textBox1.Text = res.ToString();
        }

        private void XPLMGetDataRefTypesHandle_Click(object sender, EventArgs e)
        {
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            int res = da.XPLMGetDataRefTypes(RefHandle);
            this.textBox1.Text = res.ToString();
        }

        private void XPLMSetDataiHandle_Click(object sender, EventArgs e)
        {
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            da.XPLMSetDatai(RefHandle, Convert.ToInt16(tbValueHandle.Text));
            this.textBox1.Text = "send";
        }

        private void XPLMGetDatafHandle_Click(object sender, EventArgs e)
        {
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            float res = da.XPLMGetDataf(RefHandle);
            this.textBox1.Text = res.ToString();
        }
        

        private void XPLMSetDatafHandle_Click(object sender, EventArgs e)
        {
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            da.XPLMSetDataf(RefHandle, Convert.ToSingle(tbValueHandle.Text));
            this.textBox1.Text = "send";
        }
        

        private void btXPLMGetDatavfHandle_Click(object sender, EventArgs e)
        {
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            this.textBox1.Text = "";
            float[] res = da.XPLMGetDatavf(RefHandle);
            if (res != null)
            {
                foreach (float x in res)
                {
                    this.textBox1.Text += x.ToString(CultureInfo.CreateSpecificCulture("de-DE")) + "\r\n";
                }
            }
        }

        private void btXPLMGetDataviHandle_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            Int16[] res = da.XPLMGetDatavi(RefHandle);
            if (res != null)
            {
                foreach (Int16 x in res)
                {
                    this.textBox1.Text += x.ToString() + "\r\n";
                }
            }
        }
        
        private void btXPLMGetDatabHandle_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            byte[] res = da.XPLMGetDatab(RefHandle);
            if (res != null)
            {
                //foreach (byte x in res)
                //{
                //    this.textBox1.Text += x.ToString() + "\r\n";
                //}

                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                this.textBox1.Text += enc.GetString(res);
            }

        }
        

        private void btXPLMSetDatavfHandle_Click(object sender, EventArgs e)
        {
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            // only 8 for test
            Single[] array = new Single[8];
            array[0] = Convert.ToSingle(tbValue.Text);

            da.XPLMSetDatavf(RefHandle, array);
            this.textBox1.Text = tbValue.Text + " is send";
        }

        private void btXPLMSetDataviHandle_Click(object sender, EventArgs e)
        {
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            // only 8 for test
            Int16[] array = new Int16[8];
            array[0] = Convert.ToInt16(tbValue.Text);

            da.XPLMSetDatavi(RefHandle, array);
            this.textBox1.Text = tbValue.Text + " is send";
        }

        private void btXPLMSetDatabHandle_Click(object sender, EventArgs e)
        {
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            // only 8 for test
            byte[] array = new byte[8];
            array[0] = Convert.ToByte(tbValue.Text);

            da.XPLMSetDatab(RefHandle, array);
            this.textBox1.Text = tbValue.Text + " is send";
        }

        private void XPLMGetDatadHandle_Click(object sender, EventArgs e)
        {
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            double res = da.XPLMGetDatad(RefHandle);
            this.textBox1.Text = res.ToString();
        }

        private void XPLMSetDatadHandle_Click(object sender, EventArgs e)
        {
            uint RefHandle = Convert.ToUInt32(tbHandle.Text);
            da.XPLMSetDatad(RefHandle, Convert.ToDouble(tbValueHandle.Text));
            this.textBox1.Text = "send";
        }

        #endregion

    }
}
