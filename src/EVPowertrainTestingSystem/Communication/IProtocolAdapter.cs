using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVPowertrainTestingSystem.Communication
{
    /// <summary>
    /// 协议适配器接口 - 定义所有通信协议的统一接口
    /// </summary>
    public interface IProtocolAdapter
    {
        /// <summary>
        /// 协议名称
        /// </summary>
        string ProtocolName { get; }

        /// <summary>
        /// 是否已连接
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 连接设备
        /// </summary>
        Task<bool> ConnectAsync(string connectionString);

        /// <summary>
        /// 断开连接
        /// </summary>
        Task<bool> DisconnectAsync();

        /// <summary>
        /// 读取数据
        /// </summary>
        Task<byte[]> ReadDataAsync(string address, int length);

        /// <summary>
        /// 写入数据
        /// </summary>
        Task<bool> WriteDataAsync(string address, byte[] data);

        /// <summary>
        /// 发送命令
        /// </summary>
        Task<byte[]> SendCommandAsync(byte[] command);

        /// <summary>
        /// 数据接收事件
        /// </summary>
        event EventHandler<DataReceivedEventArgs> DataReceived;

        /// <summary>
        /// 错误事件
        /// </summary>
        event EventHandler<ErrorEventArgs> ErrorOccurred;
    }

    /// <summary>
    /// 数据接收事件参数
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
        public DateTime Timestamp { get; set; }
        public string SourceDevice { get; set; }
    }

    /// <summary>
    /// 错误事件参数
    /// </summary>
    public class ErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public DateTime Timestamp { get; set; }
    }
}