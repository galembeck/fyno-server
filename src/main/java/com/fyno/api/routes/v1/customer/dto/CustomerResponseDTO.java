package com.fyno.api.routes.v1.customer.dto;

import java.time.LocalDateTime;

public record CustomerResponseDTO(
        String id,
        String name,
        String email,
        String phone,
        String document,
        String address,
        LocalDateTime createdAt
) {
}
