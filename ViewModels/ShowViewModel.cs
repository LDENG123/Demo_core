using Caliburn.Micro;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using ACS.SPiiPlusNET;
using System.Threading.Tasks;
using System.Threading;
using Advantech.Motion;
using Panuon.UI.Silver;
using NPOI.SS.UserModel;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using System.Collections.ObjectModel;
using System.IO;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Text;
using Demo_core.Models;
using MessageBox = System.Windows.MessageBox;
using LiveCharts;
using LiveCharts.Configurations;
using System.Windows;
using Demo_core.ExtendViews.ExtendViewModels;
using Demo_core.ExtendViews;

namespace Demo_core.ViewModels
{

    class ShowViewModel : Caliburn.Micro.Screen, INotifyPropertyChanged,IHandle<string>
    {
        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        #region
        private AxActProgTypeLib.AxActProgType axActProgType1;
        private readonly IEventAggregator _eventAggregator;
        public ShowViewModel()
        {
            _eventAggregator = IoC.Get<IEventAggregator>();
            _eventAggregator.SubscribeOnPublishedThread(this);
            actDefine = new ActDefine();
            xls_creat();
            axActProgType1 =new AxActProgTypeLib.AxActProgType() ;
            #region realchart
            var mapper = Mappers.Xy<MeasureModel>()
         .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
         .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);

            //the values property will store our values array
            ChartValues = new ChartValues<MeasureModel>();

            //lets set how to display the X Labels
            DateTimeFormatter = value => new DateTime((long)value).ToString("hh.mm:ss");

            //AxisStep forces the distance between each separator in the X axis
            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            //AxisUnit forces lets the axis know that we are plotting seconds
            //this is not always necessary, but it can prevent wrong labeling
            AxisUnit = TimeSpan.TicksPerSecond;

            SetAxisLimits(DateTime.Now);

            //The next code simulates data changes every 300 ms

            IsReading = false;

            #endregion
            Sql_Load();
            Logger.Debug("Backgroundtask stopped.");
        }
        #endregion
        #region acs Fild
        private int slectAxis;
        public int SlectAxis
        {
            get
            {

                return slectAxis;
            }
            set
            {
                slectAxis = value;
                NotifyOfPropertyChange(() => SlectAxis
);
            }
        }
        private int connect_Type;
        public int Connect_Type
        {
            get
            {

                return connect_Type;
            }
            set
            {
                connect_Type = value;
                NotifyOfPropertyChange(() => connect_Type
);
            }
        }
        private bool romotconnect;
        public bool RomotConnect
        {
            get
            {

                return romotconnect;
            }
            set
            {
                romotconnect = value;
                NotifyOfPropertyChange(() => RomotConnect
);
            }
        }
       
        private bool isconnect;
        public bool IsConnect
        {
            get
            {

                return isconnect;
            }
            set
            {
                isconnect = value;
                NotifyOfPropertyChange(() => IsConnect
);
            }
        }
        private bool isenable;
        public bool IsEnable
        {
            get
            {

                return isenable;
            }
            set
            {
                isenable = value;
                if (value == true)
                { EnableAxis(); }
                else
                {; }
                NotifyOfPropertyChange(() => IsEnable
);
            }
        }
        private String  ip;
        public String IP
        {
            get
            {

                return ip;
            }
            set
            {
               // Regex.Match(IP, @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
                ip = value;
                NotifyOfPropertyChange(() => IP);
            }
        }
        private String message;
        public String Message
        {
            get
            {

                return message;
            }
            set
            {
                message = value;
              
                NotifyOfPropertyChange(() => Message);
            }
        }
        private String axisActPoistion;
        public String AxisActPoistion
        {
            get
            {

                return axisActPoistion;
            }
            set
            {
                axisActPoistion = value;
                NotifyOfPropertyChange(() => AxisActPoistion);
            }
        }
        #endregion
        #region ACS  command
        private Command.NewCommand connect;
        public Command.NewCommand Connect
        {
           
                get
        {
                    if (connect == null)
                        connect = new Command.NewCommand(
                            new Action<object>(
                            e =>
                            {
                                Connect_asc();
                                MessageBox.Show("123");
                            }));
                    return connect;
                }
            }
        private Command.NewCommand jog;
        public Command.NewCommand Jog
        {

            get
            {
                if (jog == null)
                    jog = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            Ascapi.Enable(Axis.ACSC_AXIS_0);
                            JogAxis();
                        }));
                return jog;
            }
        }
        #endregion
        #region ACS  FUNC
        Api Ascapi;
        private void Connect_asc()
        {
            Ascapi = new Api();
            // Ascapi.OpenCommEthernet(IP,701);
            Ascapi.OpenCommSimulator();
            //  // Get connection info
             ACSC_CONNECTION_INFO AscAxisInfo = Ascapi.GetConnectionInfo();
           IP= AscAxisInfo.Type.ToString();
        }
        private void EnableAxis()
        {
            Ascapi.Enable(Axis.ACSC_AXIS_0);
        }
        private void JogAxis()
        {
            Ascapi.Jog(MotionFlags.ACSC_NONE,Axis.ACSC_AXIS_0,1000);
            AxisActPoistion = Ascapi.GetFPosition(Axis.ACSC_AXIS_0).ToString();
        }

        public  Task HandleAsync(string message, CancellationToken cancellationToken)
        {
            Message = message;
            return Task.FromResult(Message);
        }
        #endregion
        #region Mitsushi Fild
        ActDefine actDefine;
      
        private bool isconnectMitsushi;
        public bool IsConnectMitsushi
        {
            get
            {

                return isconnectMitsushi;
            }
            set
            {
                isconnectMitsushi = value;
                NotifyOfPropertyChange(() => IsConnectMitsushi
);
            }
        }

        private String ipMitsushi;
        public String IPMitsushi
        {
            get
            {

                return ipMitsushi;
            }
            set
            {
                // Regex.Match(IPMitsushi, @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
                ip = value;
                NotifyOfPropertyChange(() => IPMitsushi);
            }
        }
        private int portMitsushi;
        public int PortMitsushi
        {
            get
            {

                return portMitsushi;
            }
            set
            {
                portMitsushi = value;
                NotifyOfPropertyChange(() => PortMitsushi);
            }
        }
        #endregion
        #region Mitsushi  command
        private Command.NewCommand connectMitsushi;
        public Command.NewCommand ConnectMitsushi
        {

            get
            {
                if (connectMitsushi == null)
                    connectMitsushi = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            _eventAggregator.PublishOnUIThreadAsync(Message);
                          
                        }));
                return connectMitsushi;
            }
        }
        private Window ExtendWindow;
        private Command.NewCommand readMitsushi;
        public Command.NewCommand ReadMitsushi
        {

            get
            {
                if (readMitsushi == null)
                    readMitsushi = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            if (ExtendWindow == null)
                            {ExtendWindow = new SystemInformationView(); }
                            
                            ExtendWindow.Show();

                        }));
                return readMitsushi;
            }
        }
        #endregion
        #region 研华测试
        private bool isenableaxis;
        public bool Isenableaxis 
        {
            get
            {

                return isenableaxis;
            }
            set
            {
                isenableaxis = value;
                if (isenableaxis == true)
                {
                    Motion.mAcm_AxSetSvOn(m_Axishand[0], 1);
                }
                else
                    Motion.mAcm_AxSetSvOn(m_Axishand[0], 0);
                NotifyOfPropertyChange(() => Isenableaxis
);
            }
        }
        private int devsSlect;
        public int DevsSlect
        {
            get
            {

                return devsSlect;
            }
            set
            {
                devsSlect = value;
                NotifyOfPropertyChange(() => DevsSlect
);
            }
        }
        private int axisSlect;
        public int AxisSlect {
            get
            {

                return axisSlect;
            }
            set
            {
                axisSlect = value;
                NotifyOfPropertyChange(() => AxisSlect
);
            }
        }
        private string acmAxis_GetAvailableDevs; 
        public string AcmAxis_GetAvailableDevs
        {
            get
            {

                return acmAxis_GetAvailableDevs;
            }
            set
            {
                acmAxis_GetAvailableDevs = value;
                NotifyOfPropertyChange(() => AcmAxis_GetAvailableDevs
);
            }
        }
        private string acmAxis_GetActPos;
        public string AcmAxis_GetActPos
        {
            get
            {

                return acmAxis_GetActPos;
            }
            set
            {
                acmAxis_GetActPos = value;
                NotifyOfPropertyChange(() => AcmAxis_GetActPos
);
            }
        }
        private string acmAxis_GetAxisStates;
        public string AcmAxis_GetAxisStates
        {
            get
            {

                return acmAxis_GetAxisStates;
            }
            set
            {
                acmAxis_GetAxisStates = value;
                NotifyOfPropertyChange(() => AcmAxis_GetAxisStates
);
            }
        }
        private ObservableCollection<string> availableAxis=new ObservableCollection<string>();
        public ObservableCollection<string> AvailableAxis
        {
            get
            {

                return availableAxis;
            }
            set
            {
                availableAxis = value;
                NotifyOfPropertyChange(() => AvailableAxis
);
            }
        }
        private ObservableCollection<string> availableDevs=new ObservableCollection<string>();
        public ObservableCollection<string> AvailableDevs
        {
            get
            {

                return availableDevs;
            }
            set
            {
                availableDevs = value;
                NotifyOfPropertyChange(() => AvailableDevs
);
            }
        }
        #endregion
        #region 研华  command 
        private void Act_GetPOS()
        { double POS=0;
            uint STATES = 0;
            Task.Run(() =>
            {
                while (true)
                {

                    var TU=  Motion.mAcm_AxGetCmdPosition(m_Axishand[0], ref POS);
                    var TU0=   Motion.mAcm_AxGetMotionStatus(m_Axishand[0],ref STATES);
                    AcmAxis_GetActPos = POS.ToString();
                    Thread.Sleep(100); ;
                }


            });


        }
        DEV_LIST[] CurAvailableDevs = new DEV_LIST[Motion.MAX_DEVICES];
      
        uint deviceCount = 0;
        uint DeviceNum = 0;
        IntPtr m_DeviceHandle = IntPtr.Zero;
        IntPtr[] m_Axishand = new IntPtr[64];
        uint m_AxisCount = 0;
        uint DIChanNum = 0;
        bool m_bInit = false;
        bool m_bServoOn = false;
        Lib.PCIE_1203Initialize pCIE_1203Initialize;
        private Command.NewCommand adlink;
        public Command.NewCommand Adlink
        {

            get
            {
                if (adlink == null)
                    adlink = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            // int i = 0;
                            // var valausout=  Motion.mAcm_GetAvailableDevs(CurAvailableDevs, Motion.MAX_DEVICES, ref deviceCount);
                            // if (deviceCount != 0 )
                            // {
                            //     AcmAxis_GetAvailableDevs = CurAvailableDevs[0].DeviceName;
                            //     AvailableDevs.Add("0");
                            // var reult=    Motion.mAcm_DevOpen(CurAvailableDevs[0].DeviceNum, ref m_DeviceHandle);
                            // var Result = Motion.mAcm_GetU32Property(m_DeviceHandle, (uint)PropertyID.FT_DevAxesCount, ref m_AxisCount);
                            // AvailableAxis.Clear(); 
                            //if(Result==0)
                            //     { 
                            //     for (i = 0; i < m_AxisCount; i++)
                            //     {
                            //         AvailableAxis.Add(String.Format("{0:d}-Axis", i));

                            //     }
                            //     }
                            //     }
                            pCIE_1203Initialize = new Lib.PCIE_1203Initialize();
                        }));
                return adlink;
            }
        }
        private Command.NewCommand adlinkservon;
        public Command.NewCommand Adlinkservon
        {

            get
            {
                if (adlinkservon == null)
                    adlinkservon = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                           var Result = Motion.mAcm_AxOpen(m_DeviceHandle, 0, ref m_Axishand[0]);
                            var TU=   Motion.mAcm_AxJog(m_Axishand[0], 1);
                            Act_GetPOS();
                        }));
                return adlinkservon;
            }
        }
        private double cmd_Pos;
        public Double Cmd_Pos
        {
            get
            {

                return cmd_Pos;
            }
            set
            {
                cmd_Pos = value;
                NotifyOfPropertyChange(() => Cmd_Pos
);
            }
        }
        private double act_Pos;
        public Double Act_Pos
        {
            get
            {

                return act_Pos;
            }
            set
            {
                act_Pos = value;
                NotifyOfPropertyChange(() => Act_Pos
);
            }
        }
        private string axis_States;
        public string Axis_States
        {
            get
            {

                return axis_States;
            }
            set
            {
                axis_States = value;
                NotifyOfPropertyChange(() => Axis_States
);
            }
        }
        private ushort axis_State;
        public ushort Axis_State
        {
            get
            {

                return axis_State;
            }
            set
            {
                axis_State = value;
                Application.Current.Dispatcher.BeginInvoke((System.Action)(() =>
                {
                    GetAxis_State(axis_State);
                }));
                NotifyOfPropertyChange(() => Axis_State
);
            }
        }
        private void GetAxis_State(ushort state)
        {
            switch (state)
            {
                case 0:
                    Axis_States=" STA_AxDisable";
                    break;
                case 1:
                    Axis_States = " STA_AxReady";
                    break;
                case 2:
                    Axis_States = " STA_Stopping";
                    break;
                case 3:
                    Axis_States = " STA_AxErrorStop";
                    break;
                case 4:
                    Axis_States = " STA_AxHoming";
                    break;
                case 5:
                    Axis_States = " STA_AxPtpMotion";
                    break;
                case 6:
                    Axis_States = " STA_AxContiMotion";
                    break;
                case 7:
                    Axis_States = " STA_AxSyncMotion";
                    break;
            }
        }
        private uint motion_State;
        public uint Motion_State
        {
            get
            {
                return motion_State;
            }
            set
            {
                motion_State = value;
                Application.Current.Dispatcher.BeginInvoke((System.Action)(() =>
                {
                    GetMotion_State(motion_State);
                }));
                NotifyOfPropertyChange(() => Motion_State
);
            }
        }
        private string motion_States;
        public string Motion_States
        {
            get
            {
                return motion_States;
            }
            set
            {
                motion_States = value;
                NotifyOfPropertyChange(() => Motion_States
);
            }
        }
        private void GetMotion_State(uint state)
        {
            switch (state)
            {
                case 1:
                    Motion_States = " Stop";
                    break;
                case 2:
                    Motion_States = " Res";
                    break;
                case 4:
                    Motion_States = " WaitERC";
                    break;
                case 8:
                    Motion_States = " Res";
                    break;
                case 16:
                    Motion_States = " CorrectBksh";
                    break;
                case 32:
                    Motion_States = " Res";
                    break;
                case 64:
                    Motion_States = " InFA";
                    break;
                case 128:
                    Motion_States = " InFL";
                    break;
                case 256:
                    Motion_States = " InACC";
                    break;
                case 512:
                    Motion_States = " InFH";
                    break;
                case 1024:
                    Motion_States = " InDEC";
                    break;
                case 2048:
                    Motion_States = " WaitINP";
                    break;
            }
        }
        private uint return_State;
        public uint Return_State
        {
            get
            {

                return return_State;
            }
            set
            {
                Application.Current.Dispatcher.BeginInvoke((System.Action)(() =>
                {
                    Act_Pos = act_Pos;
                    Cmd_Pos = cmd_Pos;
                    Axis_States = ((AxisState)axis_State).ToString();
                    //Motion_State = motion_State;
                    
                    //MessageBox.Show($"实际位置{Act_Pos}命令位置{Cmd_Pos}轴状态{Axis_State}运动状态{ Motion_State} ");
                }));
                //GetAxis_State(axis_State);
                    GetMotion_State(motion_State);
                return_State = value;
                NotifyOfPropertyChange(() => Return_State
);
            }
        }
        private Command.NewCommand jogp;
        public Command.NewCommand Jogp
        {

            get
            {
                if (jogp == null)
                    jogp = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {

                            pCIE_1203Initialize.Jog(0, true, false, 1000, 1000, 100);
                            Return_State= pCIE_1203Initialize.Axis_State(0, ref act_Pos,ref cmd_Pos, ref axis_State, ref motion_State);
                        }));
                return jogp;
            }
        }
        private Command.NewCommand jogn;
        public Command.NewCommand Jogn
        {

            get
            {
                if (jogn == null)
                    jogn = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            
                            ;
                            pCIE_1203Initialize.Jog(0, false, true, 1000, 1000, 100);
                        }));
                return jogn;
            }
        }
        private Command.NewCommand jogStop;
        public Command.NewCommand JogStop
        {

            get
            {
                if (jogStop == null)
                    jogStop = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            pCIE_1203Initialize.JogStop(0);
                            
                        }));
                return jogStop;
            }
        }
        private Command.NewCommand axisSlectCbxDropDownOpened;
        public Command.NewCommand AxisSlectCbxDropDownOpened
        {

            get
            {
                if (axisSlectCbxDropDownOpened == null)
                    axisSlectCbxDropDownOpened = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            NoticeX.Show("Opened", Panuon.UI.Silver.MessageBoxIcon.Success );

                        }));
                return axisSlectCbxDropDownOpened;
            }
        }
        private Command.NewCommand axisSlectCbxDropDownClosed;
        public Command.NewCommand AxisSlectCbxDropDownClosed
        {

            get
            {
                if (axisSlectCbxDropDownClosed == null)
                    axisSlectCbxDropDownClosed = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {

                            NoticeX.Show("Closed", Panuon.UI.Silver.MessageBoxIcon.Success);  
                        }));
                return axisSlectCbxDropDownClosed;
            }
        }
        private Command.NewCommand axisSlectCbxSelectionChanged;
        public Command.NewCommand AxisSlectCbxSelectionChanged
        {

            get
            {
                if (axisSlectCbxSelectionChanged == null)
                    axisSlectCbxSelectionChanged = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            NoticeX.Show("Changed", Panuon.UI.Silver.MessageBoxIcon.Success); NoticeX.Show("Success", Panuon.UI.Silver.MessageBoxIcon.Success);

                        }));
                return axisSlectCbxSelectionChanged;
            }
        }
        #endregion
        #region UDP通信
        Lib.SoceketSever_Lib soceketSever_Lib;
        private string act_Hight;
        public string Act_Hight { 
            get{ 

                return act_Hight;
            }   
            set
            {
                act_Hight = value;
                NotifyOfPropertyChange(() => Act_Hight
);
            }
        }
        private bool stop;
        public bool Stop
        {
            get
            {

                return stop;
            }
            set
            {
                stop = value;
                NotifyOfPropertyChange(() => Stop
);
            }
        }
        private Command.NewCommand red_Hight;
        public Command.NewCommand Red_Hight
        {

            get
            {
                if (red_Hight == null)
                    red_Hight = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            try {
                                soceketSever_Lib = new Lib.SoceketSever_Lib();
                                soceketSever_Lib.Socket_Udp();
                                Stop = true;
                                    InjectStopOnClick();
                                Task.Run(() =>
                                {
                                    while
                                    (Stop)
                                    {

                                        soceketSever_Lib.Socket_UdpSend();
                                        byte[] High_Return = soceketSever_Lib.Socket_UdpRevice();
                                        Act_Hight = Encoding.Default.GetString(High_Return,6,High_Return.Length-6);
                                        //Xls_write(0, DateTime.Now.ToString());
                                        //Xls_write(1, Act_Hight);
                                        Thread.Sleep(250);

                                    }

                                });
                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                        }));
                return red_Hight;
            }
        }
        private Command.NewCommand stop_Read;
        public Command.NewCommand Stop_Read
        {

            get
            {
                if (stop_Read == null)
                    stop_Read = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                           Stop = !Stop;
                            InjectStopOnClick();
                        }));
                return stop_Read;
            }
        }
        #endregion 
        #region EXCEL 读取写入
        #region
        HSSFWorkbook HSSFWorkbook;
        HSSFWorkbook HSSFWorkbook_re;
        private FileStream FileStreamfile;
        string xlsfilename;
        private void  xls_creat()
        {

             Task.Run(async() =>
            {

                HSSFWorkbook = new HSSFWorkbook();
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI Team";
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Subject = "NPOI SDK Example";
                HSSFWorkbook.DocumentSummaryInformation = dsi;
                HSSFWorkbook.SummaryInformation = si;
                ISheet hSSFSheet = HSSFWorkbook.CreateSheet("sheet1");
                xlsfilename = DateTime.Now.ToString("D") + ".xls";
                if (File.Exists(xlsfilename))
                {
                    //System.Windows.MessageBox.Show("该表已经存在");
                    await ((MetroWindow)System.Windows.Application.Current.MainWindow).ShowMessageAsync("", "该表已经存在");
                }
                else //第一行数据设置
                {
                    ///创建单元格风格
                    ICellStyle hSSFCellStyle = HSSFWorkbook.CreateCellStyle();
                    //style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    hSSFCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    hSSFCellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    IRow rows = hSSFSheet.CreateRow(0);
                    rows.CreateCell(0).SetCellValue("Time");
                    rows.CreateCell(1).SetCellValue("High");
                   
                    using (FileStreamfile = new FileStream(xlsfilename, FileMode.Create))
                    {
                        HSSFWorkbook.Write(FileStreamfile);
                    };
                }


            });

        }
        string str1 = null;
        private void Xls_write(int column_index, string data)
        {

            //string str = "E:/YJ_personal/上位机/Wpf_notice - 副本/WpfApp1/bin/Debug/excel/" + DateTime.Now.ToString("D") + ".xls";

            try
            {
                using (FileStream file = new FileStream(xlsfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    HSSFWorkbook_re = new HSSFWorkbook(file);//获取读取的文件
                    ISheet sheet = HSSFWorkbook_re.GetSheetAt(0);//读取第一个文档
                    int cellRows = sheet.LastRowNum;//读取最大行数 
                    ICell reult = sheet.GetRow(cellRows).GetCell(column_index);
                    IRow rows_frist = sheet.GetRow(cellRows);
                    ICellStyle HSSFCellStylecellStyle = HSSFWorkbook_re.CreateCellStyle();
                    HSSFCellStylecellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.000");
                    if (reult == null)
                    {
                        ICellStyle cell_style = rows_frist.CreateCell(column_index).CellStyle;
                        rows_frist.CreateCell(column_index).SetCellValue(data);
                        cell_style = HSSFCellStylecellStyle;
                    }
                    else if (reult != null)
                    {
                        var cellrows1 = cellRows + 1;
                        IRow rows = sheet.CreateRow(cellrows1);
                        ICellStyle cell_style = rows.CreateCell(column_index).CellStyle;
                        cell_style = HSSFCellStylecellStyle;
                        rows.CreateCell(column_index).SetCellValue(data);
                    }

                    using (FileStreamfile = new FileStream(xlsfilename, FileMode.Create))
                    {
                        HSSFWorkbook_re.Write(FileStreamfile);
                    };

                }
            }
            catch (Exception error)
            {
                ;

            }

        }
        #endregion
        #endregion
        #region SQL
        private ObservableCollection<SqlDb> products = new ObservableCollection<SqlDb>();
        public ObservableCollection<SqlDb> Products
        {
            get
            {

                return products;
            }
            set
            {
                products = value;
                NotifyOfPropertyChange(() => Products
);
            }
        }
        private ObservableCollection<Product1> products1 = new ObservableCollection<Product1>();
        public ObservableCollection<Product1> Products1
        {
            get
            {

                return products1;
            }
            set
            {
                products1 = value;
                NotifyOfPropertyChange(() => Products1
);
            }
        }
        private  void Sql_Load()
        {

            using (_12Context context = new _12Context())
            {
                context.Products1.Load();
                Products1 = context.Products1.Local.ToObservableCollection();

            }
        }

        #endregion
        #region realtimechart
        #region 实时曲线属性字段声明
        public ChartValues<MeasureModel> ChartValues { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }
        private double _axisMin;
        private double _axisMax;
        private double _trend;
        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                //OnPropertyChanged1("AxisMax");
                NotifyOfPropertyChange(() => AxisMax);
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                // OnPropertyChanged1("AxisMin");
                NotifyOfPropertyChange(() => AxisMin);
            }
        }
        public bool IsReading { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }
        public Func<double, string> Formatter { get; set; }
        #endregion
        #region 实时曲线
        private void Read()
        {
            var r = new Random();
            try {
                while (IsReading)
                {
                    Thread.Sleep(150);
                    var now = DateTime.Now;

                    _trend = double.Parse(Act_Hight);

                    ChartValues.Add(new MeasureModel
                    {
                        DateTime = now,
                        Value = _trend

                    });

                    SetAxisLimits(now);

                    //lets only use the last 150 values
                    if (ChartValues.Count > 50) ChartValues.RemoveAt(0);
                } }
            catch (Exception x)
            { NoticeX.Show("Error", Panuon.UI.Silver.MessageBoxIcon.Error); NoticeX.Show(x.ToString(), Panuon.UI.Silver.MessageBoxIcon.Error); }
        }

        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 1 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks; // and 8 seconds behind

        }

        private void InjectStopOnClick()
        {
            IsReading = !IsReading;
            if (IsReading) Task.Factory.StartNew(Read);

        }
        #endregion
        #endregion
        #region LOG
        #region FILD
        public StringBuilder stringBuilderlog = new StringBuilder();
        public StringBuilder StringBuilderlog {
            get { return stringBuilderlog; }
            set
            {
                stringBuilderlog = value;
                NotifyOfPropertyChange(() => StringBuilderlog);
            }
        }
        public string  document;
        public String Document
        {
            get { return document; }
            set
            {
                document = value;
                NotifyOfPropertyChange(() => Document);
            }
        }
        private Command.NewCommand loginfo;
        public Command.NewCommand LogInfo
        {

            get
            {
                if (loginfo == null)
                    loginfo = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            try
                            {
                                StringBuilderlog.AppendLine("123");
                                Document = "123";
                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                        }));
                return loginfo;
            }
        }
        #endregion
        #endregion
    }
    public class MeasureModel
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }
}
