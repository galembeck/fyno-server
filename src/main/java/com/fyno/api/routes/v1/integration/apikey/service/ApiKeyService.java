package com.fyno.api.routes.v1.integration.apikey.service;

import com.fyno.api.routes.v1.integration.apikey.dto.request.ApiKeyRequestDTO;
import com.fyno.api.routes.v1.integration.apikey.dto.response.ApiKeyResponseDTO;

import java.util.List;

public interface ApiKeyService {
    ApiKeyResponseDTO createApiKey(String userEmail, ApiKeyRequestDTO dto);
    List<ApiKeyResponseDTO> listKeys(String userEmail);
    void revokeKey(String keyId, String userEmail);
}
