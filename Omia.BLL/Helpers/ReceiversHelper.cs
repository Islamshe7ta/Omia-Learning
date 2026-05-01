using System.Collections.Generic;
using System.Text.Json;

namespace Omia.BLL.Helpers
{
    public static class ReceiversHelper
    {
        public static string? Serialize(List<string>? receivers)
        {
            if (receivers == null || receivers.Count == 0)
                return null;
            return JsonSerializer.Serialize(receivers);
        }

        public static List<string> Deserialize(string? receivers)
        {
            if (string.IsNullOrWhiteSpace(receivers))
                return new List<string>();

            try
            {
                return JsonSerializer.Deserialize<List<string>>(receivers) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}
