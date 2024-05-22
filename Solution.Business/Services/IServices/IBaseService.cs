namespace Solution.Business.Services.IServices
{
    public interface IBaseService
    {
        public string Encode(int request);
        
        public int Decode(string request);
        public string CompanyIdEncode(int request);
        public int CompanyIdDecode();
        public string GetLoginUserId();

    }
}
