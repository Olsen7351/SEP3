package alexanderf.authmicroservice;

import alexanderf.authmicroservice.DTO.UserDTO;
import alexanderf.authmicroservice.controllers.UserController;
import alexanderf.authmicroservice.models.JwtResponse;
import alexanderf.authmicroservice.models.User;
import alexanderf.authmicroservice.services.UserService;
import alexanderf.authmicroservice.util.JwtUtil;
import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.AuthenticationServiceException;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.MethodArgumentNotValidException;

import java.io.Console;
import java.util.ArrayList;

import static org.mockito.ArgumentMatchers.any;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
public class UserControllerAuthTests {

    @Mock
    private UserService userService;

    @Mock
    private AuthenticationManager authenticationManager;

    @Mock
    private JwtUtil jwtUtil;

    @InjectMocks
    private UserController userController;

    private UserDTO userDto;
    private UserDetails userDetails;

    private ArrayList<String> createdUsernames = new ArrayList<>();

    @BeforeEach
    void setUp() {
        userDto = new UserDTO("testUserAuth", "test@example.com", "password123");
        userDetails = new org.springframework.security.core.userdetails.User(userDto.getUsername(), userDto.getPassword(), new ArrayList<>());
        userController.registerUser(userDto);
        createdUsernames.add(userDto.getUsername());
    }

    @AfterEach
    void tearDown() {
        createdUsernames.forEach(userService::deleteUser);
        createdUsernames.clear();
    }

    @Test
    void testCreateAuthenticationTokenSuccess() {
        // TODO: Not working idk why

        Authentication authentication = mock(Authentication.class);
        UserDetails userDetails = mock(UserDetails.class);
        when(userDetails.getUsername()).thenReturn(userDto.getUsername());

        when(authenticationManager.authenticate(any(UsernamePasswordAuthenticationToken.class)))
                .thenReturn(authentication);
        when(authentication.getPrincipal()).thenReturn(userDetails);

        ResponseEntity<?> response = userController.createAuthenticationToken(userDto);
        assertNotNull(response.getBody());

        // Cast the response body to JwtResponse and extract the token
        JwtResponse jwtResponse = (JwtResponse) response.getBody();
        String token = jwtResponse.getJwt();

        JwtUtil jwtUtil = new JwtUtil();
        // Extract the username from the token
        String extractedUsername = jwtUtil.extractSubjectFromToken(token);

        assertEquals(HttpStatus.OK, response.getStatusCode());
        assertEquals(userDto.getUsername(), extractedUsername);
    }


    @Test
    void testAuthenticationWithBadCredentials() {
        when(authenticationManager.authenticate(any(UsernamePasswordAuthenticationToken.class)))
                .thenThrow(new BadCredentialsException("Bad credentials"));

        ResponseEntity<?> response = userController.createAuthenticationToken(userDto);

        assertEquals(HttpStatus.UNAUTHORIZED, response.getStatusCode());
        assertEquals("Authentication failed: Bad credentials", response.getBody());
    }

    @Test
    void testCreateAuthenticationTokenExceptionHandling() {
        when(authenticationManager.authenticate(any(UsernamePasswordAuthenticationToken.class)))
                .thenThrow(new AuthenticationServiceException("Authentication service exception"));

        ResponseEntity<?> response = userController.createAuthenticationToken(userDto);

        assertEquals(HttpStatus.UNAUTHORIZED, response.getStatusCode());
        assertTrue(response.getBody().toString().contains("Authentication failed"));
    }
}
