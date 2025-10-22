package com.fyno.api.routes.v1.integration.apikey.service;

import com.fyno.api.exception.ApiException;
import com.fyno.api.exception.ErrorCodes;
import com.fyno.api.routes.user.entity.User;
import com.fyno.api.routes.user.repository.UserRepository;
import com.fyno.api.routes.v1.integration.apikey.dto.request.ApiKeyRequestDTO;
import com.fyno.api.routes.v1.integration.apikey.dto.response.ApiKeyResponseDTO;
import com.fyno.api.routes.v1.integration.apikey.entity.ApiKey;
import com.fyno.api.routes.v1.integration.apikey.enums.ApiKeyOrigin;
import com.fyno.api.routes.v1.integration.apikey.repository.ApiKeyRepository;
import org.springframework.stereotype.Service;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.security.SecureRandom;
import java.time.format.DateTimeFormatter;
import java.util.Base64;
import java.util.List;
import java.util.UUID;

@Service
public class ApiKeyService {

    private static final Logger log = LoggerFactory.getLogger(ApiKeyService.class);

    private final ApiKeyRepository repository;
    private final UserRepository userRepository;

    public ApiKeyService(ApiKeyRepository repository, UserRepository userRepository) {
        this.repository = repository;
        this.userRepository = userRepository;
    }

    public ApiKeyResponseDTO createApiKey(String email, ApiKeyRequestDTO dto) {
        log.info("[APIKEY] Buscando usuário por email: {}", email);
        User user = userRepository.findByEmail(email)
                .orElseThrow(() -> ApiException.of(ErrorCodes.USER_NOT_FOUND));

        ApiKeyOrigin origin = dto.origin() != null ? dto.origin() : ApiKeyOrigin.DASHBOARD;

        log.info("[APIKEY] Gerando chave segura para usuário: {}", email);
        String generatedKey = generateSecureApiKey();

        ApiKey apiKey = ApiKey.builder()
                .keyValue(generatedKey)
                .notes(dto.notes())
                .origin(origin)
                .user(user)
                .build();

        log.info("[APIKEY] Salvando chave no banco para usuário: {}", email);
        repository.save(apiKey);

        log.info("[APIKEY] Chave salva com sucesso para usuário: {}", email);
        return toResponse(apiKey);
    }

    public List<ApiKeyResponseDTO> listKeys(String email) {
        User user = userRepository.findByEmail(email)
                .orElseThrow(() -> ApiException.of(ErrorCodes.USER_NOT_FOUND));

        return repository.findByUserId(user.getId())
                .stream()
                .map(this::toResponse)
                .toList();
    }

    public void revokeKey(String keyId, String email) {
        User user = userRepository.findByEmail(email)
                .orElseThrow(() -> ApiException.of(ErrorCodes.USER_NOT_FOUND));

        ApiKey apiKey = repository.findById(keyId)
                .filter(k -> k.getUser().getId().equals(user.getId()))
                .orElseThrow(() -> ApiException.of(ErrorCodes.API_KEY_NOT_FOUND));

        apiKey.setActive(false);
        repository.save(apiKey);
    }

    private ApiKeyResponseDTO toResponse(ApiKey apiKey) {
        return new ApiKeyResponseDTO(
                apiKey.getId(),
                apiKey.getKeyValue(),
                apiKey.getNotes(),
                apiKey.getOrigin().name(),
                apiKey.getUser().getName() + " " + apiKey.getUser().getLastname(),
                apiKey.isActive(),
                apiKey.getCreatedAt().format(DateTimeFormatter.ISO_LOCAL_DATE_TIME)
        );
    }

    private String generateSecureApiKey() {
        SecureRandom random = new SecureRandom();
        byte[] bytes = new byte[24];
        random.nextBytes(bytes);
        String tokenPart = Base64.getUrlEncoder().withoutPadding().encodeToString(bytes);
        return UUID.randomUUID() + "-" + tokenPart;
    }
}
