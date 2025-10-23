package com.fyno.api.routes.v1.integration.webhook.service.impl;

import com.fyno.api.exception.ApiException;
import com.fyno.api.exception.ErrorCodes;
import com.fyno.api.routes.user.entity.User;
import com.fyno.api.routes.user.repository.UserRepository;
import com.fyno.api.routes.v1.integration.webhook.dto.requests.WebhookRequestDTO;
import com.fyno.api.routes.v1.integration.webhook.dto.responses.WebhookResponseDTO;
import com.fyno.api.routes.v1.integration.webhook.entity.Webhook;
import com.fyno.api.routes.v1.integration.webhook.repository.WebhookRepository;
import com.fyno.api.routes.v1.integration.webhook.service.WebhookService;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class WebhookServiceImpl implements WebhookService {

    private final WebhookRepository repository;
    private final UserRepository userRepository;

    public WebhookServiceImpl(WebhookRepository repository, UserRepository userRepository) {
        this.repository = repository;
        this.userRepository = userRepository;
    }

    @Override
    public WebhookResponseDTO createWebhook(String userEmail, WebhookRequestDTO dto) {
        User user = userRepository.findByEmail(userEmail)
                .orElseThrow(() -> ApiException.of(ErrorCodes.USER_NOT_FOUND));

        Webhook webhook = Webhook.builder()
                .name(dto.name())
                .url(dto.url())
                .secret(dto.secret())
                .events(dto.events())
                .user(user)
                .build();

        repository.save(webhook);

        return new WebhookResponseDTO(
                webhook.getId(),
                webhook.getName(),
                webhook.getUrl(),
                webhook.getEvents(),
                webhook.getCreatedAt()
        );
    }

    @Override
    public List<WebhookResponseDTO> listWebhooks(String userEmail) {
        User user = userRepository.findByEmail(userEmail)
                .orElseThrow(() -> ApiException.of(ErrorCodes.USER_NOT_FOUND));

        return repository.findByUserId(user.getId())
                .stream()
                .map(w -> new WebhookResponseDTO(
                        w.getId(),
                        w.getName(),
                        w.getUrl(),
                        w.getEvents(),
                        w.getCreatedAt()
                ))
                .collect(Collectors.toList());
    }

    @Override
    public void deleteWebhook(String userEmail, String webhookId) {
        User user = userRepository.findByEmail(userEmail)
                .orElseThrow(() -> ApiException.of(ErrorCodes.USER_NOT_FOUND));

        Webhook webhook = repository.findById(webhookId)
                .filter(w -> w.getUser().getId().equals(user.getId()))
                .orElseThrow(() -> ApiException.of(ErrorCodes.WEBHOOK_NOT_FOUND));

        repository.delete(webhook);
    }
}
