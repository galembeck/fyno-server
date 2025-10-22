package com.fyno.api.routes.v1.integration.apikey.dto.request;

import com.fyno.api.routes.v1.integration.apikey.enums.ApiKeyOrigin;

public record ApiKeyRequestDTO(
        String notes,
        ApiKeyOrigin origin
) {}
