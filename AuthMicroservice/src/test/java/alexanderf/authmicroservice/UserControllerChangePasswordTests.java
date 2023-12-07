package alexanderf.authmicroservice;

import alexanderf.authmicroservice.DTO.ChangePasswordDTO;
import alexanderf.authmicroservice.DTO.UserDTO;
import alexanderf.authmicroservice.controllers.UserController;
import alexanderf.authmicroservice.models.User;
import alexanderf.authmicroservice.services.UserService;
import alexanderf.authmicroservice.util.JwtUtil;
import io.jsonwebtoken.JwtException;
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
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;

import java.util.ArrayList;

import static org.mockito.Mockito.*;
import static org.junit.jupiter.api.Assertions.*;

@ExtendWith(MockitoExtension.class)
public class UserControllerChangePasswordTests {

    @Mock
    private UserService userService;

    @Mock
    private AuthenticationManager authenticationManager;

    @Mock
    private JwtUtil jwtUtil;

    @InjectMocks
    private UserController userController;

    private ChangePasswordDTO changePasswordDto;
    private UserDTO userDto;
    private ArrayList<UserDTO> registeredUsers;

    @BeforeEach
    void setUp() {
        userDto = new UserDTO("testUser", "test@example.com", "password123");
        changePasswordDto = new ChangePasswordDTO("password123", "newPassword");

        registeredUsers = new ArrayList<>();
        registeredUsers.add(userDto);
    }

    @AfterEach
    void tearDown() {
        for (UserDTO user : registeredUsers) {
            userService.deleteUser(user.getUsername());
        }
    }

    @Test
    void whenChangePasswordWithValidCredentials_thenStatusOk() {
        when(jwtUtil.extractSubjectFromToken(anyString())).thenReturn(userDto.getUsername());
        when(authenticationManager.authenticate(any())).thenReturn(mock(Authentication.class));
        doNothing().when(userService).changePassword(userDto.getUsername(), changePasswordDto.getNewPassword());

        ResponseEntity<?> response = userController.changePassword(changePasswordDto, "Bearer validToken");

        assertEquals(HttpStatus.OK, response.getStatusCode());
        assertEquals("Password changed successfully", response.getBody());
    }

    @Test
    void whenChangePasswordWithInvalidCredentials_thenStatusUnauthorized() {
        when(jwtUtil.extractSubjectFromToken(anyString())).thenReturn(userDto.getUsername());
        doThrow(new AuthenticationException("Authentication failed") {})
                .when(authenticationManager).authenticate(any());

        ResponseEntity<?> response = userController.changePassword(changePasswordDto, "Bearer invalidToken");

        assertEquals(HttpStatus.UNAUTHORIZED, response.getStatusCode());
        assertTrue(response.getBody().toString().contains("Authentication failed"));
    }

    @Test
    void whenChangePasswordWithIncorrectCurrentPassword_thenStatusUnauthorized() {
        when(jwtUtil.extractSubjectFromToken(anyString())).thenReturn(userDto.getUsername());
        doThrow(new AuthenticationException("Incorrect current password") {})
                .when(authenticationManager).authenticate(any());

        ResponseEntity<?> response = userController.changePassword(changePasswordDto, "Bearer validToken");

        assertEquals(HttpStatus.UNAUTHORIZED, response.getStatusCode());
        assertTrue(response.getBody().toString().contains("Incorrect current password"));
    }

    @Test
    void whenChangePasswordForNonExistentUser_thenStatusNotFound() {
        when(jwtUtil.extractSubjectFromToken(anyString())).thenReturn("nonExistentUser");
        when(userService.findByUsername("nonExistentUser")).thenReturn(false);

        ResponseEntity<?> response = userController.changePassword(changePasswordDto, "Bearer validToken");

        assertEquals(HttpStatus.NOT_FOUND, response.getStatusCode());
        assertTrue(response.getBody().toString().contains("User not found"));
    }
}