package com.fyno.api.routes.v1.integration.webhook.dto.response;

import java.time.LocalDateTime;
import java.util.List;

public record WebhookResponseDTO(
        String id,
        String name,
        String url,
        List<String> events,
        LocalDateTime createdAt
) {}
