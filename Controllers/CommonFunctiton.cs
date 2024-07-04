using EMP.Models.Master;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EMP.Controllers
{
    public class CommonFunctiton
    {
        private readonly LogErrors _logErrors = new LogErrors();
        public static string CreateToken(Users user)
        {
            string sec = ConfigurationManager.AppSettings.Get("secret").ToString();
            var issuer = ConfigurationManager.AppSettings["issuer"];
            var audience = ConfigurationManager.AppSettings.Get("Audience");
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(signingCredentials);
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddHours(24) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);
            var payload = new JwtPayload
            {
                {"email", user.Email},
                {"userId", user.Id},
                {"role", user.RoleName},
                {"sub", user.Id},
                {"nbf", issuedAt},
                {"iat", issuedAt},
                {"iss",issuer.ToLower()},
                {"aud",audience.ToLower()},
                {"exp", expiry}
            };
            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(secToken);
            Console.WriteLine(tokenString);
            return tokenString;
        }
        public static string CreateSalt()
        {
            var data = new byte[0x10];
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                cryptoServiceProvider.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }
        public static string EncryptPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Format("{0}{1}", salt, password);
                var saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }
        public static ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public void Compressimage(Stream sourcePath, string targetPath, String filename)
        {
            //https://www.c-sharpcorner.com/UploadFile/85ed7a/compress-and-resize-the-image-using-web-api/

            try
            {

                using (var image = Image.FromStream(sourcePath))
                {
                    float maxHeight = 900.0f;
                    float maxWidth = 900.0f;
                    int newWidth;
                    int newHeight;
                    string extension;
                    Bitmap originalBMP = new Bitmap(sourcePath);
                    int originalWidth = originalBMP.Width;
                    int originalHeight = originalBMP.Height;

                    if (originalWidth > maxWidth || originalHeight > maxHeight)
                    {

                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else
                    {
                        newWidth = (int)originalWidth;
                        newHeight = (int)originalHeight;

                    }
                    Bitmap bitMAP1 = new Bitmap(originalBMP, newWidth, newHeight);
                    Graphics imgGraph = Graphics.FromImage(bitMAP1);
                    extension = Path.GetExtension(targetPath);
                    if (extension == ".png" || extension == ".gif")
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);


                        bitMAP1.Save(targetPath, image.RawFormat);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();
                    }
                    else if (extension == ".jpg")
                    {

                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                        ImageCodecInfo jpgEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);
                        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        bitMAP1.Save(targetPath, jpgEncoder, myEncoderParameters);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();

                    }


                }

            }
            catch (Exception)
            {
                throw;

            }
        }
    }
}