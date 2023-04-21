namespace APIKrolowie.Models
{
    public class Users
    {
        public int Id { get; set; }
        private string _firstName = null!;
        public string _lastName = null!;
        public string _password = null!;
        public string _pesel = null!;
        public string _eMail = null!;
        public string _phoneNumber = null!;
        public string FirstName 
        { 
            get => _firstName;
            set => _firstName = value ?? throw new ArgumentNullException("First Name is required.");
        }
        public string LastName
        {
            get => _lastName;
            set => _lastName = value ?? throw new ArgumentNullException("Last Name is required.");
        }
        public string Password
        {
            get => _password;
            set => _password = value ?? throw new ArgumentNullException("Password is required.");
        }
        public string Pesel
        {
            get => _pesel;
            set => _pesel = value ?? throw new ArgumentNullException("Pesel is required.");
        }
        public string EMail
        {
            get => _eMail;
            set => _eMail = value ?? throw new ArgumentNullException("e-mail is required.");
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value ?? throw new ArgumentNullException("Phone Number is required.");
        }
        public byte Age { get; set; }
        public float ElectricityUsage {get; set; }
    }
}
