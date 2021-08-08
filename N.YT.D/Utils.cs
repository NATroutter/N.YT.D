using System;

namespace N.YT.D {
    class Utils {

        public Format GetFormat(string str) {
            Format form = Format.INVALID;
            if (Enum.TryParse(str, out form)) {
                return form;
            }
            return Format.INVALID;
        }

        public string DateFormat(DateTime? time) {
            if (time.HasValue) {
                return time.Value.Day + "." + time.Value.Month + "." + time.Value.Year;
            }
            return "Unknown";
        }

        public string numFormat(long? num) {
            if (num.HasValue) {
                return string.Format("{0:0,0}", num.Value).Replace(",", " ");
            }
            return "Unknown";
        }

    }
}
