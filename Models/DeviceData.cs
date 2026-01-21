using System;
using System.Collections.Generic;

namespace EVPowertrainTestingSystem.Models
{
    /// <summary>
    /// 设备数据模型 - 表示从设备采集的实时数据
    /// </summary>
    public class DeviceData
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public string Protocol { get; set; }
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// 电池数据模型
    /// </summary>
    public class BatteryData : DeviceData
    {
        public float StateOfCharge { get; set; } // SOC %
        public float StateOfHealth { get; set; } // SOH %
        public float Voltage { get; set; } // V
        public float Current { get; set; } // A
        public float Temperature { get; set; } // °C
        public float Power { get; set; } // kW
        public int CellCount { get; set; }
        public List<float> CellVoltages { get; set; } = new();
        public List<float> CellTemperatures { get; set; } = new();
    }

    /// <summary>
    /// 电机数据模型
    /// </summary>
    public class MotorData : DeviceData
    {
        public float Speed { get; set; } // rpm
        public float Torque { get; set; } // N·m
        public float Temperature { get; set; } // °C
        public float Power { get; set; } // kW
        public float Efficiency { get; set; } // %
        public float Voltage { get; set; } // V
        public float Current { get; set; } // A
        public string OperatingMode { get; set; } // 工作模式
    }

    /// <summary>
    /// 测功机数据模型
    /// </summary>
    public class DynamometerData : DeviceData
    {
        public float LoadTorque { get; set; } // N·m
        public float Speed { get; set; } // rpm
        public float Power { get; set; } // kW
        public float Temperature { get; set; } // °C
        public float Frequency { get; set; } // Hz
        public string Status { get; set; }
    }

    /// <summary>
    /// 水冷机数据模型
    /// </summary>
    public class WaterCoolerData : DeviceData
    {
        public float InletTemperature { get; set; } // °C
        public float OutletTemperature { get; set; } // °C
        public float FlowRate { get; set; } // L/min
        public float Pressure { get; set; } // bar
        public float Power { get; set; } // kW
        public string Status { get; set; }
    }

    /// <summary>
    /// 功率分析仪数据模型
    /// </summary>
    public class PowerAnalyzerData : DeviceData
    {
        public float Voltage { get; set; } // V
        public float Current { get; set; } // A
        public float ActivePower { get; set; } // kW
        public float ReactivePower { get; set; } // kVar
        public float ApparentPower { get; set; } // kVA
        public float PowerFactor { get; set; }
        public float Frequency { get; set; } // Hz
        public float THDVoltage { get; set; } // %
        public float THDCurrent { get; set; } // %
    }

    /// <summary>
    /// 环境箱数据模型
    /// </summary>
    public class EnvironmentChamberData : DeviceData
    {
        public float Temperature { get; set; } // °C
        public float Humidity { get; set; } // %RH
        public float TemperatureSetpoint { get; set; } // °C
        public float HumiditySetpoint { get; set; } // %RH
        public string Status { get; set; }
        public float CoolingPower { get; set; } // kW
    }
}