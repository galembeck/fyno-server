package com.fyno.api.routes.v1.customer.dto;

import io.swagger.v3.oas.annotations.media.Schema;

@Schema(description = "Payload for creating a new customer")
public record CreateCustomerDTO(
        @Schema(description = "Client's full name", example = "Jhon Doe")
        String name,

        @Schema(description = "Client's email", example = "jhon@email.com")
        String email,

        @Schema(description = "Client's phone", example = "11987654321")
        String phone,

        @Schema(description = "Client's document (CPF or CNPJ)", example = "12345678909")
        String document,

        @Schema(description = "Client's complete address", example = "Flower Street, 123")
        String address
) {}
