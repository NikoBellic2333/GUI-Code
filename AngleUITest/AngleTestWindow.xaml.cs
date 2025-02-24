using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfApp1.DashBoard_Detent;
using WpfApp1.DashBoard_Fric;
using WpfApp1.Mysql;
using WpfApp1.Serial;

namespace WpfApp1
{
    /// <summary>
    /// AngleTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AngleTestWindow : Window
    {

        public enum EShapeMode
        {
            defalut,
            point,
            three_detent,
            six_detent
 
        };
        public enum ETorqueFdb
        {
            NF,
            fric,
            ten_detent,
            detent_thrity
    

        };
        public enum EUImode
         {
            degree_30,
            degree_120,
            degree_330

         };

        // 定义一个组合结构体来表示枚举的组合
        public struct Combo
        {
            public EShapeMode ShapeMode;
            public ETorqueFdb TorqueFdb;
            public EUImode UiMode;

            public Combo(EShapeMode shapeMode, ETorqueFdb torqueFdb, EUImode uiMode)
            {
                ShapeMode = shapeMode;
                TorqueFdb = torqueFdb;
                UiMode = uiMode;
            }
        };

        private const string SqlCmdStr = "server=127.0.0.1;port=3308;user=root;password=1598960748;database=angletest_visual";

        private static List<Combo> combinations = new List<Combo>();
        private static int currentIndex = 0; // 用于访问数组中的下一个组合
        private bool isReset = false;
        private string formName;
        private int TargetAngle;
        private Stopwatch stopwatch;
        private int Posset=0;

        private EShapeMode Smode;
        private ETorqueFdb Tmode;
        private EUImode Umode;
        private SerialPort serialPort = new SerialPort();
        private DataRx datarx;
        private DataTx datatx;
        private DispatcherTimer timer;
        private UInt32 timeTick = 0;
        private SerialManager serialManager;
        private int detent = 0;
        private float posfdb;
        private CancellationTokenSource _cancellationTokenSource;
        public AngleTestWindow()
        {
            InitializeComponent();

            // 生成所有可能的组合
            GenerateCombinations();

            // 打乱组合的顺序
            Shuffle(combinations);

            InitializeSerialPort();
            serialManager = new SerialManager(serialPort, SerialComboBox);
            datatx = new DataTx(serialPort);
            datarx = new DataRx(serialPort);
            serialPort.DataReceived += datarx.Serial_DataReceived;
            timer = new DispatcherTimer();
            // 设置定时器的间隔为 1 毫秒
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();
            stopwatch=new Stopwatch();


        }
        private void InitializeSerialPort()
        {
            // 配置串口参数
            serialPort.BaudRate = 115200;             // 波特率
            serialPort.Parity = Parity.None;        // 校验位
            serialPort.DataBits = 8;                // 数据位
            serialPort.StopBits = StopBits.One;     // 停止位
            serialPort.Handshake = Handshake.None;  // 流控
            serialPort.ReadBufferSize = 8192;
            serialPort.WriteBufferSize = 8192;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeTick++;

            // 每分钟重置串口连接
            if (timeTick % 6000 == 0) // 定时器每 10ms 触发，所以 60000ms 是 6000 个周期
            {
                SerialConnectUpdate();
            }
            detent = datarx.GetDetent();
            posfdb = -datarx.GetPositionFdb();// + (float)(Math.PI * 3.0f) / 4.0f;
            posfdb = posfdb < 0.0f ? posfdb+(float)Math.PI*2.0f : posfdb;

            if (currentIndex >= combinations.Count)
            {
                Console.WriteLine("已经访问所有组合!");
                this.Close();
            }
            else
            {
                Smode = combinations[currentIndex].ShapeMode;
                Tmode = combinations[currentIndex].TorqueFdb;
                Umode = combinations[currentIndex].UiMode;
            }


            if (!isReset)
            {
                switch (Smode)
                {
                    case EShapeMode.defalut:
                        datatx.SetDefMode(DataTx.DeformationMode.SmoothState);
                        break;
                    case EShapeMode.point:
                        datatx.SetDefMode(DataTx.DeformationMode.PointState);
                        break;
                    case EShapeMode.three_detent:
                        datatx.SetDefMode(DataTx.DeformationMode.ThreeState);
                        break;
                    case EShapeMode.six_detent:
                        datatx.SetDefMode(DataTx.DeformationMode.SixState);
                        break;
                }

                switch (Tmode)
                {
                    case ETorqueFdb.NF:
                        datatx.SetTorFdbMode(DataTx.TorqueFdbMode.Smooth);
                        Posset= (int)((posfdb / Math.PI) * 180);
                        break;
                    case ETorqueFdb.fric:
                        datatx.SetTorFdbMode(DataTx.TorqueFdbMode.Fric);
                        Posset= (int)((posfdb / Math.PI) * 180);
                        break;
                    case ETorqueFdb.ten_detent:
                        datatx.SetDetent(36);
                        Posset= 360 - detent * 10;
                        datatx.SetTorFdbMode(DataTx.TorqueFdbMode.Detents);
                        break;
                    case ETorqueFdb.detent_thrity:
                        datatx.SetDetent(12);
                        Posset= 360 - detent * 30;
                        datatx.SetTorFdbMode(DataTx.TorqueFdbMode.Detents);
                        break;
                }
                progressBar.Percentage = 56;
                switch (Umode)
                {
                    case EUImode.degree_30:
                        TitleNameTextBlock.Text = "Please turn to: 30°";
                        TargetAngle = 30;
                        break;
                    case EUImode.degree_120:
                        TitleNameTextBlock.Text = "Please turn to: 120°";
                        TargetAngle = 120;
                        break;
                    case EUImode.degree_330:
                        TitleNameTextBlock.Text = "Please turn to: 330°";
                        TargetAngle = 330;
                        break;


                }
                //progressBar.Percentage = TargetAngle;
            }

            // 发送数据
            SendData();
        }

        private async void SerialConnectUpdate()
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                    serialPort.PortName = SerialComboBox.SelectedItem.ToString().Split(':')[0];
                    await Task.Delay(250); // 使用 Task.Delay 而不是 Thread.Sleep
                    serialPort.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Reset Serial Failed! :{ex.Message}");
                }
            }
        }

        private void SendData()
        {
            // 异步发送数据
            //Todo



            Task.Run(() => SendMessageAsync());
        }

        private async Task SendMessageAsync()
        {
            if (serialPort.IsOpen)
            {
                try
                {

                    datatx.sendMsg(); // 发送消息
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Data Send Failed: {ex.Message}");
                    });
                }
            }

            // 可选：根据需求调整发送间隔
            await Task.Delay(5); // 这个延迟可以根据需要调整
        }

        private void SerialConnectButton_Click(object sender, EventArgs e)
        {
            serialManager.ConnectOrDisconnectSerial(SerialConnectButton);
        }

        private static void GenerateCombinations()
        {
            var shapeModes = Enum.GetValues(typeof(EShapeMode));
            var torqueFdbs = Enum.GetValues(typeof(ETorqueFdb));
            var uiModes = Enum.GetValues(typeof(EUImode));

            foreach (EShapeMode shapeMode in shapeModes)
            {
                foreach (ETorqueFdb torqueFdb in torqueFdbs)
                {
                    foreach (EUImode uiMode in uiModes)
                    {
                        // 将每种组合重复两次
                        combinations.Add(new Combo(shapeMode, torqueFdb, uiMode));
                        combinations.Add(new Combo(shapeMode, torqueFdb, uiMode));
                    }
                }
            }
        }

        // 获取下一个组合
        private static Combo GetNextCombination()
        {
            // 确保当前索引不会超出范围
            if (currentIndex >= combinations.Count)
            {
                Console.WriteLine("已经访问所有组合!");
                return default; // 返回默认的组合
            }

            var combo = combinations[currentIndex];
            currentIndex++; // 移动到下一个组合
            return combo;
        }

        // Fisher-Yates 洗牌算法来打乱列表顺序
        private static void Shuffle(List<Combo> list)
        {
            Random rand = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                stopwatch.Stop();
                SaveDataToTable();
                isReset = true;
                datatx.SetTorFdbMode(DataTx.TorqueFdbMode.DefaultTorque);
                datatx.SetDefMode(DataTx.DeformationMode.SmoothState);
                detent = 0;
                posfdb = 0.0f;
                progressBar.Percentage = 0;
            }
            else if (e.Key == Key.D)
            {
                currentIndex++;
                isReset = false;
                stopwatch.Reset();
                stopwatch.Start(); 
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string addstr = $"insert into userinfo(m_name,m_age,m_sex) values('{NameTextBox.Text}','{int.Parse(AgeTextBox.Text)}','{SexTextBox.Text}')";
            formName = NameTextBox.Text + "AngleTextWithOutVisual";
            string newTablesFiles = $"CREATE TABLE `{formName}` (Step INT, AnsPosFdb INT, PhyPosFdb INT,Acc INT,TorqueFdbMode VARCHAR(50),DeforFdbMode VARCHAR(50),UiMode VARCHAR(50),time FLOAT);";
            MySQLOperationClass mscop = new MySQLOperationClass(SqlCmdStr);
            mscop.updateMySql(addstr);
            mscop.TableCreated(newTablesFiles);
            ConfirmButton.Content = "Confirmed!";
            ConfirmButton.IsEnabled = false;
            NameTextBox.IsEnabled = false;
            SexTextBox.IsEnabled = false;
            AgeTextBox.IsEnabled = false;

            stopwatch.Start();
           
        }


        private void SaveDataToTable()
        {
            //first find table name
            string tmode = $"{Tmode}";
            string smode = $"{Smode}";
            string umode = $"{Umode}";
            int acc=Math.Abs(Posset-TargetAngle);
            float time=(float)stopwatch.Elapsed.TotalMilliseconds*0.001f;
            if(progressBar.Percentage==360)
            {
                progressBar.Percentage = 0;
            }

            string addstr = $"insert into {formName}(Step,AnsPosFdb,PhyPosFdb,Acc,TorqueFdbMode,DeforFdbMode,UiMode,time) values('{currentIndex+1}','{Posset}','{TargetAngle}','{acc}','{tmode}','{smode}','{umode}','{time}')";

            MySQLOperationClass mscop = new MySQLOperationClass(SqlCmdStr);
            mscop.updateMySql(addstr);
        }
    }
}
