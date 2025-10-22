package com.fyno.api.routes.v1.integration.apikey.dto.response;

public record ApiKeyResponseDTO(
   String id,
   String key,
   String notes,
   String origin,
   String createdBy,
   boolean active,
   String createdAt
) {}
