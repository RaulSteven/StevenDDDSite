namespace Steven.Domain.Enums
{
    public enum ThumMode
    {
        //根据缩略图的尺寸截取原图
        Crop,
        //按比例缩放，自动调整尺寸
        Fit,
        //按比例缩放，保持尺寸，不足部分填白
        Pad
    }
}
