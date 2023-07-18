using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient
{
public partial class MainWindow : Window
{
private TcpClient client;

public MainWindow()
{
InitializeComponent();
}

private async void ConnectButton_Click(object sender, RoutedEventArgs e)
{
try
{
client = new TcpClient();
await client.ConnectAsync("localhost", 8888);

Log("Подключено к серверу.");

// Обработка входящих сообщений от сервера
_ = Task.Run(ReceiveMessagesAsync);
}
catch (Exception ex)
{
Log("Ошибка при подключении к серверу: " + ex.Message);
}
}

private async Task ReceiveMessagesAsync()
{
try
{
NetworkStream stream = client.GetStream();
byte[] buffer = new byte[4096];

while (true)
{
int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

if (bytesRead == 0)
{
// Сервер отключился
break;
}

string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
Log("Получено сообщение от сервера: " + message);
}
}
catch (Exception ex)
{
Log("Ошибка при получении сообщений: " + ex.Message);
}
}

private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
{
try
{
string message = MessageTextBox.Text;

if (!string.IsNullOrWhiteSpace(message))
{
byte[] buffer = Encoding.UTF8.GetBytes(message + Environment.NewLine);
await client.GetStream().WriteAsync(buffer, 0, buffer.Length);

Log("Отправлено сообщение серверу: " + message);

MessageTextBox.Clear();
}
}
catch (Exception ex)
{
Log("Ошибка при отправке сообщения: " + ex.Message);
}
}

private void Log(string message)
{
Dispatcher.Invoke(() =>
    {
    LogTextBox.AppendText(message + Environment.NewLine);
    LogTextBox.ScrollToEnd();
    });
    }
    }
    }
