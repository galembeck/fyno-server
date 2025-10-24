package com.fyno.api.routes.v1.integration.webhook.dto.request;

import java.util.List;

public record WebhookRequestDTO(
        String name,
        String url,
        String secret,
        List<String> events
) {}
