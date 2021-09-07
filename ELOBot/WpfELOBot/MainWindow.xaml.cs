using System;
using System.Collections.Generic;
using System.IO;
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
using Discord;
using Discord.WebSocket;

namespace WpfELOBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DiscordSocketClient _client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            await MainAsync();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if (_client != null)
            {
                _client.LogoutAsync();
                _client.StopAsync();
            }
        }

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandler;

            var token = File.ReadAllText("token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        public Task CommandHandler(SocketMessage message)
        {
            string command = "";

            //Als bericht niet begint met vraagteken
            if (!message.Content.StartsWith("!"))
            {
                //Laat weten dat task gedaan is (doe niets)
                return Task.CompletedTask;
            }
            //Als auteur van bericht een bot is
            if(message.Author.IsBot)
            {
                //Laat weten dat task gedaan is (reageer niet op andere bots)
                return Task.CompletedTask;
            }

            //Haal vraagteken af van de message en zet het in variabele command
            command = message.Content.Substring(1, message.Content.Length - 1);

            //Als command gelijk is aan test
            if(command.Equals("test"))
            {
                //Stuur bericht
                message.Channel.SendMessageAsync("It works!");
            }

            //Geef terug dat task klaar is
            return Task.CompletedTask;
        }
    }
}
