using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SerialChartDrawer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //Why I can't using datagroup...
        private ObservableDataSource<Point> dataSource1 = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource2 = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource3 = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource4 = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource11 = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource12 = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource13 = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource14 = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource21 = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource22 = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource23 = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource24 = new ObservableDataSource<Point>();
        private DispatcherTimer timer = new DispatcherTimer();
        private int i = 0;
        SerialPort com;
        private bool[] chartIsinit = new bool[12];

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += Window_Closing;
            comboboxInit();
            readUserOption();
        }
        
        
        private ObservableDataSource<Point> getDataSourceName(int group)
        {
            ObservableDataSource<Point> getDataSource;
            switch (group)
            {
                case 0:
                    getDataSource = dataSource1;
                    break;
                case 1:
                    getDataSource = dataSource2 ;
                    break;
                case 2:
                    getDataSource = dataSource3 ;
                    break;
                case 3:
                    getDataSource = dataSource4 ;
                    break;
                case 4:
                    getDataSource = dataSource11 ;
                    break;
                case 5:
                    getDataSource = dataSource12 ;
                    break;
                case 6:
                    getDataSource = dataSource13 ;
                    break;
                case 7:
                    getDataSource = dataSource14 ;
                    break;
                case 8:
                    getDataSource = dataSource21 ;
                    break;
                case 9:
                    getDataSource = dataSource22 ;
                    break;
                case 10:
                    getDataSource = dataSource23 ;
                    break;
                case 11:
                    getDataSource = dataSource24 ;
                    break;
                default:
                    getDataSource = dataSource1;
                    break;
            }
            return getDataSource;
        }

        /// <summary>
        /// Add net point to source
        /// </summary>
        /// <param name="x">data1</param>
        /// <param name="y">data2</param>
        private void addPoint(double x, double y, int group)
        {
            Point point = new Point(x, y);
            getDataSourceName(group).AppendAsync(base.Dispatcher, point);
            Chart_load(group);
        }

        /// <summary>
        /// Timer active function,make new point drawing to chart after Window_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimatedPlot(object sender, EventArgs e)
        {

            addPoint(Math.Cos(10 + i), Math.Sin(9 + i), 0);
            i += 1;
        }
       
        /// <summary>
        /// Get random color
        /// </summary>
        /// <returns>A 0-156 Color arch</returns>
        private Color getRandomColor()
        {
            Random rnd = new Random();
            Color myColor = Color.FromRgb(
         Convert.ToByte(rnd.Next(0, 156)), /*红色*/
         Convert.ToByte(rnd.Next(0, 156)), /*绿色*/
         Convert.ToByte(rnd.Next(0, 155)) /*蓝色*/ );

            return myColor;
        }
        private void Chart_load(int group)
        {
            if (chartIsinit[group] == false)
            {
                MainChart.AddLineGraph(getDataSourceName(group), getRandomColor(), 1, "Group"+group);
                chartIsinit[group] = true;
                MainChart.Viewport.FitToView();
            }
            else
                MainChart.Viewport.FitToView();


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = TimeSpan.FromSeconds(0.03);
            timer.Tick += new EventHandler(AnimatedPlot);
            MainChart.Viewport.FitToView();
        }

        private void btSetSerial_Click(object sender, RoutedEventArgs e)
        {
            //  showSerialSetting();
        }

        private void machineStart()
        {
            timer.IsEnabled = true;
            btSettingApply.Background = new SolidColorBrush(Colors.IndianRed);
            btSettingApply.Content = "停止";
        }
        private void machineStop()
        {
            timer.IsEnabled = false;
            btSettingApply.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
            btSettingApply.Content = "开始测试";
        }

        private void serial_OptionSend(int voltage,int cl,int gnv)
        {
            
        }

        /// <summary>
        /// Apply all setting and start data record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSettingApply_Click(object sender, RoutedEventArgs e)
        {
            if (btSettingApply.Content.ToString() != "停止")
            {
                MessageBoxResult result = MessageBox.Show("是否应用当前配置并启动？", "确认", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    //  machineStart();
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                //  machineStop();
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.Key == Key.Space)
                {
                    btSettingApply_Click(sender, e);
                }
            }
            catch
            {

            }
        }
        public void comboboxInit()
        {

            //Init serial using combobox
            cbBaud.Items.Add("9600");//添加常用的波特率
            cbBaud.Items.Add("115200");
            cbBaud.SelectedIndex = 0;
            foreach (string portname in SerialPort.GetPortNames())//get device name
            {
                cbCommunicatePort.ItemsSource = MulGetHardwareInfo(HardwareEnum.Win32_PnPEntity, "Name");
                cbCommunicatePort.SelectedIndex = 0;
            }

            //Init cbLineGroup
            int i = 0;
            while (i < chartIsinit.Length)
            {
                cbLineGroup.Items.Add(i++);
             }
            cbLineGroup.SelectedIndex = 0;
            autoSelectport();

        }
        private void autoSelectport()
        {
            foreach (string item in cbCommunicatePort.Items)
            {
                if (item.Contains(GetPort(tbDefautlDevice.Text)))
                {
                    cbCommunicatePort.SelectedItem = item;
                }
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("是否要关闭？", "确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                e.Cancel = false;
                saveUserOption();
            }
            else
            {
                e.Cancel = true;
            }



        }
        private void readUserOption()
        {
            byte[] bydata = new byte[256];
            char[] charData = new char[256];
            try
            {
                FileStream fs = new FileStream("serialOption.ini", FileMode.Open);
                StreamReader sw = new StreamReader(fs);
                tbDefautlDevice.Text = sw.ReadLine();
                sw.Close();
                fs.Close();
            }
            catch
            {
                MessageBox.Show("读取配置文件错误,重新建立配置文件");
                FileStream fs = new FileStream("serialOption.ini", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(tbDefautlDevice.Text);
                sw.Close();
                fs.Close();
            }
        }
        private void saveUserOption()
        {
            try
            {
                FileStream fs = new FileStream("serialOption.ini", FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(tbDefautlDevice.Text);
                sw.Close();
                fs.Close();
            }
            catch
            {
                MessageBox.Show("写入配置文件错误");
            }
        }
        

        private void btConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                com = new SerialPort(GetPort(cbCommunicatePort.Text), (Convert.ToInt16(cbBaud.Text)));
                if (com.IsOpen == false)
                {
                    com.Open();
                    btConnect.Content = "关闭连接";
                }
                else
                {
                    com.Close();
                    btConnect.Content = "连接";
                    
                }
            }
            catch
            {
                MessageBox.Show("打开失败，检查串口是否被占用");
            }
        }
        /// <summary>
        /// 枚举win32 api
        /// </summary>
        public enum HardwareEnum
        {
            // 硬件
            Win32_Processor, // CPU 处理器
            Win32_PhysicalMemory, // 物理内存条
            Win32_Keyboard, // 键盘
            Win32_PointingDevice, // 点输入设备，包括鼠标。
            Win32_FloppyDrive, // 软盘驱动器
            Win32_DiskDrive, // 硬盘驱动器
            Win32_CDROMDrive, // 光盘驱动器
            Win32_BaseBoard, // 主板
            Win32_BIOS, // BIOS 芯片
            Win32_ParallelPort, // 并口
            Win32_SerialPort, // 串口
            Win32_SerialPortConfiguration, // 串口配置
            Win32_SoundDevice, // 多媒体设置，一般指声卡。
            Win32_SystemSlot, // 主板插槽 (ISA & PCI & AGP)
            Win32_USBController, // USB 控制器
            Win32_NetworkAdapter, // 网络适配器
            Win32_NetworkAdapterConfiguration, // 网络适配器设置
            Win32_Printer, // 打印机
            Win32_PrinterConfiguration, // 打印机设置
            Win32_PrintJob, // 打印机任务
            Win32_TCPIPPrinterPort, // 打印机端口
            Win32_POTSModem, // MODEM
            Win32_POTSModemToSerialPort, // MODEM 端口
            Win32_DesktopMonitor, // 显示器
            Win32_DisplayConfiguration, // 显卡
            Win32_DisplayControllerConfiguration, // 显卡设置
            Win32_VideoController, // 显卡细节。
            Win32_VideoSettings, // 显卡支持的显示模式。

            // 操作系统
            Win32_TimeZone, // 时区
            Win32_SystemDriver, // 驱动程序
            Win32_DiskPartition, // 磁盘分区
            Win32_LogicalDisk, // 逻辑磁盘
            Win32_LogicalDiskToPartition, // 逻辑磁盘所在分区及始末位置。
            Win32_LogicalMemoryConfiguration, // 逻辑内存配置
            Win32_PageFile, // 系统页文件信息
            Win32_PageFileSetting, // 页文件设置
            Win32_BootConfiguration, // 系统启动配置
            Win32_ComputerSystem, // 计算机信息简要
            Win32_OperatingSystem, // 操作系统信息
            Win32_StartupCommand, // 系统自动启动程序
            Win32_Service, // 系统安装的服务
            Win32_Group, // 系统管理组
            Win32_GroupUser, // 系统组帐号
            Win32_UserAccount, // 用户帐号
            Win32_Process, // 系统进程
            Win32_Thread, // 系统线程
            Win32_Share, // 共享
            Win32_NetworkClient, // 已安装的网络客户端
            Win32_NetworkProtocol, // 已安装的网络协议
            Win32_PnPEntity,//all device
        }
        /// <summary>
        /// WMI取硬件信息
        /// </summary>
        /// <param name="hardType"></param>
        /// <param name="propKey"></param>
        /// <returns></returns>
        public static string[] MulGetHardwareInfo(HardwareEnum hardType, string propKey)
        {

            List<string> strs = new List<string>();
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + hardType))
                {
                    var hardInfos = searcher.Get();
                    foreach (var hardInfo in hardInfos)
                    {
                        if (hardInfo.Properties[propKey].Value != null)
                            if (hardInfo.Properties[propKey].Value.ToString().Contains("COM"))
                            {
                                strs.Add(hardInfo.Properties[propKey].Value.ToString());
                            }

                    }
                    searcher.Dispose();
                }
                return strs.ToArray();
            }
            catch
            {
                return null;
            }
            finally
            { strs = null; }
        }
        //通过WMI获取COM端口
        /// <summary>
        /// 返回COM开头的真实COM口
        /// </summary>
        /// <param name="targetPortname"></param>
        /// <returns></returns>
        public string GetPort(string targetPortname)
        {
            string[] ss = MulGetHardwareInfo(HardwareEnum.Win32_PnPEntity, "Name");
            string pattern = @"(?is)(?<=\()[^\)]+(?=\))";//匹配模式
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            foreach (string port in ss)
            {
                if (port.Contains(targetPortname))
                {
                    MatchCollection matches = regex.Matches(port);
                    foreach (Match m in matches)
                    {
                        return m.ToString();
                    }
                }
            }
            return null;
        }

        private void btDefDevice_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("设置" + cbCommunicatePort.Text + "为默认设备？", "确定？", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                tbDefautlDevice.Text = cbCommunicatePort.Text;
            }
            else
            {
                return;
            }
        }

        private void btTestEnter_Click(object sender, RoutedEventArgs e)
        {
            addPoint(Convert.ToDouble(tbX.Text), Convert.ToDouble(tbY.Text),Convert.ToInt16( cbLineGroup.Text));
        }


        /// <summary>
        /// use regular and make specific textbox only enter number 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)

        {
            Regex re = new Regex("[^0-9.-]+");

            e.Handled = re.IsMatch(e.Text);

        }

        /// <summary>
        /// user can use wheel to change number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberWheelchange(object sender, MouseWheelEventArgs e)
        {
            if(tbX.IsSelectionActive)
            {
               if(tbX.Text != null)
                {
                    tbX.Text = (Convert.ToInt16(tbX.Text) + e.Delta/120).ToString();
                }
            }
            if(tbY.IsSelectionActive)
            {
                if (tbY.Text != null)
                {
                    tbY.Text = (Convert.ToInt16(tbY.Text) + e.Delta / 120).ToString();
                }
            }
        }


    }
}
