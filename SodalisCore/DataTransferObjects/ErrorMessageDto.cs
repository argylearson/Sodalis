namespace SodalisCore.DataTransferObjects {
    public class ErrorMessageDto {
        public string Message { get; set; }

        public ErrorMessageDto(string message) {
            Message = message;
        }
    }
}