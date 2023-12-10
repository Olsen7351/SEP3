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
        // Check it returns bad request
        ResponseEntity<Void> response = userController.registerUser(new UserDTO("", "", ""));
        assertEquals(HttpStatus.BAD_REQUEST, response.getStatusCode());
    }

    // One
    @Test
    void testRegisterUserWithValidData() {
        when(userService.registerNewUserAccount(any(UserDTO.class))).thenReturn(user);
        ResponseEntity<Void> response = userController.registerUser(userDto);
        assertEquals(HttpStatus.OK, response.getStatusCode());
    }

    // Many
    @Test
    void testRegisterUserWhenUserAlreadyExists() {
        UserDTO alreadyRegisteredUser = new UserDTO("string", "test@gmail.com", "password123");
        when(userService.findByUsername(alreadyRegisteredUser.getUsername())).thenReturn(true);

        ResponseEntity<Void> response = userController.registerUser(alreadyRegisteredUser);
        assertEquals(HttpStatus.CONFLICT, response.getStatusCode());
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