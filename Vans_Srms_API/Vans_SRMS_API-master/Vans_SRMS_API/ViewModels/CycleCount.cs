using System;
using System.Collections.Generic;
using static Vans_SRMS_API.Utils.Util;

namespace Vans_SRMS_API.ViewModels
{
    public class CycleCountInput
    {
        public string LocationBarcode { get; set; }
        public List<CycleCountLineItem> Items { get; set; }
    }
    public class CycleCountLineItem
    {
        private string _gtin;
        public string GTIN
        {
            get { return padGTIN(_gtin); }
            set { _gtin = value; }
        }
        public DateTime ScannedAt { get; set; }
    }
}
