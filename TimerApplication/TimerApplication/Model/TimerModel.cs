using System.ComponentModel;

namespace TimerApplication
{
    public class TimerModel : INotifyPropertyChanged
    {
        private string reminderTime;
        private string timerTime;
        private double timerPointerValue;
        private TimeSpan selectedTime;

        public TimerModel()
        {
            this.reminderTime = string.Empty; 
            this.timerTime = string.Empty;  
            this.selectedTime = new TimeSpan(0, 0, 0);
            this.timerPointerValue = 0;
        }

        public TimeSpan SelectedTime
        {
            get
            {
                return selectedTime;
            }
            set
            {
                selectedTime = value;
                this.OnPropertyChanged(nameof(SelectedTime));
            }
        }
        public string ReminderTime
        {
            get
            {
                return reminderTime;
            }
            set
            {
                reminderTime = value;
                this.OnPropertyChanged(nameof(ReminderTime));
            }
        }

        public string TimerTime
        {
            get
            {
                return timerTime;
            }
            set
            {
                timerTime = value;
                this.OnPropertyChanged(nameof(TimerTime));
            }
        }

        public double TimerPointerValue
        {
            get
            {
                return timerPointerValue;
            }
            set
            {
                timerPointerValue = value;
                this.OnPropertyChanged(nameof(TimerPointerValue));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }   
    }
}
