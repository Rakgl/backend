using System;
using System.Collections;
using System.Collections.Generic;
using API_BACKEND.Models.Utilities;
using BACKEND.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;



namespace BACKEND.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class KhmerLunarController : ControllerBase
    {
        private readonly ILogger<KhmerLunarController> _logger;

        public KhmerLunarController(ILogger<KhmerLunarController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public KhmerLunarViewModel Get(int dm = 0)
        {
            if (dm == 0)
            {
                //
            }
            DateTime dt = DateTime.Now;

            DateTime firstDayOfMonth = new DateTime(dt.Year, dt.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            Hashtable hashMonth = KhmerLunar.getHashMonth();

            string firstDayLunarCode = KhmerLunar.getKhmerLunarCode(firstDayOfMonth);
            string prevMonth = hashMonth[firstDayLunarCode.Substring(8,2)].ToString();
            //ViewBag.prevMonth = prevMonth;

            string lastDayLunarCode = KhmerLunar.getKhmerLunarCode(lastDayOfMonth);
            string nextMonth = hashMonth[lastDayLunarCode.Substring(8,2)].ToString();
            //ViewBag.nextMonth = nextMonth;

            int skipper = (int) firstDayOfMonth.DayOfWeek; 
            int lastDay = lastDayOfMonth.Day; 
            int numRows = (lastDay + skipper) / 7;

            String mKh = KhmerLunar.getKhmerLunarString(dt);
            //ViewBag.mKh = mKh;

            //function to create calendar needs to be param of month (default: current month)
            ICollection<KhmerLunarDate> klds = new List<KhmerLunarDate>();
            Hashtable hsMonth = KhmerLunar.getHashMonth();

            for (int i = 0; i < lastDay; i++)
            {
                DateTime solarDate = firstDayOfMonth.AddDays(i);

                string enText = KhmerLunar.getKhmerLunarCode(solarDate);
                string month = enText.Substring(8, 2);
                month = KhmerLunar.getLunarMonth(month);
                string kr = enText.Substring(10, 1);
                kr = kr.Replace("K", "កើត").Replace("R", "រោច");
                string d = enText.Substring(11, 2);
                int dd = int.Parse(d);
                d = KhmerLunar.convertToKhmerNum(dd.ToString());
                string lunarDate = d + kr + " ខែ" + month;

                KhmerLunarDate kld = new KhmerLunarDate();
                kld.solarDate = solarDate;
                kld.lunarDate = lunarDate;
                kld.note = "";
                kld.holiday = false;
                kld.buddhistDay = false;
                kld.shavedDay = false;
                kld.monthTransition = false;
                
                klds.Add(kld);
            }
            
            KhmerLunarViewModel khmerLunarView = new KhmerLunarViewModel();
            khmerLunarView.khmerLunarDates = klds;
            khmerLunarView.prevMonth = prevMonth;
            khmerLunarView.nextMonth = nextMonth;
            khmerLunarView.skipper = skipper;
            khmerLunarView.numRows = numRows;
            khmerLunarView.lastDay = lastDay;
            khmerLunarView.lLunarDate = mKh;
            
            return khmerLunarView;
        }
    }
}

