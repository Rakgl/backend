namespace API_BACKEND.Models.Utilities;

public class KhmerLunarDate
{
    public DateTime solarDate { get; set; }
    public string lunarDate { get; set; }
    public string note { get; set; }
    public bool holiday { get; set; }
    public bool buddhistDay { get; set; } //holy-day
    public bool shavedDay { get; set; } //shaved day
    public bool monthTransition { get; set; } //change color of day in the new month

    public KhmerLunarDate(){}
		
    public KhmerLunarDate(DateTime solarDate, string lunarDate)
    {
        this.solarDate = solarDate;
        this.lunarDate = lunarDate;
    }

    public KhmerLunarDate(DateTime solarDate, string lunarDate, bool buddhistDay)
    {
        this.solarDate = solarDate;
        this.lunarDate = lunarDate;
        this.buddhistDay = buddhistDay; // this property is calculated
    }

    public KhmerLunarDate(DateTime solarDate, string lunarDate, string note)
    {
        this.solarDate = solarDate;
        this.lunarDate = lunarDate;
        this.note = note;
    }

    // constructor for holiday
    public KhmerLunarDate(DateTime solarDate, bool holiday, string note)
    {
        this.solarDate = solarDate;
        this.holiday = holiday;
        this.note = note;
    }

    public KhmerLunarDate(DateTime solarDate, string lunarDate, bool holiday, string note)
    {
        this.solarDate = solarDate;
        this.lunarDate = lunarDate;
        this.holiday = holiday;
        this.note = note;
    }

    public KhmerLunarDate(DateTime solarDate, string lunarDate, string note, bool shavedDay)
    {
        this.solarDate = solarDate;
        this.lunarDate = lunarDate;
        this.note = note;
        this.shavedDay = shavedDay;
    }

    // constructor for all attribute
    public KhmerLunarDate(DateTime solarDate, string lunarDate, string note, bool holiday, bool buddhistDay, bool shavedDay, bool monthTransition)
    {
        this.solarDate = solarDate;
        this.lunarDate = lunarDate;
        this.note = note;
        this.holiday = holiday;
        this.buddhistDay = buddhistDay;
        this.shavedDay = shavedDay;
        this.monthTransition = monthTransition;
    }
		
    // define static holiday
}