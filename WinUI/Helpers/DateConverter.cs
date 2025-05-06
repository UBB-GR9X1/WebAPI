using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace WinUI.Helpers
{
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="DateTime"/> value to a <see cref="DateTimeOffset"/> value.
        /// </summary>
        /// <param name="_value">The value produced by the binding source. Expected to be a <see cref="DateTime"/>.</param>
        /// <param name="_target_type">The type of the binding target property.</param>
        /// <param name="_parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="_language">The culture to use in the converter.</param>
        /// <returns>
        /// A <see cref="DateTimeOffset"/> representing the input <see cref="DateTime"/>, or <see cref="DateTimeOffset.Now"/> if the input is not a <see cref="DateTime"/>.
        /// </returns>
        public object Convert(object _value, Type _target_type, object _parameter, string _language)
        {
            if (_value is DateTime date_time)
            {
                return new DateTimeOffset(date_time);
            }
            return DateTimeOffset.Now;
        }

        /// <summary>
        /// Converts a <see cref="DateTimeOffset"/> value back to a <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="_value">The value produced by the binding target. Expected to be a <see cref="DateTimeOffset"/>.</param>
        /// <param name="_target_type">The type to convert to.</param>
        /// <param name="_parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="_language">The culture to use in the converter.</param>
        /// <returns>
        /// A <see cref="DateTime"/> representing the input <see cref="DateTimeOffset"/>, or <see cref="DateTime.Now"/> if the input is not a <see cref="DateTimeOffset"/>.
        /// </returns>
        public object ConvertBack(object _value, Type _target_type, object _parameter, string _language)
        {
            if (_value is DateTimeOffset date_time_offset)
            {
                return date_time_offset.DateTime;
            }
            return DateTime.Now;
        }
    }
}