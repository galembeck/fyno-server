package com.fyno.api.routes.v1.product.dto.response;

import java.math.BigDecimal;
import java.time.LocalDateTime;

public record ProductResponseDTO(
        String id,
        String name,
        String description,
        BigDecimal price,
        LocalDateTime createdAt
) {}
