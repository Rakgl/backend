using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace API_BACKEND.Models.Utilities;

public class Helper
{
    public Helper() {}

    public string numKH(string numN)
    {
        return numN
            .Replace("0", "០")
            .Replace("1", "១")
            .Replace("2", "២")
            .Replace("3", "៣")
            .Replace("4", "៤")
            .Replace("5", "៥")
            .Replace("6", "៦")
            .Replace("7", "៧")
            .Replace("8", "៨")
            .Replace("9", "៩");
    }

    public static string tranMonth(int month)
    {
        switch (month)
        {
            case 1: return "មករា";
            case 2: return "កុម្ភៈ";
            case 3: return "មីនា";
            case 4: return "មេសា";
            case 5: return "ឧសភា";
            case 6: return "មិថុនា";
            case 7: return "កក្កដា";
            case 8: return "សីហា";
            case 9: return "កញ្ញា";
            case 10: return "តុលា";
            case 11: return "វិច្ឆិកា";
            case 12: return "ធ្នូ";
            default: return "";
        }
    }
}