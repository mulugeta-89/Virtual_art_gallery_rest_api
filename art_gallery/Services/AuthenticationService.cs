namespace art_gallery.Services
{
    public class AuthenticationService
    {
        public string DetermineUserRole(bool isArtist)
        {
            return isArtist ? "ARTIST" : "USER";
        }
    }
}
