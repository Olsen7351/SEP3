package alexanderf.authmicroservice.controllers;

import alexanderf.authmicroservice.DTO.ChangePasswordDTO;
import alexanderf.authmicroservice.DTO.UserDTO;
import alexanderf.authmicroservice.models.JwtResponse;
import alexanderf.authmicroservice.services.UserService;
import alexanderf.authmicroservice.util.JwtUtil;
import io.jsonwebtoken.Claims;
import io.jsonwebtoken.Jwts;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.autoconfigure.security.oauth2.resource.OAuth2ResourceServerProperties;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.*;
import org.springframework.security.oauth2.jwt.Jwt;


import javax.validation.Valid;
import java.util.Base64;

@RestController
@RequestMapping("/api/users")
public class UserController {
    @Autowired
    private UserService userService;

    @Autowired
    private AuthenticationManager authenticationManager;

    @Autowired
    private JwtUtil jwtUtil;

    @PostMapping
    public ResponseEntity<Void> registerUser(@Valid @RequestBody UserDTO userDto) {
        if (userService.findByUsername(userDto.getUsername())) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build();
        }
        userService.registerNewUserAccount(userDto);
        return ResponseEntity.ok().build();
    }

    @PostMapping("/authenticate")
    public ResponseEntity<?> createAuthenticationToken(@Valid @RequestBody UserDTO userDto) {
        if (userDto == null)
        {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body("UserDTO is null");
        }
        try {
            Authentication authentication = authenticationManager.authenticate(
                    new UsernamePasswordAuthenticationToken(userDto.getUsername(), userDto.getPassword())
            );

            SecurityContextHolder.getContext().setAuthentication(authentication);
            UserDetails userDetails = (UserDetails) authentication.getPrincipal();
            final String token = jwtUtil.generateToken(userDetails);

            return ResponseEntity.ok(new JwtResponse(token));
        } catch (AuthenticationException e) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body("Authentication failed: " + e.getMessage());
        }
    }

    @PutMapping("/change-password")
    public ResponseEntity<?> changePassword(@Valid @RequestBody ChangePasswordDTO changePasswordDto,
                                            @RequestHeader("Authorization") String authorizationHeader) {
        try {
            // Extract token from the Authorization header
            String jwtToken = authorizationHeader.substring(7); // Remove "Bearer " prefix

            // Extract username from the JWT token
            String currentLoggedInUsername = jwtUtil.extractSubjectFromToken(jwtToken);
            System.out.println("Trying to change password for user: " + currentLoggedInUsername);

            // Authenticate the user with the current password
            authenticationManager.authenticate(
                    new UsernamePasswordAuthenticationToken(
                            currentLoggedInUsername, changePasswordDto.getCurrentPassword())
            );

            // Update the user's password
            userService.changePassword(currentLoggedInUsername, changePasswordDto.getNewPassword());

            return ResponseEntity.ok().body("Password changed successfully");
        } catch (AuthenticationException e) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body("Authentication failed: " + e.getMessage());
        }
    }
}


