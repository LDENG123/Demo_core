using Advantech.Motion;
using Caliburn.Micro;
using Panuon.UI.Silver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Demo_core.ViewModels
{
    class MannualViewModel : Caliburn.Micro.Screen, INotifyPropertyChanged, IHandle<string>
    {
        Lib.PCIE_1203Initialize pCIE_1203Initialize;
        Task IHandle<string>.HandleAsync(string message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #region inlin
        public MannualViewModel()
        {
            _CancellationToken = new CancellationTokenSource();
        }
        #endregion
        #region Command 
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
                if (Isenableaxis)
                { pCIE_1203Initialize.SeverOn(DevsSlect, true); }
                else
                { pCIE_1203Initialize.SeverOn(DevsSlect, false); }
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
        public int AxisSlect
        {
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
        private string acmAxis_SetAxisAcc;
        public string AcmAxis_SetAxisAcc
        {
            get
            {

                return acmAxis_SetAxisAcc;
            }
            set
            {
                acmAxis_SetAxisAcc = value;
                NotifyOfPropertyChange(() => AcmAxis_SetAxisAcc
);
            }
        }
        private string acmAxis_SetAxisJogVol;
        public string AcmAxis_SetAxisJogVol
        {
            get
            {

                return acmAxis_SetAxisJogVol;
            }
            set
            {
                acmAxis_SetAxisJogVol = value;
                NotifyOfPropertyChange(() => AcmAxis_SetAxisJogVol
);
            }
        }
        private string acmAxis_GetAxisMotion;
        public string AcmAxis_GetAxisMotion
        {
            get
            {

                return acmAxis_GetAxisMotion;
            }
            set
            {
                acmAxis_GetAxisMotion = value;
                NotifyOfPropertyChange(() => AcmAxis_GetAxisMotion
);
            }
        }
        private ObservableCollection<string> availableAxis = new ObservableCollection<string>();
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
        private ObservableCollection<string> availableDevs = new ObservableCollection<string>();
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
        private List<string> availableDevs1 = new List<string>();
        public List<string> AvailableDevs1
        {
            get
            {

                return availableDevs1;
            }
            set
            {
                availableDevs1 = value;
                NotifyOfPropertyChange(() => AvailableDevs1
);
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
                    Axis_States = " STA_AxDisable";
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
                return_State = value;
                if (return_State != 0)
                {
                    Application.Current.Dispatcher.BeginInvoke((System.Action)(() =>
               {
                   NoticeX.Show("Success",$"{AxisSlect}轴发生错误；错误代码{return_State}", Panuon.UI.Silver.MessageBoxIcon.Success,10000);
               }));
            }
                NotifyOfPropertyChange(() => Return_State
);
            }
        }
        CancellationTokenSource _CancellationToken;
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
                            
                            double GetActPos = 0;
                            AvailableDevs.Clear();
                            AvailableAxis.Clear();
                             pCIE_1203Initialize = new Lib.PCIE_1203Initialize();
                            for (int i = 0; i < pCIE_1203Initialize.mDeviceCount; i++)
                            {
                                AvailableDevs.Add( $"卡号{i}");
                                for (int j=0;j< pCIE_1203Initialize.m_AxisCount[i];j++)
                                {
                                    AvailableAxis.Add($"轴号{j}");
                                }
                            }
                            AcmAxis_GetAvailableDevs = pCIE_1203Initialize.CurAvailableDevs[0].DeviceName.ToString();
                            pCIE_1203Initialize.Axis_State(0,ref GetActPos);
                            AcmAxis_GetActPos= GetActPos.ToString();
                            Task.Run(() =>
                            {
                                while (true )
                                {
                                    Return_State = pCIE_1203Initialize.Axis_State(AxisSlect, ref act_Pos, ref cmd_Pos, ref axis_State, ref motion_State);
                                    
                                    Thread.Sleep(500);
                                }
                            }, _CancellationToken.Token).ContinueWith(t=>{
                                if (t.IsCompleted)
                                {
                                    NoticeX.Show("Task Completed", "Success", Panuon.UI.Silver.MessageBoxIcon.Error, 1000);
                                }
                                if (t.IsFaulted)
                                {
                                    NoticeX.Show("Task Faulted", "Success", Panuon.UI.Silver.MessageBoxIcon.Error, 1000);
                                }
                                if (t.IsCanceled)
                                {
                                    NoticeX.Show("Task Canceled", "Success", Panuon.UI.Silver.MessageBoxIcon.Error, 1000);
                                }
                            });
                          
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
                            _CancellationToken.Cancel();
                            if (_CancellationToken.IsCancellationRequested)
                            {NoticeX.Show("Task Canceledtest", "Success", Panuon.UI.Silver.MessageBoxIcon.Error, 1000); }
                            
                        }));
                return adlinkservon;
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
                            try {
                            Return_State= pCIE_1203Initialize.Jog(AxisSlect, true, false, double.Parse(AcmAxis_SetAxisAcc), double.Parse(AcmAxis_SetAxisAcc), double.Parse(AcmAxis_SetAxisJogVol));
                            }
                            catch (Exception k)
                            { NoticeX.Show(k.ToString(), "Wran", Panuon.UI.Silver.MessageBoxIcon.Error, 10000); }
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
                            try {
                                Return_State = pCIE_1203Initialize.Jog(AxisSlect, false, true, double.Parse(AcmAxis_SetAxisAcc), double.Parse(AcmAxis_SetAxisAcc), double.Parse(AcmAxis_SetAxisJogVol));
                            }
                            catch (Exception k )
                            { NoticeX.Show(k.ToString(), "Wran", Panuon.UI.Silver.MessageBoxIcon.Error, 10000); }
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
                            Return_State= pCIE_1203Initialize.JogStop(AxisSlect);  

                        }));
                return jogStop;
            }
        }
        #region DEV下拉选择
        private Command.NewCommand devSlectCbxDropDownOpened;
        public Command.NewCommand DevSlectCbxDropDownOpened
        {

            get
            {
                if (devSlectCbxDropDownOpened == null)
                    devSlectCbxDropDownOpened = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                           

                        }));
                return devSlectCbxDropDownOpened;
            }
        }
        private Command.NewCommand devSlectCbxDropDownClosed;
        public Command.NewCommand DevSlectCbxDropDownClosed
        {

            get
            {
                if (devSlectCbxDropDownClosed == null)
                    devSlectCbxDropDownClosed = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            ;
                            
                        }));
                return devSlectCbxDropDownClosed;
            }
        }
        private Command.NewCommand devSlectCbxSelectionChanged;
        public Command.NewCommand DevSlectCbxSelectionChanged
        {

            get
            {
                if (devSlectCbxSelectionChanged == null)
                    devSlectCbxSelectionChanged = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                             AcmAxis_GetAvailableDevs = pCIE_1203Initialize.CurAvailableDevs[DevsSlect].DeviceName.ToString();
                            NoticeX.Show($"Task Canceled{DevsSlect}", "Success", Panuon.UI.Silver.MessageBoxIcon.Error, 1000);
                        }));
                return devSlectCbxSelectionChanged;
            }
        }
        #endregion
        #region AXIS下拉选择
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
                            try
                            {
                                double GetActPos = 0;
                                Return_State= pCIE_1203Initialize.Axis_State(AxisSlect, ref GetActPos);
                                AcmAxis_GetActPos = GetActPos.ToString();
                            } catch (Exception g)
                            {; }
                           
                        }));
                return axisSlectCbxSelectionChanged;
            }
        }
        #endregion
        #endregion
    }
}
