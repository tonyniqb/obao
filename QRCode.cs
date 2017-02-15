using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.QRCode.Codec;

namespace obao.common
{
    /// <summary>
    /// 二维码
    /// </summary>
    public class QRCode
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="src">二维码原串</param>
        /// <param name="imgFormat">图片格式</param>
        /// <returns></returns>
        public static byte[] GenQRCode(string src, System.Drawing.Imaging.ImageFormat imgFormat)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 2;
            qrCodeEncoder.QRCodeVersion = 10;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

            System.Drawing.Bitmap img = qrCodeEncoder.Encode(src, Encoding.GetEncoding("gb2312"));
            byte[] data = null;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                img.Save(ms, imgFormat);
                data = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(data, 0, (int)ms.Length);
                ms.Flush();
                ms.Close();
            }
            return data;
        }
    }
}
