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
        private ZipEventSource zipEventIns;
        private MainWindow window;
        private TextBlock textBlock = new TextBlock();
        private DateTime startCompressionTime;
        public static readonly DependencyProperty progressProperty = DependencyProperty.Register("progress", typeof(int), typeof(ZipEventListener));
        public ZipEventListener (MainWindow window, ZipEventSource zipEventIns)
        {
            this.zipEventIns = zipEventIns;
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
        /// 监听事件
        /// </summary>
        public void On()
        {
            zipEventIns.ZipEvent += handlePrgressBar;
        }
        /// <summary>
        /// 释放监听事件
        /// </summary>
        public void Off()
        {
            zipEventIns.ZipEvent -= handlePrgressBar;
        }

        /// <summary>
        /// 创建进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void  handlePrgressBar(object sender, EventArgs e)
        {
            ZipEventSource.ZipEventArgs eventArgs = e as ZipEventSource.ZipEventArgs;
            
            if (progress == 100)
            {
                window.SBar.Items.Remove(PBar);
                textBlock.Text = "压缩完成,用时" + (int)Math.Ceiling((DateTime.Now - startCompressionTime).TotalSeconds) + "秒";
                
            }
            else
            {
                textBlock.Text = "正在压缩 " + progress + "%";
            }
            if (eventArgs.EventValue == ZipEventSource.ZipEventValue.COMPRESS)
            {
                createPrgressBar();
                startCompressionTime = DateTime.Now;
            }
           else if (eventArgs.EventValue == ZipEventSource.ZipEventValue.UPDATE_BAR)
            {
                ZipUtil zipUtil = sender as ZipUtil;
                progress = (int) Math.Ceiling(zipUtil.compressedSize * 1.0d / zipUtil.totalSize * 100);
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
