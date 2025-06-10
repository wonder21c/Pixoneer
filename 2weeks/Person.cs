
namespace AddressBook
{
    public class Person
    {
        public string name { get; set; }
        public string team { get; set; }
        public string grade { get; set; }
        public string phoneNum { get; set; }
        public string email { get; set; }

        public override string ToString()
        {
            return $"{name}|{team}|{grade}|{phoneNum}|{email}";
        }
        

        public static Person FromString(string line)
        {
            var parts = line.Split('|');
            if (parts.Length != 5) return null;

            return new Person
            {
                name = parts[0],
                team = parts[1],
                grade = parts[2],
                phoneNum = parts[3],
                email = parts[4]
            };
        }
    }
}
