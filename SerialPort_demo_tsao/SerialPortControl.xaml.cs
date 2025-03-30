using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO.Ports;
using System.Threading;
using System.Timers;
using System.Windows.Threading;

namespace SerialPort_demo_tsao
{
    /// <summary>
    /// SerialPortControl.xaml 的互動邏輯
    /// </summary>
    public partial class SerialPortControl : UserControl
    {
        private SerialPort serialPort;

        public SerialPortControl()
        {
            InitializeComponent();
            LoadBaudRates();
            LoadSerialPorts();
            LoadStopBits();
            LoadDataBits();
            LoadParity();

        }

        private void LoadBaudRates()
        {
            int[] baudRates = { 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200 };
            cmbBaudRate.ItemsSource = baudRates;
            cmbBaudRate.SelectedItem = 9600;
        }

        private void LoadSerialPorts()
        {
            //string[] ports = {"COM1","COM2","COM3"};
            string[] ports = SerialPort.GetPortNames();
            cmbPorts.ItemsSource = ports;
            if (ports.Length > 0 )
            {
                cmbPorts.SelectedIndex = 0;
            }
        }

        private void LoadDataBits()
        {
            int[] dataBits = { 5, 6, 7, 8 };
            cmbDataBits.ItemsSource = dataBits;
            cmbDataBits.SelectedItem = 8;
        }
        private void LoadStopBits()
        {
            cmbStopBits.ItemsSource = Enum.GetValues(typeof(StopBits));
            cmbStopBits.SelectedItem = StopBits.One;
        }

        private void LoadParity()
        {
            cmbParity.ItemsSource = Enum.GetValues(typeof(Parity));
            cmbParity.SelectedItem = Parity.None;
        }


        private void settingserialport_Click(object sender, RoutedEventArgs e)
        {
            SettingSerialPort();
        }

        private byte CalculateChecksum(byte[] data)
        {
            int sum = data.Sum(b => b);
            byte checksum = (byte)(~sum + 1);
            return checksum;
        }

        private void SettingSerialPort()

        {
            string selectedPort = cmbPorts.SelectedItem.ToString();
            int selectedBaudRate = (int)cmbBaudRate.SelectedItem;
            Parity selectedparity = (Parity)cmbParity.SelectedItem;
            int selectedDataBits = (int)cmbDataBits.SelectedItem;
            StopBits selectedStopBits = (StopBits)cmbStopBits.SelectedItem;

            serialPort = new SerialPort(selectedPort, selectedBaudRate, selectedparity, selectedDataBits, selectedStopBits);
            
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
           openDevice();
            //byte[] command = { 0x80, 0x60, 0x50, 0x40, 0x30 };
            //byte checksum = CalculateChecksum(command);
            //byte[] fullCommand = command.Concat(new byte[] { checksum }).ToArray();
            //serialPort.Write(fullCommand, 0, fullCommand.Length);
        }

        private void openDevice()
        {
            byte[] command = { 0x80, 0x60, 0x50, 0x40, 0x30 };
            byte checksum = CalculateChecksum(command);
            byte[] fullCommand = command.Concat(new byte[] { checksum }).ToArray();
            serialPort.Write(fullCommand, 0, fullCommand.Length);
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            //byte[] command = { 0x80, 0x60, 0x50, 0x40, 0x20 };
            //byte checksum = CalculateChecksum(command);
            //byte[] fullCommand = command.Concat(new byte[] { checksum }).ToArray();
            //serialPort.Write(fullCommand, 0, fullCommand.Length);
        }

        private void closeDevice()
        {
            byte[] command = { 0x80, 0x60, 0x50, 0x40, 0x20 };
            byte checksum = CalculateChecksum(command);
            byte[] fullCommand = command.Concat(new byte[] { checksum }).ToArray();
            serialPort.Write(fullCommand, 0, fullCommand.Length);
        }

        private void serialportOpen_Click(object sender, RoutedEventArgs e)
        {
            serialPort.Open();
        }

        private void serialportClose_Click(object sender, RoutedEventArgs e)
        {
            serialPort.Close();
        }

        private void triggerdevice_Click(object sender, RoutedEventArgs e)
        {
            int interval = int.Parse(txtMilliseconds.Text);
            ReadOnce(interval);
        }

        private Task ReadOnce(int interval)
        {
            
            openDevice();

            return Task.Delay(interval).ContinueWith((i) =>
            {
                closeDevice();
            });
        }


    }
}
