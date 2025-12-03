namespace FastPayApi.Dtos
{
    public class SendPaymentDto
    {
        public string FromPhone { get; set; } = string.Empty;
        public string ToPhone { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Note { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
    }
}