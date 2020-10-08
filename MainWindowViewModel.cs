using Caliburn.Micro;
using Demo_core.ExtendViews;
using Demo_core.Models;
using Microsoft.EntityFrameworkCore;
using Panuon.UI.Silver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Demo_core
{
    [Export(typeof(IShell))]
    class MainWindowViewModel : Caliburn.Micro.Screen, INotifyPropertyChanged
    {
        private readonly IEventAggregator _eventAggregator;
        #region
        [ImportingConstructor]
        public MainWindowViewModel(IEventAggregator e)
        {
            _eventAggregator = e;
            _eventAggregator.SubscribeOnPublishedThread(this);
        }
        #endregion
        #region Fild FlyOut
        private bool loginFlyOut_Isopen;
        public bool LoginFlyOut_Isopen
        {  get
            {   return loginFlyOut_Isopen;
            }
            set
            {  loginFlyOut_Isopen = value;
                NotifyOfPropertyChange(() => LoginFlyOut_Isopen );
            }}
        #endregion
        #region Fild PASSWORE
        private string login_Name;
        public string Login_Name
        {
            get
            {
                return login_Name;
            }
            set
            {
                login_Name = value;
                NotifyOfPropertyChange(() => Login_Name);
            }
        }
        private string pass_Word;
        public string Pass_Word
        {
            get
            {
                return pass_Word;
            }
            set
            {
                pass_Word = value;
                NotifyOfPropertyChange(() => Pass_Word);
            }
        }
        #endregion
        #region LiftControlCommand
        private Window SystemInfoWindow;
        private Command.NewCommand systemInfo;
        public Command.NewCommand SystemInfo
        {

            get
            {
                if (systemInfo == null)
                    systemInfo = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke((System.Action)(() =>
                            {
                                 if (SystemInfoWindow == null)
                            { SystemInfoWindow = new SystemInformationView(); }
                          
                                SystemInfoWindow.Show();
                          
                            }));
                            //Task.Run(() => { 
                           
                            //}).ContinueWith(t => {
                            //    if (t.IsCompleted)
                            //    {
                            //        NoticeX.Show("Task Completed", "Success", Panuon.UI.Silver.MessageBoxIcon.Success, 1000);
                            //    }
                            //    if (t.IsFaulted)
                            //    {
                            //        NoticeX.Show("Task Faulted", "Success", Panuon.UI.Silver.MessageBoxIcon.Error, 1000);
                            //    }
                            //    if (t.IsCanceled)
                            //    {
                            //        NoticeX.Show("Task Canceled", "Success", Panuon.UI.Silver.MessageBoxIcon.Error, 1000);
                            //    }
                            //});
                            

                        }));
                return systemInfo;
            }
        }
        private Command.NewCommand systemLogin;
        public Command.NewCommand SystemLogin
        {

            get
            {
                if (systemLogin == null)
                    systemLogin = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            LoginFlyOut_Isopen = !LoginFlyOut_Isopen;  }));
                return systemLogin;
            }
        }
        #endregion
        #region LoginFlyOutCommand
        private string password;
        public void MyGetPassword(object Sender , string e)
        {
            if (Sender is PasswordBox passwordBox)
            {
                Pass_Word = passwordBox.Password;
            }
        }
        private Command.NewCommand getPassword;
        public Command.NewCommand GetPassword
        {

            get
            {
                if (getPassword == null)
                    getPassword = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            if (e is PasswordBox password)
                            {
                                Pass_Word = password.Password;
                            }
                        }));
                return getPassword;
            }
        }
        private Command.NewCommand cheakIn;
        public Command.NewCommand CheakIn
        {

            get
            {
                if (cheakIn == null)
                    cheakIn = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        {
                            try
                            {
                                if (Login_Name != null && Pass_Word != null)
                                {
                                    using (_12Context context = new _12Context())
                                    {
                                        //var Returndata = context.Products1.Where(i => i.UserName == Login_Name && i.UserPass == Pass_Word).Select(I => I.Id).ToList();
                                        int id = 0;
                                        var Returndata1 = from i in context.Products1
                                                          where i.UserName == Login_Name && i.UserPass == Pass_Word
                                                          select i;
                                        foreach (var item in Returndata1)
                                        {
                                            id = item.Id;
                                          
                                       
                                    } 
                                        if (id != 0)
                                        {
                                            NoticeX.Show("登陆成功", "Success", Panuon.UI.Silver.MessageBoxIcon.Success, 3000);
                                        }
                                        else
                                        {
                                            Login_Name = null;
                                            NoticeX.Show("登陆失败", "Error", Panuon.UI.Silver.MessageBoxIcon.Error, 3000); }
                                        }
                                }
                                else
                                { NoticeX.Show("用户名或密码格式错误检查后重新输入", "Error", Panuon.UI.Silver.MessageBoxIcon.Error, 3000); }
                            }
                            catch (Exception ex)
                            {
                                System.Windows.MessageBox.Show(ex.ToString());
                            }
                             }));
                return cheakIn;
            }
        }
        private Command.NewCommand cheakOut;
        public Command.NewCommand CheakOut
        {

            get
            {
                if (cheakOut == null)
                    cheakOut = new Command.NewCommand(
                        new Action<object>(
                        e =>
                        { 
                            Pass_Word = null;
                              Login_Name = null;
                            NoticeX.Show("退出登陆成功", "Success", Panuon.UI.Silver.MessageBoxIcon.Success, 3000);
                        }));
                return cheakOut;
            }
        }
        #endregion
    }
}
