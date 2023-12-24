namespace UploadFileToDatabase.Dtos.UserDto
{
    public class UserEditDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public byte[]? CurriculumVitae { get; set; }
        public string? CurriculumVitaeBase64 { get; set; }
        public IFormFile? FormFile { get; set; } // برای گرفتن فایل رزومه از فرم
    }
}
