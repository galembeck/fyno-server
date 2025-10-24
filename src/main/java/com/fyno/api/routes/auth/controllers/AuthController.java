package com.fyno.api.routes.auth.controllers;

import com.fyno.api.common.ApiResponse;
import com.fyno.api.exception.ApiException;
import com.fyno.api.exception.ErrorCodes;
import com.fyno.api.routes.auth.dto.request.AuthRequestDTO;
import com.fyno.api.routes.auth.dto.response.AuthResponseDTO;
import com.fyno.api.routes.auth.dto.RegisterUserDTO;
import com.fyno.api.routes.auth.services.AuthService;
import com.fyno.api.routes.user.repository.UserRepository;
import com.fyno.api.security.entity.AuthenticatedUser;
import com.fyno.api.security.jwt.JwtTokenProvider;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/auth")
public class AuthController {

    private final AuthService service;
    private final JwtTokenProvider tokenProvider;
    private final UserRepository userRepository;

    public AuthController(AuthService service, JwtTokenProvider tokenProvider, UserRepository userRepository) {
        this.service = service;
        this.tokenProvider = tokenProvider;
        this.userRepository = userRepository;
    }

    @Operation(summary = "Register", description = "Register a new user or validate email availability (dryRun)")
    @ApiResponses({
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "200", description = "User created successfully or email availability checked"),
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "400", description = "Invalid data"),
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "409", description = "User already exists")
    })
    @PostMapping("/register")
    public ApiResponse<?> register(
            @RequestBody RegisterUserDTO dto,
            @RequestParam(required = false, defaultValue = "false") boolean dryRun,
            HttpServletRequest req
    ) {
        if (dryRun) {
            boolean exists = service.emailExists(dto.email());
            return ApiResponse.ok(exists, req.getRequestURI(), null);
        }

        var result = service.register(dto);
        return ApiResponse.ok(result, req.getRequestURI(), null);
    }

    @Operation(summary = "Login", description = "Authenticate user and return JWT token")
    @ApiResponses({
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "200", description = "Authenticated successfully"),
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "400", description = "Invalid data"),
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "401", description = "Unauthorized")
    })
    @PostMapping("/login")
    public ApiResponse<AuthResponseDTO> login(@RequestBody AuthRequestDTO dto, HttpServletRequest req) {
        var result = service.login(dto);
        return ApiResponse.ok(result, req.getRequestURI(), null);
    }

    @Operation(summary = "Logout", description = "Invalidate current JWT and revoke user session")
    @ApiResponses({
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "200", description = "Logout successful"),
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "401", description = "Invalid or expired token")
    })
    @PostMapping("/logout")
    public ApiResponse<Void> logout(
            @RequestHeader("Authorization") String token,
            HttpServletRequest req
    ) {
        if (token == null || !token.startsWith("Bearer ")) {
            throw ApiException.of(ErrorCodes.TOKEN_INVALID);
        }

        String cleanToken = token.replace("Bearer ", "").trim();
        service.logout(cleanToken);
        return ApiResponse.ok(null, req.getRequestURI(), null);
    }

    @Operation(summary = "Get Current User", description = "Retrieve information about the currently authenticated user")
    @ApiResponses({
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "200", description = "User info returned"),
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "401", description = "Unauthorized")
    })
    @GetMapping("/me")
    public ApiResponse<?> me(@AuthenticationPrincipal AuthenticatedUser authUser, HttpServletRequest req) {
        if (authUser == null) {
            throw ApiException.of(ErrorCodes.UNAUTHORIZED);
        }

        var user = authUser.getUser();
        return ApiResponse.ok(user, req.getRequestURI(), null);
    }

}
