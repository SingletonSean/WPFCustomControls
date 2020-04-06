using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AnalogClockControl.CustomControls
{
    [TemplateVisualState(Name = "Day", GroupName = "TimeStates")]
    [TemplateVisualState(Name = "Night", GroupName = "TimeStates")]
    [TemplateVisualState(Name = "Christmas", GroupName = "TimeStates")]
    public class Clock : Control
    {
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(DateTime), typeof(Clock), 
            new PropertyMetadata(DateTime.Now, TimePropertyChanged, TimeCoerceValue));

        private static object TimeCoerceValue(DependencyObject d, object baseValue)
        {
            if(baseValue is DateTime)
            {
                DateTime time = (DateTime)baseValue;

                if(time.Second % 2 == 1)
                {
                    baseValue = time.AddSeconds(1);
                }
            }

            return baseValue;
        }

        private static bool TimeValidateValue(object value)
        {
            if(value is DateTime)
            {
                DateTime time = (DateTime)value;

                if(time.Second % 2 == 1)
                {
                    return false;
                }
            }

            return true;
        }

        private static void TimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is Clock)
            {
                Clock clock = d as Clock;
                clock.RaiseEvent(new RoutedPropertyChangedEventArgs<DateTime>((DateTime)e.OldValue, (DateTime)e.NewValue, TimeChangedEvent));
            }
        }

        public static readonly DependencyProperty ShowSecondsProperty = DependencyProperty.Register("ShowSeconds", typeof(bool), typeof(Clock), new PropertyMetadata(true));
        public static readonly RoutedEvent TimeChangedEvent = EventManager.RegisterRoutedEvent("TimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(Clock));

        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public bool ShowSeconds
        {
            get { return (bool)GetValue(ShowSecondsProperty); }
            set { SetValue(ShowSecondsProperty, value); }
        }

        public event RoutedPropertyChangedEventHandler<DateTime> TimeChanged
        {
            add
            {
                AddHandler(TimeChangedEvent, value);
            }
            remove
            {
                RemoveHandler(TimeChangedEvent, value);
            }
        }

        public override void OnApplyTemplate()
        {
            OnTimeChanged(DateTime.Now);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += (s, e) => OnTimeChanged(DateTime.Now);
            timer.Start();

            base.OnApplyTemplate();
        }

        protected virtual void OnTimeChanged(DateTime newTime)
        {
            UpdateTimeState(newTime);
            Time = newTime;
        }

        private void UpdateTimeState(DateTime time)
        {
            if(time.Day == 25 && time.Month == 12)
            {
                VisualStateManager.GoToState(this, "Christmas", false);
            }
            else
            {
                if(time.Hour > 6 && time.Hour < 18)
                {
                    VisualStateManager.GoToState(this, "Day", false);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Night", false);
                }
            }
        }
    }
}
