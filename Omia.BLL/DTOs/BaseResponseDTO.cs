namespace Omia.BLL.DTOs
{
    public class BaseResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
