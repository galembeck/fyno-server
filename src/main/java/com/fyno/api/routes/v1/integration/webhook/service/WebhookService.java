package com.fyno.api.routes.v1.integration.webhook.service;

import com.fyno.api.routes.v1.integration.webhook.dto.request.WebhookRequestDTO;
import com.fyno.api.routes.v1.integration.webhook.dto.response.WebhookResponseDTO;

import java.util.List;

public interface WebhookService {
    WebhookResponseDTO createWebhook(String userEmail, WebhookRequestDTO dto);
    List<WebhookResponseDTO> listWebhooks(String userEmail);
    void deleteWebhook(String userEmail, String webhookId);
}
