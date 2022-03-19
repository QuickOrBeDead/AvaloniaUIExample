namespace AvaloniaUIApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Reactive.Concurrency;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Threading.Tasks;

    using Avalonia.Threading;

    using ReactiveUI;

    public class MainWindowViewModel : ViewModelBase
    {
        public string Title => "Weather Info";

        public string Temperature { get; set; } = string.Empty;

        public string Conditions { get; set; } = string.Empty;

        public MainWindowViewModel()
        {
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Timer_UpdateWeatherInfo;
            dispatcherTimer.Interval = new TimeSpan(0, 30, 0);
            dispatcherTimer.Start();

            RxApp.MainThreadScheduler.Schedule(UpdateWeatherInfo);
        }

        private void Timer_UpdateWeatherInfo(object? sender, EventArgs e)
        {
            UpdateWeatherInfo();
        }

        public void UpdateWeatherInfo()
        {
            using var client = new HttpClient();
            var result = client.GetFromJsonAsync<JsonNode>("https://wttr.in/?format=j1").Result;

            if (result != null)
            {
                var conditionInfo = result["current_condition"]![0];
                Temperature = conditionInfo!["temp_C"]!.ToString();
                Conditions = conditionInfo["weatherDesc"]![0]!["value"]!.ToString();
            }
        }
    }
}
