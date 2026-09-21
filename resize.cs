using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class Program {
    public static void Main() {
        // Only works on Windows with System.Drawing.Common
        using (var img = Image.FromFile(@"c:\Projects\mindsteps-app\assets\images\logo_horizontal.png")) {
            int newWidth = 200;
            int newHeight = (img.Height * newWidth) / img.Width;
            using (var newImg = new Bitmap(img, newWidth, newHeight)) {
                using (var ms = new MemoryStream()) {
                    newImg.Save(ms, ImageFormat.Png);
                    string b64 = Convert.ToBase64String(ms.ToArray());
                    File.WriteAllText("logo_small_b64.txt", b64);
                }
            }
        }
    }
}
