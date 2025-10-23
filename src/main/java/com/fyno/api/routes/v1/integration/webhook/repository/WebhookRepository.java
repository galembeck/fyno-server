package com.fyno.api.routes.v1.integration.webhook.repository;

import com.fyno.api.routes.v1.integration.webhook.entity.Webhook;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface WebhookRepository extends JpaRepository<Webhook, String> {
    List<Webhook> findByUserId(String userId);
}
