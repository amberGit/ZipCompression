using System;

namespace ZipCompression
{
    /// <summary>
    /// 自定义事件类
    /// </summary>
    public class ZipEventSource
    {

        public ZipEventSource()
        {
        }

        public enum ZipEventValue
        {
            COMPRESS,
            DECOMPRESS
        }
        /// <summary>
        /// 定义事件参数
        /// </summary>
        public class ZipEventArgs : EventArgs
        {
            public readonly ZipEventValue EventValue;
            public readonly string[] files;
            public readonly string filePath;
            public ZipEventArgs(ZipEventValue eventValue, string[] files, string filePath)
            {
                EventValue = eventValue;
                this.files = files;
                this.filePath = filePath;
            }
        }

        /// <summary>
        /// 压缩
        /// </summary>
        public event EventHandler<ZipEventArgs> Compress;

        public event EventHandler<ZipEventArgs> DeCompress;

        /// <summary>
        /// 事件触发方法
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnZipEvent(object sender, ZipEventArgs e)
        {
            if (e.EventValue == ZipEventValue.COMPRESS)
                Compress(sender, e);
            else if (e.EventValue == ZipEventValue.DECOMPRESS)
                DeCompress(sender, e);
        }

        

    }
}
