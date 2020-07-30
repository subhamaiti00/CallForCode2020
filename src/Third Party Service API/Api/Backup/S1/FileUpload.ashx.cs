using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Api._2
{
    /// <summary>
    /// Summary description for FileUpload
    /// </summary>
    public class FileUpload1 : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {


            try
            {
                write(context.Request.Files.Count.ToString());
                // write(HttpContext.Current.Request.Files.AllKeys.Count().ToString());
                if (context.Request.Files.Count > 0)
                {
                    //write("aaaa");
                    // Get the uploaded image from the Files collection
                    var httpPostedFile = HttpContext.Current.Request.Files[0];
                    //write(httpPostedFile.FileName);
                    if (httpPostedFile != null)
                    {
                        // Validate the uploaded image(optional)
                        // Get the complete file path
                        //  write(httpPostedFile.FileName);
                        Random r = new Random();
                        string tmpName = Path.Combine(HttpContext.Current.Server.MapPath("~/2/Fim/"), r.Next(1, 9999).ToString() + ".jpg");

                        var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/2/Fim/"), httpPostedFile.FileName);

                        // Save the uploaded file to "UploadedFiles" folder
                        httpPostedFile.SaveAs(tmpName);
                        System.Drawing.Image im = System.Drawing.Image.FromFile(tmpName);
                        int newImageHeight = 0;
                        int maxImageWidth = 1500;
                        if (im.Width > maxImageWidth)
                        {
                            newImageHeight = (int)(im.Height * ((float)maxImageWidth / (float)im.Width));
                        }
                        Bitmap b = new Bitmap(maxImageWidth, newImageHeight);
                        Graphics g = Graphics.FromImage(b);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(im, 0, 0, maxImageWidth, newImageHeight);
                        g.Dispose();
                        im.Dispose();

                        if (File.Exists(tmpName))
                            File.Delete(tmpName);

                        b.Save(fileSavePath, ImageFormat.Jpeg);
                        b.Dispose();

                    }
                    else
                        write("null");
                }
                context.Response.Write("1");

            }
            catch (Exception ex)
            {
                write(ex.Message);
                context.Response.Write(ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void write(string msg)
        {
            File.AppendAllLines(HttpContext.Current.Server.MapPath("~/2/Fim/log.txt"), DateTime.Now.ToString().Split('~'));
            File.AppendAllLines(HttpContext.Current.Server.MapPath("~/2/Fim/log.txt"), msg.Split('~'));
        }
    }
}