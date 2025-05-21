using System;
using System.Collections.Generic;
using API_BACKEND.Models.Utilities;

namespace BACKEND.Entity;

public class KhmerLunarViewModel
{
    public string prevMonth { get; set; }
    public string nextMonth { get; set; }
    public int skipper { get; set; }
    public int lastDay { get; set; }
    public int numRows { get; set; }
    public string lLunarDate { get; set; } 

    public ICollection<KhmerLunarDate> khmerLunarDates { get; set; }

    public KhmerLunarViewModel(){}
}