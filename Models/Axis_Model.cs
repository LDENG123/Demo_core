using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_core.Models
{
    class Axis_Model
    {
        #region 轴变量
        public int Axis_Index;
        public double Axis_ActVol;
        public double Axis_ActPos;
        public double Axis_JogSpeed;
        public double Axis_AbsSpeed;
        public double Axis_InchSpeed;
        public double Axis_AbsPos;
        public double Axis_InchPos;
        public double Axis_ACC;
        public double Axis_DEC;
        public int Axis_ErrorWord;
        #endregion
        #region 轴状态
        public bool Axis_Busy;
        public bool Axis_Error;
        public bool Axis_Done;
        public bool Axis_State;
        public bool Axis_Inhome;
        public bool Axis_Homed;
        public bool Axis_POT;
        public bool Axis_NOT;
        #endregion
        #region 轴操作
        public bool Home_OP;
        public bool Abs_MoveOP;
        public bool Inch_MoveOP;
        public bool JogP_OP;
        public bool JogN_OP;
        public bool Error_ClearOP;
        #endregion
    }
}
