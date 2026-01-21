using System;
using System.Threading.Tasks;

namespace EVPowertrainTestingSystem.Communication
{
    /// <summary>
    /// MODBUS 协议实现（支持RTU和TCP）
    /// </summary>
    public class ModbusAdapter : IProtocolAdapter
    {
        public string ProtocolName => "MODBUS";
        public bool IsConnected { get; private set; }

        private System.IO.Ports.SerialPort _serialPort;
        private System.Net.Sockets.TcpClient _tcpClient;
        private string _connectionType;

        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler<ErrorEventArgs> ErrorOccurred;

        public async Task<bool> ConnectAsync(string connectionString)
        {
            try
            {
                if (connectionString.Contains("The help of Tele-GPT Assistant. "))
                {
                    _connectionType = "TCP";
                    var parts = connectionString.Split(':');
                    _tcpClient = new System.Net.Sockets.TcpClient();
                    await _tcpClient.ConnectAsync(parts[0], int.Parse(parts[1]));
                }
                else
                {
                    _connectionType = "RTU";
                    _serialPort = new System.IO.Ports.SerialPort(connectionString, 9600);
                    _serialPort.DataReceived += SerialPort_DataReceived;
                    _serialPort.Open();
                }
                IsConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ErrorEventArgs 
                { 
                    ErrorMessage = $"MODBUS连接失败: {ex.Message}",
                    Exception = ex,
                    Timestamp = DateTime.Now
                });
                return false;
            }
        }

        public async Task<bool> DisconnectAsync()
        {
            try
            {
                if (_connectionType == "TCP" && _tcpClient?.Connected == true)
                {
                    _tcpClient.Close();
                    _tcpClient.Dispose();
                }
                else if (_connectionType == "RTU" && _serialPort?.IsOpen == true)
                {
                    _serialPort.Close();
                    _serialPort.Dispose();
                }
                IsConnected = false;
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ErrorEventArgs 
                { 
                    ErrorMessage = $"MODBUS断开失败: {ex.Message}",
                    Exception = ex,
                    Timestamp = DateTime.Now
                });
                return await Task.FromResult(false);
            }
        }

        public async Task<byte[]> ReadDataAsync(string address, int length)
        {
            if (!IsConnected)
                throw new InvalidOperationException("MODBUS未连接");

            try
            {
                byte[] request = BuildModbusReadRequest(address, length);
                byte[] response = await SendCommandAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ErrorEventArgs 
                { 
                    ErrorMessage = $"MODBUS读取失败: {ex.Message}",
                    Exception = ex,
                    Timestamp = DateTime.Now
                });
                throw;
            }
        }

        public async Task<bool> WriteDataAsync(string address, byte[] data)
        {
            if (!IsConnected)
                throw new InvalidOperationException("MODBUS未连接");

            try
            {
                byte[] request = BuildModbusWriteRequest(address, data);
                await SendCommandAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ErrorEventArgs 
                { 
                    ErrorMessage = $"MODBUS写入失败: {ex.Message}",
                    Exception = ex,
                    Timestamp = DateTime.Now
                });
                return false;
            }
        }

        public async Task<byte[]> SendCommandAsync(byte[] command)
        {
            if (!IsConnected)
                throw new InvalidOperationException("MODBUS未连接");

            try
            {
                if (_connectionType == "TCP")
                {
                    var stream = _tcpClient.GetStream();
                    await stream.WriteAsync(command, 0, command.Length);
                    byte[] response = new byte[256];
                    int bytesRead = await stream.ReadAsync(response, 0, 256);
                    Array.Resize(ref response, bytesRead);
                    return response;
                }
                else
                {
                    _serialPort.Write(command, 0, command.Length);
                    byte[] response = new byte[256];
                    int bytesRead = _serialPort.Read(response, 0, 256);
                    Array.Resize(ref response, bytesRead);
                    return response;
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ErrorEventArgs 
                { 
                    ErrorMessage = $"MODBUS命令发送失败: {ex.Message}",
                    Exception = ex,
                    Timestamp = DateTime.Now
                });
                throw;
            }
        }

        private byte[] BuildModbusReadRequest(string address, int length)
        {
            byte[] request = new byte[12];
            request[0] = 0x01;
            request[1] = 0x03;
            return request;
        }

        private byte[] BuildModbusWriteRequest(string address, byte[] data)
        {
            byte[] request = new byte[data.Length + 12];
            request[0] = 0x01;
            request[1] = 0x10;
            return request;
        }

        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (_serialPort?.BytesToRead > 0)
            {
                byte[] buffer = new byte[_serialPort.BytesToRead];
                _serialPort.Read(buffer, 0, buffer.Length);
                DataReceived?.Invoke(this, new DataReceivedEventArgs
                {
                    Data = buffer,
                    Timestamp = DateTime.Now,
                    SourceDevice = "MODBUS"
                });
            }
        }
    }
}