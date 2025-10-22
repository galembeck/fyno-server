package com.fyno.api.common;

import com.fasterxml.jackson.annotation.JsonInclude;
import lombok.Builder;

import java.time.Instant;
import java.util.Map;

@Builder
@JsonInclude
public record ApiResponse<T>(
    boolean success,
    T data,
    ApiError error,
    Instant timestamp,
    String path,
    String requestId
) {
    public static <T> ApiResponse<T> ok(T data, String path, String requestId) {
        return ApiResponse.<T>builder()
                .success(true)
                .data(data)
                .timestamp(Instant.now())
                .path(path)
                .requestId(requestId)
                .build();
    }

    public static <T> ApiResponse<T> fail(ApiError error, String path, String requestId) {
        return ApiResponse.<T>builder()
                .success(false)
                .error(error)
                .timestamp(Instant.now())
                .path(path)
                .requestId(requestId)
                .build();
    }

    @Builder
    @JsonInclude(JsonInclude.Include.NON_NULL)
    public record ApiError(
            String code,
            String message,
            Map<String, Object> details
    ) {}
}
