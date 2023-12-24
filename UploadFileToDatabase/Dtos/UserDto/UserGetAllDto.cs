namespace UploadFileToDatabase.Dtos.UserDto
{
    public class UserGetAllDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public byte[]? CurriculumVitae { get; set; }
    }
}
