namespace LeadTheBoard.Application.Utilities.Helpers
{
    /// <summary>
    /// Tarih ve saat ile ilgili işlemleri yapmak için kullanılan yardımcı sınıf
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Pazartesi gününün başlangıç tarihini döndüren metod
        /// </summary>
        public static DateTime GetStartOfWeek()
        {
            var today = DateTime.Today;
            var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var startOfWeek = today.AddDays(-diff);
            return startOfWeek;
        }
    }
}
