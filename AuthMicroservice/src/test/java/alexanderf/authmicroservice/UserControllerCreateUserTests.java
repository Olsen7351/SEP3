package alexanderf.authmicroservice;

import alexanderf.authmicroservice.DTO.UserDTO;
import alexanderf.authmicroservice.controllers.UserController;
import alexanderf.authmicroservice.models.User;
import alexanderf.authmicroservice.services.UserService;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.MethodArgumentNotValidException;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
public class UserControllerCreateUserTests {

    @Mock
    private UserService userService;

    @InjectMocks
    private UserController userController;

    private UserDTO userDto;
    private User user;

    private ArrayList<String> createdUsernames = new ArrayList<>();

    @BeforeEach
    void setUp() {
        userDto = new UserDTO("testUser", "test@example.com", "password123");
        user = new User("1", "testUser", "test@example.com", "password123");
        createdUsernames.add(user.getUsername());
    }

    @AfterEach
    void tearDown() {
        createdUsernames.forEach(userService::deleteUser);
        createdUsernames.clear();
    }

    // Zero
    @Test
    void testRegisterUserWithNull() {
        // Exception expected due to @Valid annotation
        assertThrows(MethodArgumentNotValidException.class, () -> {
            userController.registerUser(null);
        });
    }

    // One
    @Test
    void testRegisterUserWithValidData() {
        when(userService.registerNewUserAccount(any(UserDTO.class))).thenReturn(user);
        ResponseEntity<User> response = userController.registerUser(userDto);
        assertEquals(HttpStatus.OK, response.getStatusCode());
        assertEquals(user, response.getBody());
    }

    // Many
    @Test
    void testRegisterUserMultipleTimes() {
        UserDTO secondUserDto = new UserDTO("anotherUser", "another@example.com", "password456");
        User secondUser = new User("2", "anotherUser", "another@example.com", "password456");
        createdUsernames.add(secondUser.getUsername());

        when(userService.registerNewUserAccount(any(UserDTO.class)))
                .thenReturn(user)
                .thenReturn(secondUser);

        ResponseEntity<User> firstResponse = userController.registerUser(userDto);
        ResponseEntity<User> secondResponse = userController.registerUser(secondUserDto);

        assertEquals(HttpStatus.OK, firstResponse.getStatusCode());
        assertEquals(user, firstResponse.getBody());

        // No duplicate usernames allowed
        assertEquals(HttpStatus.CONFLICT, secondResponse.getStatusCode());
        assertEquals(secondUser, secondResponse.getBody());
    }

    // Interfaces
    @Test
    void testRegisterUserInteractionWithUserService() {
        when(userService.registerNewUserAccount(any(UserDTO.class))).thenReturn(user);
        userController.registerUser(userDto);
        verify(userService, times(1)).registerNewUserAccount(any(UserDTO.class));
    }

    // Exceptions
    @Test
    void testRegisterUserHandlingExceptions() {
        when(userService.registerNewUserAccount(any(UserDTO.class)))
                .thenThrow(new RuntimeException("Service exception"));

        assertThrows(RuntimeException.class, () -> {
            userController.registerUser(userDto);
        });
    }
}