using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ZipCompression
{
    class ZipEventListener: DependencyObject
    {
        private ProgressBar PBar = new ProgressBar();
        private MainWindow window;
        private TextBlock textBlock = new TextBlock();
        private DateTime startCompressionTime;
        public static readonly DependencyProperty progressProperty = DependencyProperty.Register("progress", typeof(int), typeof(ZipEventListener));

        public enum ZipProgressBarEventValue
        {
            BEGIN,
            PROGRESSING,
            FINISHED
        }

        public class ZipProgressBarEventArgs : EventArgs
        {
            public int Progress;
            public ZipProgressBarEventValue EventValue;
            public ZipProgressBarEventArgs(ZipProgressBarEventValue EventValue, int Progress)
            {
                this.Progress = Progress;
                this.EventValue = EventValue;
            }
        }


        public ZipEventListener (MainWindow window)
        {
            this.window = window;
            textBlock.Text = "就绪";
            window.SBar.Items.Add(textBlock);
        }

        public int progress
        {
            get
            {
                return (int)window.Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    (DispatcherOperationCallback)delegate { return getProgressPercent(progressProperty); },
                    progressProperty);
            }
            protected set
            {
                window.Dispatcher.BeginInvoke(DispatcherPriority.Background, 
                    (SendOrPostCallback) delegate { updatePrgressBar(progressProperty, value); }, value);
            }
        }

        
        /// <summary>
        /// 创建进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void  HandlePrgressBar(object sender, EventArgs e)
        {
            ZipProgressBarEventArgs eventArgs = e as ZipProgressBarEventArgs;

            if (eventArgs.EventValue == ZipProgressBarEventValue.BEGIN)
            {
                createPrgressBar();
                startCompressionTime = DateTime.Now;
            }
            else if (eventArgs.EventValue == ZipProgressBarEventValue.PROGRESSING)
            {
                progress = eventArgs.Progress;
                textBlock.Text = "正在压缩 " + progress + "%";
                //progress = (int)Math.Ceiling(zipUtil.compressedSize * 1.0d / zipUtil.totalSize * 100);
            } 
            else
            {
                window.SBar.Items.Remove(PBar);
                textBlock.Text = "压缩完成,用时" + (int)Math.Ceiling((DateTime.Now - startCompressionTime).TotalSeconds) + "秒";
            }
        }

        private void createPrgressBar()
        {
            PBar.IsIndeterminate = false;
            PBar.Maximum = 100;
            PBar.Value =  progress;
            PBar.Orientation = Orientation.Horizontal;
            PBar.Width = 200;
            PBar.Height = 15;
            PBar.SetBinding(progressProperty, "progress");
            window.SBar.Items.Add(PBar);
        }


        private void updatePrgressBar(DependencyProperty dp, double value)
        {
            PBar.Value = value;
        }
        private int getProgressPercent(DependencyProperty dp)
        {
            return (int) PBar.Value;
        }
    }
}
