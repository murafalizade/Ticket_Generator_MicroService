namespace QRTicketGenerator.API.Dtos
{
    public class TicketDesignDto
    {
        public string DesignFilePath { get; set; } 
        public float QrCodeX { get; set; } 
        public float QrCodeY { get; set; } 
        public int ValueCount { get; set; }
        public float PositionX1 { get; set; }
        public float PositionY1 { get; set; }
        public int fontSize1 { get; set; }
        public float PositionX2 { get; set; } 
        public float PositionY2 { get; set; } 
        public int fontSize2 { get; set; } 
        public float PositionX3 { get; set; }
        public float PositionY3 { get; set; }
        public int fontSize3 { get; set; } 
        public float PositionX4 { get; set; }
        public float PositionY4 { get; set; } 
        public int fontSize4 { get; set; }
    }
}
