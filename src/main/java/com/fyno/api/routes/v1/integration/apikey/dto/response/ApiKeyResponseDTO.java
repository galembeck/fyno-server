package com.fyno.api.routes.v1.integration.apikey.dto.response;

public record ApiKeyResponseDTO(
        String id,
        String keyValue,
        String notes,
        String origin,
        boolean active,
        String createdAt
) {}
