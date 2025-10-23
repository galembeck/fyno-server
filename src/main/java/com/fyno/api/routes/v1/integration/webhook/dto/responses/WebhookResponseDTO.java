package com.fyno.api.routes.v1.integration.webhook.dto.responses;

import java.time.LocalDateTime;
import java.util.List;

public record WebhookResponseDTO(
        String id,
        String name,
        String url,
        List<String> events,
        LocalDateTime createdAt
) {}
