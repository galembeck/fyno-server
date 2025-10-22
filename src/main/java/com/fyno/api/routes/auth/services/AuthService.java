package com.fyno.api.routes.auth.services;

import com.fyno.api.routes.auth.dto.AuthRequestDTO;
import com.fyno.api.routes.auth.dto.AuthResponseDTO;
import com.fyno.api.routes.auth.dto.RegisterUserDTO;
import com.fyno.api.exception.ApiException;
import com.fyno.api.exception.ErrorCodes;
import com.fyno.api.security.jwt.JwtTokenProvider;
import com.fyno.api.routes.user.entity.User;
import com.fyno.api.routes.user.mapper.UserMapper;
import com.fyno.api.routes.user.repository.UserRepository;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

@Service
public class AuthService {
    private final UserRepository repository;
    private final PasswordEncoder encoder;
    private final JwtTokenProvider jwtTokenProvider;

    public AuthService(UserRepository repository, PasswordEncoder encoder, JwtTokenProvider jwtTokenProvider) {
        this.repository = repository;
        this.encoder = encoder;
        this.jwtTokenProvider = jwtTokenProvider;
    }

    public AuthResponseDTO register(RegisterUserDTO dto) {
        if (repository.findByEmail(dto.email()).isPresent()) {
            throw ApiException.of(ErrorCodes.USER_ALREADY_REGISTERED);
        }

        User user = UserMapper.toEntity(dto);
        user.setPassword(encoder.encode(dto.password()));
        repository.save(user);

        String token = jwtTokenProvider.generateToken(user.getEmail());
        return new AuthResponseDTO(token, "User created successfully");
    }

    public AuthResponseDTO login(AuthRequestDTO dto) {
        User user = repository.findByEmail(dto.email())
                .orElseThrow(() -> ApiException.of(ErrorCodes.USER_NOT_FOUND));

        if (!encoder.matches(dto.password(), user.getPassword())) {
            throw ApiException.of(ErrorCodes.INVALID_PASSWORD);
        }

        String token = jwtTokenProvider.generateToken(user.getEmail());
        return new AuthResponseDTO(token, "Logged in successfully");
    }

    public void logout(String token) {
        // Logout stateless → nada a invalidar
    }

    public boolean emailExists(String email) {
        return repository.existsByEmail(email);
    }
}
