using System.IO.Ports;

// Пример запроса на оплату
//{"method": "pay","orderId": "bdf75773-4a63-4439-98ce-29602ebfa4100","amount": "100000"}

class Program
{
    static SerialPort serialPort;

    static void Main(string[] args)
    {
        while (true)
        {
            try
            {
                // Инициализация порта
                Console.Write("Enter COM port number (e.g., COM7):");
                string portName = Console.ReadLine();

                serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);

                // Подписка на событие получения данных
                serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                // Открываем порт
                serialPort.Open();
                Console.WriteLine($"Connected to {portName}");
                Console.WriteLine("Enter a command:");

                while (true)
                {
                    // Ввод сообщения
                    string message = Console.ReadLine();

                    if (message.ToLower() == "exit")
                    {
                        Console.WriteLine("Exiting program...");
                        serialPort.Close();
                        return;
                    }

                    if (message.ToLower() == "reconnect")
                    {
                        Console.WriteLine("Reconnecting...");
                        serialPort.Close();
                        break; // Выход из внутреннего цикла для переподключения
                    }

                    // Отправка сообщения в порт
                    serialPort.WriteLine(message);
                    Console.WriteLine("Command sent: " + message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.Close();
                    Console.WriteLine("Port closed");
                }
            }
        }
    }


    static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting(); // Чтение данных из порта
            Console.WriteLine("Received: " + data);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while reading data: " + ex.Message);
        }
    }
}
