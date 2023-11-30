package alexanderf.authmicroservice.models;

public class JwtResponse {
    private final String jwt;

    public JwtResponse(String jwt) {
        this.jwt = jwt;
    }

    // Getter
    public String getJwt() {
        return jwt;
    }
}