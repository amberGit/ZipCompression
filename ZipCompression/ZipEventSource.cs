using System;

namespace ZipCompression
{
    /// <summary>
    /// 自定义事件类
    /// </summary>
    public class ZipEventSource
    {

        public enum ZipEventValue
        {
            COMPRESS,
            DECOMPRESS,
            UPDATE_BAR
        }
        /// <summary>
        /// 定义事件参数
        /// </summary>
        public class ZipEventArgs : EventArgs
        {
            public readonly ZipEventValue EventValue;
            public ZipEventArgs(ZipEventValue eventValue)
            {
                EventValue = eventValue;
            }
        }

        /// <summary>
        /// 声明事件对象
        /// </summary>
        public event ZipEventHandler ZipEvent;

        /// <summary>
        /// 事件触发方法
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnZipEvent(ZipEventArgs e)
        {
            if (ZipEvent != null)
                ZipEvent(this, e);
        }

        /// <summary>
        /// 引发事件
        /// </summary>
        /// <param name="eventValue"></param>
        public void RaiseEvent(ZipEventValue eventValue)
        {
            ZipEventArgs e = new ZipEventArgs(eventValue);
            OnZipEvent(e);
        }

        /// <summary>
        /// 定义事件处理的委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void ZipEventHandler(object sender, ZipEventArgs e);

    }
}
