package com.fyno.api.routes.v1.product.dto.request;

import java.math.BigDecimal;

public record ProductRequestDTO(
    String id,
    String name,
    String description,
    BigDecimal price
) {}
