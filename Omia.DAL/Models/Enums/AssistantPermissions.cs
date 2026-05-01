namespace Omia.DAL.Models.Enums
{
    [Flags]
    public enum AssistantPermissions
    {
        None = 0,
        ReplyToStudents = 1,
        CorrectPapers = 2,
        UploadFiles = 4,
        ManageLiveSessions = 8,
        AssignmentReviewer = 16,
        QuizManager = 32,
    }
}
