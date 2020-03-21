using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ShaqAttack.Views
{
    /// <summary>
    /// Interaction logic for MainPageView.xaml
    /// </summary>
    public partial class MainPageView : UserControl
    {
        public MainPageView()
        {
            InitializeComponent();


            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 2)
            };
            timer.Tick += ((sender, e) =>
            {
                if (_scrollViewer.VerticalOffset == _scrollViewer.ScrollableHeight)
                {
                    _scrollViewer.ScrollToEnd();
                }
                if (_scrollViewer2.VerticalOffset == _scrollViewer2.ScrollableHeight)
                {
                    _scrollViewer2.ScrollToEnd();
                }
            });
            timer.Start();
        }
    }
}
