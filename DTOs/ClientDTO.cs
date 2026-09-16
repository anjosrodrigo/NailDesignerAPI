namespace NailDesignerAPI.DTOs {
    public class ClientDTO {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int BirthDay { get; set; }
        public int BirthMonth { get; set; }
        public string Color { get; set; } = "#C9956C";
    }

    public class CreateClientDTO {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int BirthDay { get; set; }
        public int BirthMonth { get; set; }
        public string Color { get; set; } = "#C9956C";
    }

    public class UpdateClientDTO {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int BirthDay { get; set; }
        public int BirthMonth { get; set; }
        public string Color { get; set; } = "#C9956C";
    }
}
