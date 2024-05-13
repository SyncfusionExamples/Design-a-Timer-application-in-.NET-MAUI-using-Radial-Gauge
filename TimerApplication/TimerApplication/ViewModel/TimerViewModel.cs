using System.ComponentModel;


namespace TimerApplication
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        public TimerModel TimerModel { get; set; }

        private TimeSpan timerTick;

        private IDispatcherTimer timer;

        private string buttonText;

        private bool gaugeIsVisible, pickerIsVisible , isStartButtonEnabled, isReminderTimeVisible;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Command StartButtonCommand { get; set; }

        public Command CancelButtonCommand { get; set; }

        public Command PauseButtonCommand { get; set; }

        public bool IsStartButtonEnabled
        {
            get
            {
                return isStartButtonEnabled;
            }
            set
            {
                this.isStartButtonEnabled = value;
                this.OnPropertyChanged(nameof(IsStartButtonEnabled));
            }
        }

        public bool IsReminderTimeVisible
        {
            get
            {
                return isReminderTimeVisible;
            }
            set
            {
                this.isReminderTimeVisible = value;
                this.OnPropertyChanged(nameof(IsReminderTimeVisible));
            }
        }

        public bool RadialGaugeIsVisible
        {
            get
            {
                return gaugeIsVisible;
            }
            set
            {
                gaugeIsVisible = value;
                this.OnPropertyChanged(nameof(RadialGaugeIsVisible));
            }
        }
        public bool PickerIsVisible
        {
            get { return pickerIsVisible; }
            set
            {
                pickerIsVisible = value;
                this.OnPropertyChanged(nameof(PickerIsVisible));
            }
        }

        public string ButtonText
        {
            get
            {
                return buttonText;
            }
            set
            {
                buttonText = value;
                this.OnPropertyChanged(nameof(ButtonText));
            }
        }

        public TimerViewModel()
        {
            this.StartButtonCommand = new Command(OnStartButtonClick);
            this.CancelButtonCommand = new Command(OnCancelButtonClick);
            this.PauseButtonCommand = new Command(OnPauseButtonClick);
            this.TimerModel = new TimerModel();
            this.IsReminderTimeVisible = true;
            this.ButtonText = "Pause";
            this.IsStartButtonEnabled = false;
            this.PickerIsVisible = true;
            this.RadialGaugeIsVisible = false;
            timerTick = new TimeSpan();
            var dispatcher = Dispatcher.GetForCurrentThread();

            if (dispatcher != null)
            {
                this.timer = dispatcher.CreateTimer();
                this.timer.Interval = new TimeSpan(0, 0, 1);
                if (this.timer != null)
                {
                    this.timer.Tick += Timer_Tick;
                }
            }
            this.TimerModel.PropertyChanged += TimerModel_PropertyChanged;
        }

        private void TimerModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(TimerModel.SelectedTime))
                return;

            this.IsStartButtonEnabled = TimerModel.SelectedTime > TimeSpan.Zero ? true:false;
        }

        private void OnCancelButtonClick()
        {
            this.TimerModel.SelectedTime = new TimeSpan();
            this.PickerIsVisible = true;
            this.RadialGaugeIsVisible = false;
            this.TimerModel.TimerPointerValue = 100;
            this.TimerModel.TimerTime = string.Empty;
            this.timerTick = new TimeSpan();
            this.StopTimer();
        }

        private void OnPauseButtonClick()
        {
            this.TimerModel.ReminderTime = DateTime.Now.AddSeconds(this.timerTick.TotalSeconds).ToShortTimeString();
            if (this.ButtonText == "Pause")
            {
                this.StopTimer();
                this.ButtonText = "Resume";
                this.IsReminderTimeVisible = false;
            }
            else if (this.ButtonText == "Resume")
            {
                this.StartTimer();
                this.ButtonText = "Pause";
                this.IsReminderTimeVisible = true;
            }
        }

        private void OnStartButtonClick()
        {
            this.TimerModel.TimerPointerValue = 100;
            this.IsReminderTimeVisible = true;
            this.timerTick = this.TimerModel.SelectedTime;
            this.TimerModel.TimerTime = this.TimerModel.SelectedTime.ToString(@"hh\:mm\:ss");
            this.ButtonText = "Pause";

            if (this.TimerModel.SelectedTime > TimeSpan.Zero)
            {
                this.PickerIsVisible = false;
                this.RadialGaugeIsVisible = true;
                this.StartTimer();
            }
        }

        private void StartTimer()
        {
            this.TimerModel.ReminderTime = DateTime.Now.AddSeconds(this.TimerModel.SelectedTime.TotalSeconds).ToShortTimeString();
            if (!this.timer.IsRunning)
                this.timer?.Start();
        }

        private void StopTimer()
        {
            if (this.timer.IsRunning)
                this.timer?.Stop();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (timerTick > TimeSpan.Zero)
            {
                timerTick = timerTick.Subtract(new TimeSpan(0, 0, 1));
                var time = (timerTick / this.TimerModel.SelectedTime) * 100;
                this.TimerModel.TimerPointerValue = time;
                this.TimerModel.TimerTime = timerTick.ToString(@"hh\:mm\:ss");
            }
            else
            {
                this.TimerModel.TimerPointerValue = 0;
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}