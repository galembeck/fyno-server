package com.fyno.api.routes.v1.integration.apikey.controller;

import com.fyno.api.common.ApiResponse;
import com.fyno.api.exception.ApiException;
import com.fyno.api.exception.ErrorCodes;
import com.fyno.api.routes.v1.integration.apikey.dto.request.ApiKeyRequestDTO;
import com.fyno.api.routes.v1.integration.apikey.dto.response.ApiKeyResponseDTO;
import com.fyno.api.routes.v1.integration.apikey.service.ApiKeyService;
import com.fyno.api.security.entity.AuthenticatedUser;
import com.fyno.api.security.entity.CurrentUser;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.web.bind.annotation.*;

import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

@RestController
@RequestMapping("/v1/integration/apikey")
public class ApiKeyController {

    private final ApiKeyService service;

    private static final Logger log = LoggerFactory.getLogger(ApiKeyController.class);

    public ApiKeyController(ApiKeyService service) {
        this.service = service;
    }

    @Operation(summary = "Create new API key", description = "Generate a new API key for the logged-in user")
    @ApiResponses({
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "200", description = "API key created successfully"),
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "401", description = "Unauthorized")
    })
    @PostMapping("/create")
    public ApiResponse<ApiKeyResponseDTO> create(
            @CurrentUser AuthenticatedUser user,
            @RequestBody ApiKeyRequestDTO dto,
            HttpServletRequest req
    ) {
        log.info("[APIKEY] Iniciando criação de chave para usuário: {} com payload: {}", user != null ? user.getEmail() : null, dto);
        if (user == null) {
            log.warn("[APIKEY] Usuário não autenticado ao tentar criar chave");
            throw ApiException.of(ErrorCodes.UNAUTHORIZED);
        }
        try {
            var result = service.createApiKey(user.getEmail(), dto);
            log.info("[APIKEY] Chave criada com sucesso para usuário: {}", user.getEmail());
            return ApiResponse.ok(result, req.getRequestURI(), null);
        } catch (Exception e) {
            log.error("[APIKEY] Erro ao criar chave para usuário: {} - {}", user.getEmail(), e.getMessage(), e);
            throw e;
        }
    }

    @Operation(summary = "List user API keys", description = "Returns all API keys belonging to the user")
    @GetMapping("/list")
    public ApiResponse<List<ApiKeyResponseDTO>> list(
            @CurrentUser AuthenticatedUser user,
            HttpServletRequest req
    ) {
        var result = service.listKeys(user.getEmail());
        return ApiResponse.ok(result, req.getRequestURI(), null);
    }

    @Operation(summary = "Revoke an API key", description = "Deactivate an API key for the current user")
    @PostMapping("/revoke/{keyId}")
    public ApiResponse<Void> revoke(
            @CurrentUser AuthenticatedUser user,
            @PathVariable String keyId,
            HttpServletRequest req
    ) {
        service.revokeKey(keyId, user.getEmail());
        return ApiResponse.ok(null, req.getRequestURI(), null);
    }
}