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

@RestController
@RequestMapping("/v1/integration/apikey")
public class ApiKeyController {

    private final ApiKeyService service;

    public ApiKeyController(ApiKeyService service) {
        this.service = service;
    }

    @Operation(summary = "Create a new API key", description = "Generate a new API key for the authenticated user")
    @ApiResponses({
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "200", description = "API key created successfully"),
            @io.swagger.v3.oas.annotations.responses.ApiResponse(responseCode = "401", description = "Unauthorized"),
    })
    @PostMapping("/create")
    public ApiResponse<ApiKeyResponseDTO> create(
            @CurrentUser AuthenticatedUser user,
            @RequestBody ApiKeyRequestDTO dto,
            HttpServletRequest req
    ) {
        if (user == null) throw ApiException.of(ErrorCodes.UNAUTHORIZED);

        var result = service.createApiKey(user.getEmail(), dto);
        return ApiResponse.ok(result, req.getRequestURI(), null);
    }

    @Operation(summary = "Get all API keys", description = "Return all API keys for the authenticated user")
    @GetMapping("/list")
    public ApiResponse<List<ApiKeyResponseDTO>> list(
            @CurrentUser AuthenticatedUser user,
            HttpServletRequest req
    ) {
        if (user == null) throw ApiException.of(ErrorCodes.UNAUTHORIZED);

        var result = service.listKeys(user.getEmail());
        return ApiResponse.ok(result, req.getRequestURI(), null);
    }

    @Operation(summary = "Revoke API key", description = "Deactivate an API key by its ID for the authenticated user")
    @PostMapping("/revoke/{keyId}")
    public ApiResponse<Void> revoke(
            @CurrentUser AuthenticatedUser user,
            @PathVariable String keyId,
            HttpServletRequest req
    ) {
        if (user == null) throw ApiException.of(ErrorCodes.UNAUTHORIZED);

        service.revokeKey(keyId, user.getEmail());
        return ApiResponse.ok(null, req.getRequestURI(), null);
    }
}
