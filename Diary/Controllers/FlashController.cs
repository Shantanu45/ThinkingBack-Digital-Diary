using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Controllers
{
    public class FlashController : Controller
    {
        public enum FlashMessageType
        {
            Success,
            Warning,
            Error
        }

        public void SetFlash(FlashMessageType type, string text)
        {
            TempData["FlashMessage.Type"] = type;
            TempData["FlashMessage.Text"] = text;
        }

    }
}
