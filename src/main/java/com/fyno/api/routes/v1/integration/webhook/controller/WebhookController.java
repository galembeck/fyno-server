package com.fyno.api.routes.v1.integration.webhook.controller;

import com.fyno.api.common.ApiResponse;
import com.fyno.api.routes.v1.integration.webhook.dto.requests.WebhookRequestDTO;
import com.fyno.api.routes.v1.integration.webhook.dto.responses.WebhookResponseDTO;
import com.fyno.api.routes.v1.integration.webhook.service.WebhookService;
import com.fyno.api.security.entity.AuthenticatedUser;
import com.fyno.api.security.entity.CurrentUser;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/v1/integration/webhook")
public class WebhookController {

    private final WebhookService service;

    public WebhookController(WebhookService service) {
        this.service = service;
    }

    @Operation(summary = "Create new webhook", description = "Register a new URL to receive event notifications")
    @ApiResponses({
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "200", description = "Webhook created successfully"),
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "401", description = "Unauthorized")
    })
    @PostMapping("/create")
    public ApiResponse<WebhookResponseDTO> create(
            @CurrentUser AuthenticatedUser user,
            @RequestBody WebhookRequestDTO dto,
            HttpServletRequest req
    ) {
        var result = service.createWebhook(user.getEmail(), dto);
        return ApiResponse.ok(result, req.getRequestURI(), null);
    }

    @Operation(summary = "List all webhooks", description = "Return all webhooks registered by the user")
    @GetMapping("/list")
    public ApiResponse<List<WebhookResponseDTO>> list(
            @CurrentUser AuthenticatedUser user,
            HttpServletRequest req
    ) {
        var result = service.listWebhooks(user.getEmail());
        return ApiResponse.ok(result, req.getRequestURI(), null);
    }

    @Operation(summary = "Remove webhook", description = "Remove a registered webhook by its ID")
    @DeleteMapping("/delete/{id}")
    public ApiResponse<Void> delete(
            @CurrentUser AuthenticatedUser user,
            @PathVariable String id,
            HttpServletRequest req
    ) {
        service.deleteWebhook(user.getEmail(), id);
        return ApiResponse.ok(null, req.getRequestURI(), null);
    }
}
