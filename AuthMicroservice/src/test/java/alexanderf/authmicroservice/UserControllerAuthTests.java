package alexanderf.authmicroservice;

import alexanderf.authmicroservice.DTO.UserDTO;
import alexanderf.authmicroservice.controllers.UserController;
import alexanderf.authmicroservice.models.JwtResponse;
import alexanderf.authmicroservice.models.User;
import alexanderf.authmicroservice.services.UserService;
import alexanderf.authmicroservice.util.JwtUtil;
import org.junit.jupiter.api.AfterAll;
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
    private String jwtToken = "test.jwt.token";

    @BeforeEach
    void setUp() {
        userDto = new UserDTO("testUserAuth", "test@example.com", "password123");
        userDetails = new org.springframework.security.core.userdetails.User(userDto.getUsername(), userDto.getPassword(), new ArrayList<>());
    }

    @Test
    void testCreateAuthenticationTokenSuccess() {
        Authentication authentication = mock(Authentication.class);

        when(authenticationManager.authenticate(any(UsernamePasswordAuthenticationToken.class)))
                .thenReturn(authentication);
        when(authentication.getPrincipal()).thenReturn(userDetails);
        when(jwtUtil.generateToken(userDetails)).thenReturn(jwtToken);

        ResponseEntity<?> response = userController.createAuthenticationToken(userDto);

        assertEquals(ResponseEntity.ok(new JwtResponse(jwtToken)), response);
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

    @Test
    void testTokenGenerationAndManagerInteraction() {
        Authentication authentication = mock(Authentication.class);

        when(authenticationManager.authenticate(any(UsernamePasswordAuthenticationToken.class)))
                .thenReturn(authentication);
        when(authentication.getPrincipal()).thenReturn(userDetails);
        when(jwtUtil.generateToken(userDetails)).thenReturn(jwtToken);

        userController.createAuthenticationToken(userDto);

        verify(authenticationManager, times(1)).authenticate(any(UsernamePasswordAuthenticationToken.class));
        verify(jwtUtil, times(1)).generateToken(any(UserDetails.class));
    }

    @Test
    void testCreateAuthenticationTokenWithInvalidInput() {
        assertThrows(MethodArgumentNotValidException.class, () -> {
            userController.createAuthenticationToken(null);
        });
    }
}
