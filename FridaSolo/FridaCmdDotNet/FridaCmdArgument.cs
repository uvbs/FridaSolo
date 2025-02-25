﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenOcean.Common;
using FridaDotNet;

namespace FridaCmd
{
    internal class FridaCmdArgument:BaseCmdArgument,IFridaArguments
    {
        public FridaArguments ControllerArgument = new FridaArguments();
        public bool IsOutputFinalJS = false;
        public string OutputFinalJSPath = "main.js";
        public bool IsOnlyOutputFile = false;
        public string DeviceID { get => ControllerArgument.DeviceID; set => ControllerArgument.DeviceID = value; }
        public string TargetName { get => ControllerArgument.TargetName; set => ControllerArgument.TargetName = value; }
        public string ScriptPath { get => ControllerArgument.ScriptPath; set => ControllerArgument.ScriptPath = value; }
        public bool IsPause { get => ControllerArgument.IsPause; set => ((IFridaArguments)ControllerArgument).IsPause = value; }
        public bool IsNeedSpawn { get => ControllerArgument.IsNeedSpawn; set => ControllerArgument.IsNeedSpawn = value; }

        public FridaCmdArgument(string[] args) : base(args)
        {
            OptionActions.Add("-devices", ListDevices);
            OptionActions.Add("-s", GetLocalDevice);
            OptionActions.Add("-u", GetUsbDevice);
            OptionActions.Add("-f", SetTargetName);
            OptionActions.Add("-l", SetScriptPath);
            OptionActions.Add("--no-pause", SetNoPause);
            OptionActions.Add("-o", SetOutputJS);
            OptionActions.Add("-o-o", SetOnlyOutptFile);
        }

        public override string ToString()
        {
            return ControllerArgument.ToString();
        }

        public virtual int ListDevices(BaseCmdArgument self, string arg, int argIndex)
        {
            SingletonFridaManager.ListDevices();
            return 0;
        }

        public virtual int GetUsbDevice(BaseCmdArgument self, string arg, int argIndex)
        {
            ControllerArgument.DeviceID = SingletonFridaManager.GetDeviceIdByType(2);
            return 0;
        }

        public virtual int GetLocalDevice(BaseCmdArgument self, string arg, int argIndex)
        {
            ControllerArgument.DeviceID = SingletonFridaManager.GetDeviceIdByType(0);
            return 0;
        }

        public virtual int SetTargetName(BaseCmdArgument self, string arg, int argIndex)
        {
            ControllerArgument.TargetName = Arguments[argIndex + 1];
            return 1;
        }

        public virtual int SetScriptPath(BaseCmdArgument self, string arg, int argIndex)
        {
            ControllerArgument.ScriptPath = Arguments[argIndex + 1];
            return 1;
        }

        public virtual int SetNoPause(BaseCmdArgument self, string arg, int argIndex)
        {
            ControllerArgument.IsPause = false;
            return 0;
        }

        public virtual int SetOutputJS(BaseCmdArgument self, string arg, int argIndex)
        {
            IsOutputFinalJS = true;
            var newIndex = argIndex + 1;
            if(newIndex<Arguments.Length)
            {
                string argNext = Arguments[newIndex];
                if (!argNext.StartsWith("-"))
                {
                    OutputFinalJSPath = argNext;
                }
                return 1;
            }
            return 0;
        }

        public virtual int SetOnlyOutptFile(BaseCmdArgument self, string arg, int argIndex)
        {
            IsOnlyOutputFile = true;
            return SetOutputJS(self,arg,argIndex);
        }
    }
}
