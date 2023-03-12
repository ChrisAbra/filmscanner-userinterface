public class PipelineImage
{

    public enum Format
    {
        BMP
    }

    public enum ColorSpace
    {
        Undefined,
        CMY,
        CMYK,
        Gray,
        HCL,
        HCLp,
        HSB,
        HSI,
        HSL,
        HSV,
        HWB,
        Lab,
        LCH,
        LCHab,
        LCHuv,
        Log,
        LMS,
        Luv,
        OHTA,
        Rec601YCbCr,
        Rec709YCbCr,
        RGB,
        scRGB,
        sRGB,
        Transparent,
        XyY,
        XYZ,
        YCbCr,
        YCC,
        YDbDr,
        YIQ,
        YPbPr,
        YUV,
        LinearGray,
        Jzazbz,
    }

    public byte[] byteArray;
    public Format format;
    public ColorSpace colorSpace;
}