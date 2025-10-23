package com.fyno.api.routes.v1.integration.apikey.service.impl;

import com.fyno.api.exception.ApiException;
import com.fyno.api.exception.ErrorCodes;
import com.fyno.api.routes.user.entity.User;
import com.fyno.api.routes.user.repository.UserRepository;
import com.fyno.api.routes.v1.integration.apikey.dto.request.ApiKeyRequestDTO;
import com.fyno.api.routes.v1.integration.apikey.dto.response.ApiKeyResponseDTO;
import com.fyno.api.routes.v1.integration.apikey.entity.ApiKey;
import com.fyno.api.routes.v1.integration.apikey.enums.ApiKeyOrigin;
import com.fyno.api.routes.v1.integration.apikey.repository.ApiKeyRepository;
import com.fyno.api.routes.v1.integration.apikey.service.ApiKeyService;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import java.security.SecureRandom;
import java.time.format.DateTimeFormatter;
import java.util.Base64;
import java.util.List;
import java.util.UUID;

@Service
public class ApiKeyServiceImpl implements ApiKeyService {

    private final ApiKeyRepository repository;
    private final UserRepository userRepository;

    public ApiKeyServiceImpl(ApiKeyRepository repository, UserRepository userRepository) {
        this.repository = repository;
        this.userRepository = userRepository;
    }

    @Override
    public ApiKeyResponseDTO createApiKey(String email, ApiKeyRequestDTO dto) {
        User user = userRepository.findByEmail(email)
                .orElseThrow(() -> ApiException.of(ErrorCodes.USER_NOT_FOUND));

        ApiKeyOrigin origin = dto.origin() != null ? dto.origin() : ApiKeyOrigin.DASHBOARD;

        String generatedKey = generateSecureApiKey();

        ApiKey apiKey = ApiKey.builder()
                .keyValue(generatedKey)
                .notes(dto.notes())
                .origin(origin)
                .user(user)
                .build();

        try {
            repository.save(apiKey);
        } catch (DataIntegrityViolationException e) {
            throw ApiException.of(ErrorCodes.DATA_INTEGRITY_VIOLATION);
        }

        return new ApiKeyResponseDTO(
                apiKey.getId(),
                apiKey.getKeyValue(),
                apiKey.getNotes(),
                apiKey.getOrigin().name(),
                apiKey.isActive(),
                apiKey.getCreatedAt().format(DateTimeFormatter.ISO_LOCAL_DATE_TIME)
        );
    }

    @Override
    public List<ApiKeyResponseDTO> listKeys(String email) {
        User user = userRepository.findByEmail(email)
                .orElseThrow(() -> ApiException.of(ErrorCodes.USER_NOT_FOUND));

        return repository.findByUserId(user.getId())
                .stream()
                .map(apiKey -> new ApiKeyResponseDTO(
                        apiKey.getId(),
                        apiKey.getKeyValue(),
                        apiKey.getNotes(),
                        apiKey.getOrigin().name(),
                        apiKey.isActive(),
                        apiKey.getCreatedAt().format(DateTimeFormatter.ISO_LOCAL_DATE_TIME)
                ))
                .toList();
    }

    @Override
    public void revokeKey(String keyId, String email) {
        User user = userRepository.findByEmail(email)
                .orElseThrow(() -> ApiException.of(ErrorCodes.USER_NOT_FOUND));

        ApiKey apiKey = repository.findById(keyId)
                .filter(k -> k.getUser().getId().equals(user.getId()))
                .orElseThrow(() -> ApiException.of(ErrorCodes.API_KEY_NOT_FOUND));

        repository.delete(apiKey);
    }

    private String generateSecureApiKey() {
        SecureRandom random = new SecureRandom();
        byte[] bytes = new byte[24];
        random.nextBytes(bytes);
        String tokenPart = Base64.getUrlEncoder().withoutPadding().encodeToString(bytes);
        return UUID.randomUUID() + "-" + tokenPart;
    }
}
