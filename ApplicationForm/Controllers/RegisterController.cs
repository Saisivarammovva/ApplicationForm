using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace ApplicationForm.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        [HttpGet]
        public ActionResult Home()
        {

            ViewBag.UserTypeList = new List<SelectListItem>

            { new SelectListItem { Text = "Staff", Value = "Staff" },
               new SelectListItem { Text = "Student", Value = "Student" },
               new SelectListItem{Text="Non-Teaching Staff", Value="Non-Teaching"}
            };

            sivaramEntities sd = new sivaramEntities();
            ViewBag.countryList = new SelectList(GetCountries(), "cid", "cname");
            return View();
        }
        public List<country> GetCountries()
        {
            sivaramEntities sd = new sivaramEntities();
            List<country> countries = sd.countries.ToList();
            return countries;
        }
        public ActionResult GetStates(int cid)
        {
            sivaramEntities sd = new sivaramEntities();
            List<State> selectList = sd.States.Where(x => x.cid == cid).ToList();
            ViewBag.Slist = new SelectList(selectList, "sid", "sname");
            return PartialView("DisplayStates");
        }
        public ActionResult GetCities(int sid)
        {
            sivaramEntities sd = new sivaramEntities();
            List<city> selectList = sd.cities.Where(x => x.sid == sid).ToList(); ;
            ViewBag.citylist = new SelectList(selectList, "cityid", "cityname");
            return PartialView("DisplayCities");
        }
        public string GenerateCaptchaText()
        {
            Random random = new Random();
            string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string captchaString = "";
            for (int i = 0; i < 6; i++)
            {
                captchaString += chars[random.Next(chars.Length)];
            }
            return captchaString;
        }
        public ActionResult CaptchaImage()
        {
            var captchaText = GenerateCaptchaText();
            // Create the captcha image
            var image = new Bitmap(125, 50);
            var graphics = Graphics.FromImage(image);
            // Set up the graphics settings
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.BurlyWood);
            var font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold, GraphicsUnit.Pixel);
            var brush = new SolidBrush(Color.Black);
            // Draw the captcha text
            graphics.DrawString(captchaText, font, brush, new PointF(20, 20));
            // Draw the lines
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                int x1 = random.Next(125);
                int y1 = random.Next(50);
                int x2 = random.Next(125);
                int y2 = random.Next(50);
                graphics.DrawLine(new Pen(Color.Yellow), x1, y1, x2, y2);
            }
            // Save the image to a memory stream and return the byte array
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            Session["CaptchaText"] = captchaText;
            return File(stream.ToArray(), "image/png");

        }


    }
}
