namespace EBISX_POS.API.Services.DTO.Journal
{
    public class AddPwdScAccountJournalDTO
    {
        public required long OrderId { get; set; }
        public required DateTime EntryDate { get; set; }
        public required List<PwdScInfoDTO> PwdScInfo { get; set; }
        public string? Status { get; set; } = "Posted";
    }
    public class PwdScInfoDTO
    {
        public required string Name { get; set; }
        public required string OscaNum { get; set; }
    }
}
