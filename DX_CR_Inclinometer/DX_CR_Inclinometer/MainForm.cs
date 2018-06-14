using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using XE.API.Sensor.RionInclinometer;

namespace DX_CR_Inclinometer
{
    public partial class MainForm : Form
    {
        #region 构造函数
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        #region 全局变量
        private System.IO.Ports.SerialPort _SP;//串口类
        private XE.API.Sensor.RionInclinometer.SynMethod _Syn = new XE.API.Sensor.RionInclinometer.SynMethod();
        private Mat _Mat = new Mat(640, 640, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
        private Action<float, float> _UpdateImageHandle; //更新界面事件
        private Action<byte[]> _SendDataHandle; //发送数据的委托
        #endregion

        #region 窗体加载
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadDefultData();//加载默认数据,为控件添加数据源
            _UpdateImageHandle = new Action<float, float>(DrawImage);
            _Syn.Decoding.AngleValueEvent += UpdateUI;//角度值事件
            _SendDataHandle = (data) => { _SP.Write(data, 0, data.Length); };//发送数据的委托设置
        }
        #endregion

        #region 方法
        /// <summary>
        /// 默认数据设置
        /// </summary>
        private void LoadDefultData()
        {
            //本机串口集合
            comboBox1.DataSource = SerialPort.GetPortNames();
            //波特率列表
            comboBox2.DataSource = new int[] { 4800, 9600, 19200, 38400, 115200 };
            comboBox2.Text = "9600";
            //数据位
            comboBox3.DataSource = new int[] { 8, 7, 6 };
            //停止位
            comboBox4.DataSource = Enum.GetValues(typeof(StopBits));
            comboBox4.Text = StopBits.One.ToString();
            //校验位
            comboBox5.DataSource = Enum.GetValues(typeof(Parity));
            comboBox5.Text = Parity.Even.ToString();

            listBox1.DataSource = Enum.GetNames(typeof(BaudRateTypeEnum));
            listBox2.DataSource = Enum.GetNames(typeof(ZeroTypeEnum));
            listBox3.DataSource = Enum.GetNames(typeof(AutoOutputTypeEnum));
        }
        /// <summary>
        /// 串口数据接收
        /// </summary>
        private void _SP_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] readBuffer = new byte[1024];
            int sumread = _SP.Read(readBuffer, 0, readBuffer.Length);

            if (sumread <= 0) { return; }

            byte[] recbyte = new byte[sumread]; //readbyte即为收到的字节数组;
            Array.Copy(readBuffer, 0, recbyte, 0, sumread);
            _Syn.Decoding.DeCode(recbyte);
        }
        /// <summary>
        /// 更新界面
        /// </summary>
        private void UpdateUI(float x,float y)
        {//从此处调整画面和数据同步关系

            if (checkBox1.Checked){x = -x;}
            if (checkBox2.Checked) { y = -y; }
            if (checkBox3.Checked)
            {
                this.BeginInvoke(_UpdateImageHandle, y, x);
            }
            else
            {
                this.BeginInvoke(_UpdateImageHandle, x, y);
            }
        }

        /// <summary>
        /// 对界面画图
        /// </summary>
        /// <param name="x">x轴角度</param>
        /// <param name="y">y轴角度</param>
        private void DrawImage(float x, float y)
        {
            GC.Collect();
            textBox1.Text = x.ToString();
            textBox2.Text = y.ToString();
            _Mat = new Mat(640, 640, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
            Emgu.CV.CvInvoke.Circle(_Mat, new Point(_Mat.Width / 2, _Mat.Height / 2), _Mat.Width / 2, new Emgu.CV.Structure.MCvScalar(255, 0, 255), 3);
            float linelength = Math.Abs((float)(_Mat.Height * Math.Sin(y * Math.PI / 180)));
            Emgu.CV.CvInvoke.Ellipse(
                    _Mat,
                    new Emgu.CV.Structure.RotatedRect(new PointF(_Mat.Width / 2, _Mat.Height / 2), new SizeF(linelength, _Mat.Width), x),
                    new Emgu.CV.Structure.MCvScalar(255, 255, 0),
                    2);
            float offsetx = (float)(x * Math.PI / 180);
            float offsety = (float)(y * Math.PI / 180);
            int pointX = (int)(_Mat.Width / 2 + (Math.Sin(offsetx) * linelength / 2) * ((y > 0) ? -1 : 1));
            int pointY = (int)(_Mat.Width / 2 + (Math.Cos(offsetx) * linelength / 2) * ((y > 0) ? 1 : -1));

            Emgu.CV.CvInvoke.Line(_Mat, new Point(_Mat.Width / 2, _Mat.Height / 2), new Point(pointX, pointY), new Emgu.CV.Structure.MCvScalar(255, 255, 128), 2);

            imageBox1.Image = _Mat;

        }
        #endregion

        #region  界面按钮事件
        private void button3_Click(object sender, EventArgs e)
        {//连接
            try
            {
                if (_SP != null) { _SP.Dispose(); }
                _SP = new SerialPort();
                _SP.PortName = comboBox1.Text;
                _SP.BaudRate = int.Parse(comboBox2.Text);
                _SP.DataBits = int.Parse(comboBox3.Text);
                _SP.StopBits = (StopBits)comboBox4.SelectedValue;
                _SP.ReadBufferSize = 1024;
                _SP.Parity = (Parity)comboBox5.SelectedValue; //Parity.Even;
                _SP.DataReceived += _SP_DataReceived;
                _SP.Open();
                panel2.BackColor = Color.Green;
            }
            catch (Exception ex)
            {
                panel2.BackColor = Color.Red;
                MessageBox.Show(ex.Message);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {//断开
            if (_SP == null) { return; }
            _SP.DataReceived -= _SP_DataReceived;
            _SP.Close();
            _SP.Dispose();
            panel2.BackColor = Color.Transparent;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _Syn.GetValue(_SendDataHandle);
        }
        private void button2_Click(object sender, EventArgs e)
        {//设置自动发送
            _Syn.SetAutoOutputType((AutoOutputTypeEnum)Enum.Parse(typeof(AutoOutputTypeEnum), listBox3.Text), _SendDataHandle);
        }
        private void button5_Click(object sender, EventArgs e)
        {//设置0点
            _Syn.SetZero((ZeroTypeEnum)(Enum.Parse(typeof(ZeroTypeEnum), listBox2.Text)), _SendDataHandle);
        }
        private void button6_Click(object sender, EventArgs e)
        {//波特率设置
            _Syn.SetBaudRate((BaudRateTypeEnum)(Enum.Parse(typeof(BaudRateTypeEnum), listBox1.Text)), _SendDataHandle);
        }
        private void button7_Click(object sender, EventArgs e)
        {//地址设置
            if (!Regex.IsMatch("F1", "^[0-9,A-F,a-f]{1,2}$", RegexOptions.Singleline)) { MessageBox.Show("输入数值不合法"); }
            byte b = Convert.ToByte(textBox3.Text.Trim(), 16);
            _Syn.SetAddress(b, _SendDataHandle);
        }
        #endregion

        #region 关闭窗口
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {//窗体关闭
            
        }

        #endregion

    }
}
