namespace Omia.BLL.DTOs.TeacherAssignment
{
    public class CorrectAssignmentResponseDTO : BaseResponseDTO
    {
        public Guid Id { get; set; }
        public string? SubmittedFile { get; set; }
        public DateTime SubmittedAt { get; set; }
        public float? Grade { get; set; }
        public string? CorrectedFile { get; set; }
        public string? TeacherComment { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
