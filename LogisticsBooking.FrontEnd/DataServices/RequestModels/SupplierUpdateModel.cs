namespace LogisticsBooking.FrontEnd.DataServices.RequestModels
{
    public class SupplierUpdateModel
    {
        public string Email { get; set; }
        public int Telephone { get; set; }
        public string Name { get; set; }

        public SupplierUpdateModel(string email, int telephone,  string name)
        {
            Email = email;
            Telephone = telephone;
            Name = name;
        }
    }
}