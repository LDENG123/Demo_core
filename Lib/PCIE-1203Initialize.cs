using System;
using System.Collections.Generic;
using System.Text;
using Panuon.UI.Silver;
using Advantech.Motion;
using Caliburn.Micro;
using System.Windows;
using System.Threading;

namespace Demo_core.Lib
{
    class PCIE_1203Initialize
    {
        /// <summary>
        /// 初始化板卡开启板卡
        /// </summary>
       public DEV_LIST[] CurAvailableDevs = new DEV_LIST[64];//指针指向返回可用设备信息列表
        public IntPtr[] DeviceHandle = new IntPtr[16];//设备卡句柄数据
        public IntPtr[] AxisHandle = new IntPtr[64];//设备卡下面的轴句柄数据 
        public uint[] m_AxisCount =new uint[16];//获得轴下面的所有轴数
       public uint mDeviceCount = 0;//返回设备读取的轴数
        public PCIE_1203Initialize()
        {
            int Retry_OpenDEV = 0;
            bool bRescan = false;
            var iResult = Motion.mAcm_GetAvailableDevs(CurAvailableDevs, Motion.MAX_DEVICES, ref mDeviceCount);
            if (iResult == 0 && mDeviceCount > 0)
            {
                for (int i = 0; i < mDeviceCount; i++)//打开所有卡和所有轴
                {
                    do
                    {
                        var iResult0 = Motion.mAcm_DevOpen(CurAvailableDevs[i].DeviceNum, ref DeviceHandle[i]);//获得设备卡的句柄
                        if (iResult0 != 0)
                        {
                            Retry_OpenDEV++;
                            bRescan = true;
                            if (Retry_OpenDEV > 10)
                            {
                                bRescan = false;
                                 NoticeX.Show("Error Code"+iResult0.ToString()+"卡号"+i.ToString() +"开卡失败", Panuon.UI.Silver.MessageBoxIcon.Error);
                            }
                            Thread.Sleep(1000);                        }
                        else
                            bRescan = false;

                    } while (bRescan);
                   
                    var Result_A = Motion.mAcm_GetU32Property(DeviceHandle[i], (uint)PropertyID.FT_DevAxesCount, ref m_AxisCount[i]);
                    if (Result_A == 0)//打开设备轴下所有的轴
                    { 
                        for (int j= 0; j < m_AxisCount[i];j++)
                       {
                            Motion.mAcm_AxOpen(DeviceHandle[i],(ushort)j, ref AxisHandle[j]);

                        }
                    
                    }
                } 
            }

            else
                NoticeX.Show("Success","轴卡初始化失败", Panuon.UI.Silver.MessageBoxIcon.Error,1000);
        }
        /// <summary>
        /// 轴使能函数
        /// </summary>
        /// <param name="AxisIndex">轴索引</param>
        /// <param name="Enable">轴使能</param>
        /// <returns></returns>
        public uint SeverOn(int AxisIndex,bool Enable)
        {
            uint Ret= 0;
            if (Enable)

            { Ret = Motion.mAcm_AxSetSvOn(AxisHandle[AxisIndex], 1); }
            else
              Ret=  Motion.mAcm_AxSetSvOn(AxisHandle[AxisIndex], 0);
            return Ret;
        }
        /// <summary>
        /// 点动函数
        /// </summary>
        /// <param name="AxisIndex">轴数索引</param>
        /// <param name="Positive">正向点动</param>
        /// <param name="Negtive">反向点动</param>
        /// <param name="Acc">点动加速度</param>
        /// <param name="dec">点动减速度</param>
        /// <param name="JogSpeed">点动速度</param>
        ushort Diretor = 0;
        public uint Jog(int AxisIndex, bool Positive, bool Negtive,double Acc,double dec,double JogSpeed)
        { if (Positive)
                Diretor = 0;
            else if (Negtive)
                Diretor = 1;      
            var ret=Motion.mAcm_SetF64Property(AxisHandle[AxisIndex],(uint)PropertyID.CFG_AxJogVelHigh,JogSpeed); //运行速度
             ret = Motion.mAcm_SetF64Property(AxisHandle[AxisIndex], (uint)PropertyID.CFG_AxJogAcc, Acc);//加速度
            ret= Motion.mAcm_SetF64Property(AxisHandle[AxisIndex], (uint)PropertyID.CFG_AxJogDec, dec);//加速度
            ret = Motion.mAcm_AxJog(AxisHandle[AxisIndex], Diretor);
            return ret;
        }
        /// <summary>
        /// 轴停止函数
        /// </summary>
        /// <param name="AxisIndex"></param>
        public uint JogStop(int AxisIndex)
        {
          var Ret=  Motion.mAcm_AxStopDec(AxisHandle[AxisIndex]);
            return Ret;
        }
        /// <summary>
        /// 轴状态读取
        /// </summary>
        /// <param name="AxisIndex"> 轴索引</param>
        /// <param name="AxisActPos">轴实际位置</param>
        /// <param name="AxisCmdPos">轴命令位置</param>
        /// <param name="AxisState">轴状态</param>
        /// <param name="MotionStatus">轴运动状态</param>
        /// <param name="MotionIO">轴IO状态</param>
        /// <returns></returns>
        public uint Axis_State(int AxisIndex, ref double AxisActPos, ref double AxisCmdPos, ref ushort AxisState, ref uint MotionStatus,ref uint MotionIO)
        {
            var Ret = Motion.mAcm_AxGetActualPosition(AxisHandle[AxisIndex], ref AxisActPos);
            Ret = Motion.mAcm_AxGetCmdPosition(AxisHandle[AxisIndex], ref AxisCmdPos);
            Ret = Motion.mAcm_AxGetState(AxisHandle[AxisIndex], ref AxisState);
            Ret = Motion.mAcm_AxGetMotionStatus(AxisHandle[AxisIndex], ref MotionStatus);
            Ret = Motion.mAcm_AxGetMotionIO(AxisHandle[AxisIndex], ref MotionIO);
            return Ret;
        }
        /// <summary>
        /// 轴状态读取
        /// </summary>
        /// <param name="AxisIndex"> 轴索引</param>
        /// <param name="AxisActPos">轴实际位置</param>
        /// <param name="AxisCmdPos">轴命令位置</param>
        /// <param name="AxisState">轴状态</param>
        /// <param name="MotionStatus">轴运动状态</param>
        /// <returns></returns>
        public uint Axis_State(int AxisIndex,ref double AxisActPos,ref double AxisCmdPos,ref ushort AxisState,ref uint MotionStatus)
        {
         var Ret= Motion.mAcm_AxGetActualPosition(AxisHandle[AxisIndex], ref AxisActPos);
            Ret = Motion.mAcm_AxGetCmdPosition(AxisHandle[AxisIndex], ref AxisCmdPos);
            Ret = Motion.mAcm_AxGetState(AxisHandle[AxisIndex], ref AxisState);
            Ret = Motion.mAcm_AxGetMotionStatus(AxisHandle[AxisIndex],ref MotionStatus);
            return Ret;
                }
        /// <summary>
        /// 轴状态读取重载1
        /// </summary>
        /// <param name="AxisIndex">轴索引</param>
        /// <param name="AxisActPos">轴实际位置</param>
        /// <returns></returns>
        public uint Axis_State(int AxisIndex, ref double AxisActPos)
        {
            var Ret = Motion.mAcm_AxGetActualPosition(AxisHandle[AxisIndex], ref AxisActPos);
            return Ret;
        }
        public uint AxisHome(int AxisIndex)
        {
            var Ret = Motion.mAcm_AxHome(AxisHandle[AxisIndex], 13, 1);
            return Ret;
        }
    }
   
    
}
